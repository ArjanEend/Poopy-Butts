using RocketWorks.Systems;
using System.Collections.Generic;
using RocketWorks.Pooling;
using System;
using RocketWorks.Grouping;
using RocketWorks.Entities;

public class MessageSystem : SystemBase
{
    private Group group;
    private Group userGroup;
    
    public delegate void MessageEvent(int user, string message, DateTime time);
    public event MessageEvent OnMessageReceived = delegate { };
    public Action<Entity> OnNewEntity = delegate { };

    private int messageIndex;

    public override void Initialize(Contexts context)
    {
        EntityPool pool = context.Main.Pool;
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
        throw new NotImplementedException();
    }

    public override void Execute(float deltaTime)
    {
        List<Entity> messages = group.NewEntities;
        for (int i = 0; i < messages.Count; i++)
        {
            OnNewMessage(messages[i]);
            OnNewEntity(messages[i]);
        }
    }

    
}
