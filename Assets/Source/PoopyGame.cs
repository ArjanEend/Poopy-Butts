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
using RocketWorks.Serialization;
using PoopyButts.Components;
using Assets.Source.Implementation.Systems;

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
    private FoodBarController foodBar;

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
        foodBar = GameObject.FindObjectOfType<FoodBarController>();

        systemManager.AddSystem(UnitySystemBase.Initialize<VisualizationSystem>(contexts));
        systemManager.AddSystem(UnitySystemBase.Initialize<PoopVisualizer>(contexts));
        systemManager.AddSystem(UnitySystemBase.Initialize<TileMapVisualizer>(contexts));
        systemManager.AddSystem(new LerpSystem(false));
        //systemManager.AddSystem(new TilemapCollision());
        systemManager.AddSystem(new MovementSystem());
        systemManager.AddSystem(new UpdatePoops());
        systemManager.AddSystem(new CircleCollisionSystem());
        systemManager.AddSystem(UnitySystemBase.Initialize<EatinSystem>(contexts));

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

        var system = systemManager.AddSystem(new DispatchLocal<Stomach, MainContext>(socket.UserId));
        system.ComponentUpdated += foodBar.UpdateDisplay;
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
                ,true
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
        systemManager.AddSystem(new CircleCollisionSystem());
        systemManager.AddSystem(new PickupSystem(socket));
        systemManager.AddSystem(new PoopSystem(socket));
        systemManager.AddSystem(new UpdatePoops());
        systemManager.AddSystem(new SpawnPickupSystem(socket));
        systemManager.AddSystem(new SpawnTilemap());
        systemManager.AddSystem(new LerpSystem(true));
        systemManager.AddSystem(new EstimateComponentsSystem<LerpToComponent,
            MainContext>(socket));

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

        Entity playerObj = contexts.Main.Pool.GetObject();
        playerObj.AddComponent<TransformComponent>().position = new Vector2(0f, 0f);
        playerObj.AddComponent<MovementComponent>().velocity = new Vector2(0f, 0f);
        playerObj.AddComponent<VisualizationComponent>().resourceId = "character";
        playerObj.AddComponent<PlayerIdComponent>().id = obj;
        playerObj.AddComponent<LerpToComponent>();
        playerObj.AddComponent<CircleCollider>().radius = .15f;
        playerObj.AddComponent<Stomach>();
    }

        public void SendMessage(string message)
        {

        }
    }
#endif