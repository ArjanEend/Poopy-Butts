using UnityEngine;
using System.Collections;
using RocketWorks.Base;
using RocketWorks.Entities;
using RocketWorks.Networking;
using System;

public class PoopyGame : GameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        new PoopyGame();
	}

    public PoopyGame() : base()
	{	
		TestSystem system = new TestSystem ();
		systemManager.AddSystem (system);
        systemManager.AddSystem(new MoveInputSystem(0));
        systemManager.AddSystem(new MoveUpdateSystem());

        NetworkController network = GameObject.FindObjectOfType<NetworkController>();
        network.EntityPool = entityPool;
        network.OnUserConnected += OnUserConnected;
        
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        MessageSystem messageSystem = new MessageSystem();
        messageSystem.Network = network;
        systemManager.AddSystem(messageSystem);
        messageSystem.OnMessageReceived += controller.OnNewMessage;
        
		for (int i = 0; i < 100; i++) 
		{
			Entity ent = entityPool.GetObject ();
			TestComponent comp = ent.AddComponent<TestComponent> (new TestComponent());
			comp.Offset = i * 1.5f;
			GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
			go.transform.position = new Vector3 (i, 0f, 0f);
			comp.visuals = go.transform;
            if(i % 23 == 0)
            {
                var playerComp = ent.AddComponent<PlayerIdComponent>(new PlayerIdComponent());
                playerComp.id = 0;
            }
		}

    }

    private void OnUserConnected(int userId)
    {
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Init(entityPool, userId);
    }
}
