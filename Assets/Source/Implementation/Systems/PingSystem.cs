using RocketWorks.Systems;
using System.Collections.Generic;
using RocketWorks.Pooling;
using System;
using RocketWorks.Grouping;
using RocketWorks.Entities;
using Implementation.Components;

public class PingSystem : SystemBase
{
    private Group userGroup;
    private Entity serverEntity;

    public delegate void PingEvent(long ping);
    public event PingEvent OnPingUpdated = delegate { };   

    public override void Initialize(Contexts context)
    {
        tickRate = 1f;
        EntityPool pool = context.MainContext.Pool;
        userGroup = pool.GetGroup(typeof(PlayerIdComponent), typeof(PingComponent));
    }

    private void OnNewMessage(Entity obj)
    {
        if (obj == serverEntity)
            return;

        PingComponent comp = obj.GetComponent<PingComponent>();
        OnPingUpdated(comp.ticks - serverEntity.GetComponent<PingComponent>().ticks);
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Execute()
    {
        List<Entity> messages = userGroup.Entities;
        
        for (int i = 0; i < messages.Count; i++)
        {
            if (serverEntity == null)
            {
                if(messages[i].GetComponent<PlayerIdComponent>().id == -1)
                {
                    serverEntity = messages[i];
                    RocketLog.Log("server entity found");
                }
            }
            else
                OnNewMessage(messages[i]);
        }
    }


}
