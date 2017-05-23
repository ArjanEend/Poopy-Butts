#if UNITY_EDTIOR || UNITY_5
using UnityEngine;
#endif
using RocketWorks.Base;
using RocketWorks.Entities;
using RocketWorks.Networking;
using RocketWorks.Commands;
using RocketWorks.Systems;
using Implementation.Components;
using Implementation.Systems;
using Vector2 = RocketWorks.Vector2;
using Vector3 = RocketWorks.Vector3;
using RocketWorks.Serialization;
using PoopyButts.Components;
using Assets.Source.Implementation.Systems;
using System.Runtime.InteropServices;
using Random = System.Random;
using System;

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
    private CameraController camera;

    public PoopyGame() : base()
	{
        NetworkCommander commander = new NetworkCommander();
        Rocketizer rocketizer = new Rocketizer();
        rocketizer.AddProvider(contexts.Main.Pool);
        rocketizer.AddProvider(contexts.Message.Pool);
        rocketizer.AddProvider(contexts.Meta.Pool);

        commander.AddObject(contexts.Main);
        commander.AddObject(contexts.Message);
        commander.AddObject(contexts.Meta);
        commander.AddObject(contexts.Input);

        pingView = GameObject.FindObjectOfType<PingView>();
        camera = GameObject.FindObjectOfType<CameraController>();

        systemManager.AddSystem(UnitySystemBase.Initialize<VisualizationSystem>(contexts));
        systemManager.AddSystem(UnitySystemBase.Initialize<TileMapVisualizer>(contexts));
        systemManager.AddSystem(new LerpSystem(false));
        systemManager.AddSystem(new MovementSystem());
        systemManager.AddSystem(new UpdateUnits());

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
        controller.Init(contexts.Message.Pool, socket.UserId);
    }

    private void OnUserID(int id)
    {
        PingSystem pingSystem = new PingSystem(socket);
        systemManager.AddSystem(pingSystem);
        pingView.Initialize(pingSystem);

        systemManager.AddSystem(new MoveInputSystem(id));
        systemManager.AddSystem(new SendEntitiesSystem<AxisComponent, InputContext>(socket, true));
        systemManager.AddSystem(new SendEntitiesSystem<ButtonComponent, InputContext>(socket, false, true));

        var playerDispatch = systemManager.AddSystem(new DispatchLocal<VisualizationComponent, MainContext>(socket.UserId));
        playerDispatch.ComponentUpdated += camera.Initialize;
    }

    public override void UpdateGame(float deltaTime)
    {
        base.UpdateGame(deltaTime);
        socket.Update();
    }

    private void OnUserConnected(int userId)
    {
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Init(contexts.Message.Pool, userId);
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
        rocketizer.AddProvider(contexts.Message.Pool);
        rocketizer.AddProvider(contexts.Meta.Pool);
        rocketizer.AddProvider(contexts.Input.Pool);

        commander.AddObject(contexts.Main);
        commander.AddObject(contexts.Message);
        commander.AddObject(contexts.Meta);
        commander.AddObject(contexts.Input);

        socket = new SocketController(commander, rocketizer);
        socket.SetupSocket(true, "127.0.0.1", 9001
#if !UNITY_EDITOR
                , true
#endif
                );

        SendWorldSystem sendWorld = new SendWorldSystem(socket);
        systemManager.AddSystem(sendWorld);

        PingSystem pingSystem = new PingSystem(socket);
        systemManager.AddSystem(pingSystem);

        systemManager.AddSystem(new PlayerMoveSystem());
        socket.UserConnectedEvent += OnUserConnected;

        MessageSystem messageSystem = new MessageSystem();
        systemManager.AddSystem(messageSystem);
        messageSystem.OnNewEntity += OnNewMessage;

        //systemManager.AddSystem(new TilemapCollision());
        systemManager.AddSystem(new MovementSystem());
        systemManager.AddSystem(new PhysicsSystem());
        //systemManager.AddSystem(new CircleCollisionSystem());
        systemManager.AddSystem(new SpawnUnits(socket));
        systemManager.AddSystem(new UpdateInfluence());
        systemManager.AddSystem(new UpdateUnits());
        systemManager.AddSystem(new SpawnTilemap());
        systemManager.AddSystem(new LerpSystem(true));
        systemManager.AddSystem(new EstimateComponentsSystem<LerpToComponent,
            MainContext>(socket));

        Random random = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < 8; i++)
        {
            Entity spawner = contexts.Main.Pool.GetObject();
            spawner.AddComponent<TransformComponent>().position = new Vector3(random.Next(-100, 100) * .1f, 0f, random.Next(-100, 100) * .1f);
            spawner.AddComponent<VisualizationComponent>().resourceId = "Turd";
            spawner.AddComponent<OwnerComponent>();
            spawner.AddComponent<CircleCollider>().radius = .25f;
            spawner.AddComponent<SpawnerComponent>().interval = 5f;
            spawner.AddComponent<TriggerComponent>().radius = 1f;
        }

        Entity newEnt = contexts.Meta.Pool.GetObject();
        newEnt.AddComponent<PlayerIdComponent>().id = -1;
    }

    private void OnNewMessage(Entity obj)
    {
        socket.WriteSocket(new MessageContextCreateEntityCommand(obj));
    }

    public override void UpdateGame(float deltaTime)
    {
        base.UpdateGame(deltaTime);
        socket.Update();
    }

    private void OnUserConnected(int obj)
    {
        RocketLog.Log("User: " + obj, this);
        Entity ent = contexts.Meta.Pool.GetObject(true);
        ent.AddComponent<PlayerIdComponent>().id = obj;
        ent.AddComponent<PlayerInfo>().name = "Player " + obj;

        Entity playerObj = contexts.Main.Pool.GetObject();
        playerObj.AddComponent<TransformComponent>().position = new Vector2(0f, 0f);
        playerObj.AddComponent<MovementComponent>().velocity = new Vector2(0f, 0f);
        playerObj.AddComponent<VisualizationComponent>().resourceId = "character";
        playerObj.AddComponent<PlayerIdComponent>().id = obj;
        playerObj.AddComponent<LerpToComponent>();
        playerObj.AddComponent<CircleCollider>().radius = .25f;
    }

        public void SendMessage(string message)
        {

        }
    }
#endif