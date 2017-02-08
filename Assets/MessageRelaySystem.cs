using RocketWorks.Commands;
using RocketWorks.Entities;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Pooling;
using RocketWorks.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageRelaySystem : SystemBase {
    private Group group;
    private Group userGroup;

    public delegate void MessageEvent(int user, string message, DateTime time);
    public event MessageEvent OnMessageReceived = delegate { };

    private int messageIndex;

    private SocketController controller;

    public MessageRelaySystem(SocketController controller)
    {
        this.controller = controller;
    }

    public override void Initialize(EntityPool pool)
    {
        messageIndex = pool.GetIndexOf(typeof(MessageComponent));
        group = pool.GetGroup(typeof(MessageComponent));
        userGroup = pool.GetGroup(typeof(PlayerIdComponent));
    }

    private void OnNewMessage(Entity obj)
    {
        MessageComponent comp = obj.GetComponent<MessageComponent>(messageIndex);
        OnMessageReceived(comp.userId, comp.message, comp.timeStamp);
    }

    public override void Destroy()
    {
    }

    public override void Execute()
    {
        List<Entity> messages = group.NewEntities;
        for (int i = 0; i < messages.Count; i++)
        {
            controller.WriteSocket(new CreateEntityCommand(messages[i]));
        }
    }
}
