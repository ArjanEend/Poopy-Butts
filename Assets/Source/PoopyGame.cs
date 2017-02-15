#if UNITY_EDTIOR || UNITY_STANDALONE
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

#if UNITY_EDTIOR || UNITY_STANDALONE
public class PoopyGame : UnityGameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        PoopyGame game = new PoopyGame();
#if UNITY_EDITOR
        PoopyGameServer server = new PoopyGameServer();
#endif
    }

    private SocketController socket;

    public PoopyGame() : base()
	{	
        NetworkCommander commander = new NetworkCommander();
        commander.AddObject(entityPool);

        systemManager.AddSystem(UnitySystemBase.Initialize<VisualizationSystem>(entityPool));
        

        socket = new SocketController(commander);

        NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
        networkController.Init(socket);

        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Network = socket;
        MessageSystem messageSystem = new MessageSystem();
        systemManager.AddSystem(messageSystem);
        messageSystem.OnMessageReceived += controller.OnNewMessage;
        controller.Init(entityPool, socket.UserId);

        /*for (int i = 0; i < 100f; i++)
        {
            Entity ent = entityPool.GetObject();
            ent.AddComponent<TransformComponent>().position = new Vector2(i, 0f);
            ent.AddComponent<VisualizationComponent>();
            ent.AddComponent<MovementComponent>().velocity = new RocketWorks.Vector2(new System.Random(i).Next(-50, 50) * .02f, 0f);
        }*/
    }

    public override void UpdateGame(float deltaTime)
    {
        base.UpdateGame(deltaTime);
        socket.Update();
    }

    private void OnUserConnected(int userId)
    {
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Init(entityPool, userId);
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
            commander.AddObject(entityPool);

            socket = new SocketController(commander);
            socket.SetupSocket();

        SendWorldSystem sendWorld = new SendWorldSystem(socket);
        systemManager.AddSystem(sendWorld);

        socket.UserConnectedEvent += OnUserConnected;

            MessageSystem messageSystem = new MessageSystem();
            systemManager.AddSystem(messageSystem);
        systemManager.AddSystem(new MovementSystem());
        systemManager.AddSystem(new SendComponentsSystem<TransformComponent>(socket));

        for (int i = 0; i < 25; i++)
        {
            Entity ent = entityPool.GetObject();
            ent.AddComponent<TransformComponent>().position = new Vector2(i, 0f);
            ent.AddComponent<MovementComponent>().velocity = new Vector2(new System.Random(i).Next(-50, 50) * .02f, 0f);
            ent.AddComponent<VisualizationComponent>();
        }
    }

    public override void UpdateGame(float deltaTime)
    {
        base.UpdateGame(deltaTime);
        socket.Update();
    }

    private void OnUserConnected(uint obj)
        {
        RocketLog.Log("User: " + obj, this);
            Entity ent = entityPool.GetObject();
            ent.AddComponent<PlayerIdComponent>().id = obj;
        }

        public void SendMessage(string message)
        {

        }
    }
#endif