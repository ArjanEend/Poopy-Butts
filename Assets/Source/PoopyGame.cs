#if UNITY_EDTIOR || UNITY_5
using UnityEngine;
#endif
using System.Collections;
using RocketWorks.Base;
using RocketWorks.Entities;
using RocketWorks.Networking;
using System;
using RocketWorks.Commands;
using RocketWorks.Systems;
using Implementation.Components;
using Implementation.Systems;
using RocketWorks;
using Vector2 = RocketWorks.Vector2;
using RocketWorks.Serialization;

#if UNITY_EDTIOR || UNITY_5
public class PoopyGame : UnityGameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        PoopyGame game = new PoopyGame();
#if UNITY_EDITOR
        //PoopyGameServer server = new PoopyGameServer();
#endif
    }

    private SocketController socket;
    private PingView pingView;

    public PoopyGame() : base()
	{
        NetworkCommander commander = new NetworkCommander();
        Rocketizer rocketizer = new Rocketizer();
        rocketizer.AddProvider(contexts.Main.Pool);
        commander.AddObject(contexts.Main);

        pingView = GameObject.FindObjectOfType<PingView>();

        systemManager.AddSystem(UnitySystemBase.Initialize<VisualizationSystem>(contexts));
        systemManager.AddSystem(new MovementSystem());

        socket = new SocketController(commander, rocketizer);
        socket.UserConnectedEvent += OnUserConnected;
        socket.UserIDSetEvent += OnUserID;

        NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
        networkController.Init(socket);

        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Network = socket;
        MessageSystem messageSystem = new MessageSystem();
        systemManager.AddSystem(messageSystem);
        messageSystem.OnMessageReceived += controller.OnNewMessage;
        controller.Init(contexts.Main.Pool, socket.UserId);
    }

    private void OnUserID(int id)
    {
        PingSystem pingSystem = new PingSystem(socket);
        systemManager.AddSystem(pingSystem);
        pingView.Initialize(pingSystem);

        systemManager.AddSystem(new MoveInputSystem(id));
        systemManager.AddSystem(new SendEntitiesSystem<AxisComponent, MainContext>(socket));
    }

    public override void UpdateGame(float deltaTime)
    {
        base.UpdateGame(deltaTime);
        socket.Update();
    }

    private void OnUserConnected(int userId)
    {
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Init(contexts.Main.Pool, userId);
    }
}
#endif
#if !UNTIY_STANDALONE
public class PoopyGameServer :
#if !UNITY_EDITOR
    GameBase
#else
    UnityGameBase
#endif
{
    private SocketController socket;
        public PoopyGameServer() : base()
        {
            NetworkCommander commander = new NetworkCommander();
            Rocketizer rocketizer = new Rocketizer();

        rocketizer.AddProvider(contexts.Main.Pool);

        commander.AddObject(contexts.Main);

            socket = new SocketController(commander, rocketizer);
            socket.SetupSocket();

        SendWorldSystem sendWorld = new SendWorldSystem(socket);
        systemManager.AddSystem(sendWorld);

        PingSystem pingSystem = new PingSystem(socket);
        systemManager.AddSystem(pingSystem);

        systemManager.AddSystem(new PlayerMoveSystem());
        socket.UserConnectedEvent += OnUserConnected;

            MessageSystem messageSystem = new MessageSystem();
            systemManager.AddSystem(messageSystem);
        messageSystem.OnNewEntity += OnNewMessage;
        systemManager.AddSystem(new MovementSystem());
        systemManager.AddSystem(new SendComponentsSystem<TransformComponent,
            EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, PingComponent, PongComponent>>(socket));

        for (int i = 0; i < 25; i++)
        {
            Entity ent = contexts.Main.Pool.GetObject();
            ent.AddComponent<TransformComponent>().position = new Vector2(i, 0f);
            ent.AddComponent<MovementComponent>().velocity = new Vector2(new System.Random(i).Next(-25, 25) * .002f, 0f);
            ent.GetComponent<MovementComponent>().friction = .0005f;
            ent.AddComponent<VisualizationComponent>();
        }

        Entity newEnt = contexts.Main.Pool.GetObject();
        newEnt.AddComponent<PlayerIdComponent>().id = -1;
        //newEnt.AddComponent<PingComponent>().toTicks = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
        //newEnt.AddComponent<PongComponent>().toTicks = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
    }

    private void OnNewMessage(Entity obj)
    {
        socket.WriteSocket(new MainContextCreateEntityCommand(obj));
    }

    public override void UpdateGame(float deltaTime)
    {
        base.UpdateGame(deltaTime);
        socket.Update();
    }

    private void OnUserConnected(int obj)
    {
        RocketLog.Log("User: " + obj, this);
        Entity ent = contexts.Main.Pool.GetObject(true);
        ent.AddComponent<PlayerIdComponent>().id = obj;

        Entity playerObj = contexts.Main.Pool.GetObject();
        playerObj.AddComponent<TransformComponent>().position = new Vector2(0f, 0f);
        playerObj.AddComponent<MovementComponent>().velocity = new Vector2(0f, 0f);
        playerObj.GetComponent<MovementComponent>().friction = .5f;
        playerObj.AddComponent<VisualizationComponent>();
        playerObj.AddComponent<PlayerIdComponent>().id = obj;
        //ent.AddComponent<PingComponent>();
        //ent.AddComponent<PongComponent>();
    }

        public void SendMessage(string message)
        {

        }
    }
#endif