#if UNITY_EDTIOR || UNITY_STANDALONE
using UnityEngine;
#endif
using System.Collections;
using RocketWorks.Base;
using RocketWorks.Entities;
using RocketWorks.Networking;
using System;
using RocketWorks.Commands;

#if UNITY_EDTIOR || UNITY_STANDALONE
public class PoopyGame : UnityGameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        new PoopyGame();
	}

    public PoopyGame() : base()
	{	
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
    }

    private void OnUserConnected(int userId)
    {
        MessageController controller = GameObject.FindObjectOfType<MessageController>();
        controller.Init(entityPool, userId);
    }
}
#endif