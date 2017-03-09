using RocketWorks.Systems;
using System.Collections.Generic;
using RocketWorks.Pooling;
using System;
using RocketWorks.Grouping;
using RocketWorks.Entities;
using Implementation.Components;
using RocketWorks;
using RocketWorks.Networking;
using RocketWorks.Commands;

public class PingSystem : SystemBase
{
    private Group userGroup;
    private Entity serverEntity;

    public delegate void PingEvent(long ping);
    public event PingEvent OnPingUpdated = delegate { };

    private SocketController socket;

    private long lastPongRecieve;

    public PingSystem(SocketController socket)
    {
        this.socket = socket;
    }

    public override void Initialize(Contexts context)
    {
        tickRate = 1f;
        EntityPool pool = context.MainContext.Pool;
        userGroup = pool.GetGroup(typeof(PlayerIdComponent), typeof(PingComponent), typeof(PongComponent));
        userGroup.OnEntityAdded += OnNewEntity;
    }

    private void OnNewEntity(Entity obj)
    {
        if (socket.UserId == -1 && obj != serverEntity)
        {
            obj.GetComponent<PongComponent>().toTicks = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
            socket.WriteSocket(new UpdateComponentCommand(obj.GetComponent<PongComponent>(), obj.CreationIndex));
        }
        if (obj.GetComponent<PlayerIdComponent>().id == socket.UserId)
        {
            lastPongRecieve = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
        }
    }

    private void OnNewMessage(Entity obj)
    {
        if (obj == serverEntity)
            return;

        if (socket.UserId != -1 && obj.GetComponent<PlayerIdComponent>().id == socket.UserId)
        {
            obj.GetComponent<PingComponent>().toTicks = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
            socket.WriteSocket(new UpdateComponentCommand(obj.GetComponent<PingComponent>(), obj.CreationIndex));
        }

        PingComponent ping = obj.GetComponent<PingComponent>();
        if (obj.GetComponent<PlayerIdComponent>().id == socket.UserId)
        {
            OnPingUpdated(lastPongRecieve - ping.toTicks);
        }
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
                RocketLog.Log("Check server Entity " + messages[i].GetComponent<PlayerIdComponent>().id);
                if (messages[i].GetComponent<PlayerIdComponent>().id == -1)
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
