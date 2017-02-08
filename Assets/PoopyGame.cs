using UnityEngine;
using System.Collections;
using RocketWorks.Base;
using RocketWorks.Entities;
using RocketWorks.Networking;
using System;
using RocketWorks.Commands;

public class PoopyGame : UnityGameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        new PoopyGame();
	}

    public PoopyGame() : base()
	{	
		/*TestSystem system = new TestSystem ();
		systemManager.AddSystem (system);
        systemManager.AddSystem(new MoveInputSystem(0));
        systemManager.AddSystem(new MoveUpdateSystem());*/

        NetworkCommander commander = new NetworkCommander();
        commander.AddObject(entityPool);

        SocketController socket = new SocketController(commander);

        NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
        networkController.Init(socket);

        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Network = socket;
        MessageSystem messageSystem = new MessageSystem();
        systemManager.AddSystem(messageSystem);
        messageSystem.OnMessageReceived += controller.OnNewMessage;
        controller.Init(entityPool, socket.UserId);

        for (int i = 0; i < 100; i++) 
		{
			/*Entity ent = entityPool.GetObject ();
			TestComponent comp = ent.AddComponent<TestComponent> (new TestComponent());
			comp.Offset = i * 1.5f;
			GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
			go.transform.position = new Vector3 (i, 0f, 0f);
			comp.visuals = go.transform;
            if(i % 23 == 0)
            {
                var playerComp = ent.AddComponent<PlayerIdComponent>(new PlayerIdComponent());
                playerComp.id = 0;
            }*/
		}

    }

    private void OnUserConnected(int userId)
    {
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Init(entityPool, userId);
    }
}
