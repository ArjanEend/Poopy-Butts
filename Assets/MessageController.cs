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
        Entity entity = entityPool.GetObject();
        MessageComponent comp = new MessageComponent();
        comp.message = messageField.text;
        comp.userId = userId;
        comp.timeStamp = DateTime.Now;
        entity.AddComponent<MessageComponent>(comp);

        network.WriteSocket<EntityPool>(new CreateEntityCommand(entity));

        messageField.text = "";
    }

    public void OnNewMessage(int user, string message, DateTime time)
    {
        messageContainer.text += time.ToString("hh:mm") + " - " + user + ": " + message + " \n";
    }
}
