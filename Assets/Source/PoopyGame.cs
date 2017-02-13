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

#if UNITY_EDTIOR || UNITY_STANDALONE
public class PoopyGame : UnityGameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        PoopyGame game = new PoopyGame();
#if UNITY_EDITOR
        PoopyGameServer server = new PoopyGameServer();
#endif
    }

    public PoopyGame() : base()
	{	
        NetworkCommander commander = new NetworkCommander();
        commander.AddObject(entityPool);

        systemManager.AddSystem(UnitySystemBase.Initialize<VisualizationSystem>(entityPool));
        systemManager.AddSystem(new MovementSystem());

        SocketController socket = new SocketController(commander);

        NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
        networkController.Init(socket);

        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Network = socket;
        MessageSystem messageSystem = new MessageSystem();
        systemManager.AddSystem(messageSystem);
        messageSystem.OnMessageReceived += controller.OnNewMessage;
        controller.Init(entityPool, socket.UserId);

        for (int i = 0; i < 100f; i++)
        {
            Entity ent = entityPool.GetObject();
            ent.AddComponent<TransformComponent>().position = new Vector2(i, 0f);
            ent.AddComponent<VisualizationComponent>();
            ent.AddComponent<MovementComponent>().velocity = new RocketWorks.Vector2(new System.Random(i).Next(-50, 50) * .02f, 0f);
        }
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
        public PoopyGameServer() : base()
        {
            NetworkCommander commander = new NetworkCommander();
            commander.AddObject(entityPool);

            SocketController socket = new SocketController(commander);
            socket.SetupSocket();

            MessageSystem messageSystem = new MessageSystem();
            systemManager.AddSystem(messageSystem);

            for (int i = 0; i < 100f; i++)
            {
                Entity ent = entityPool.GetObject();
                ent.AddComponent<TransformComponent>().position = new Vector2(i, 0f);
                ent.AddComponent<VisualizationComponent>();
            }
    }

        public void SendMessage(string message)
        {

        }
    }
#endif