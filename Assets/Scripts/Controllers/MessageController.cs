using RocketWorks.Commands;
using RocketWorks.Entities;
using RocketWorks.Networking;
using RocketWorks.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {
    [SerializeField]
    private InputField messageField;

    [SerializeField]
    private Text messageContainer;

    [SerializeField]
    private Button button;

    private EntityPool entityPool;
    private int userId;

    private SocketController network;
    public SocketController Network
    {
        set { network = value; }
    }

    private void Start()
    {
        button.onClick.AddListener(SubmitMessage);
    }

    public void Init(EntityPool pool, int userId)
    {
        this.entityPool = pool;
        this.userId = userId;
    }

    public void SubmitMessage()
    {
        Entity entity = entityPool.GetObject(true);
        MessageComponent comp = new MessageComponent();
        comp.message = messageField.text;
        comp.userId = network.UserId;
        comp.timeStamp = DateTime.Now;
        entity.AddComponent(comp);

        network.WriteSocket(new CreateEntityCommand(entity));

        messageField.text = "";
    }

    public void OnNewMessage(int user, string message, DateTime time)
    {
        Debug.Log(time.Millisecond + " : " + DateTime.Now.Millisecond);
        messageContainer.text += time.ToString("hh:mm") + " - " + user + ": " + message + " \n";
    }
}
