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
    private Group pingGroup;
    private Group pongGroup;

    public delegate void PingEvent(long ping);
    public event PingEvent OnPingUpdated = delegate { };

    private SocketController socket;

    private Contexts contexts;

    private Entity pingEntity;

    public PingSystem(SocketController socket)
    {
        this.socket = socket;
    }

    public override void Initialize(Contexts context)
    {
        this.contexts = context;
        tickRate = 3f;
        EntityPool pool = context.Meta.Pool;
        pingGroup = pool.GetGroup(typeof(PingComponent));
        pingGroup.OnEntityAdded += OnNewEntity;
        pongGroup = pool.GetGroup(typeof(PongComponent));
        pongGroup.OnEntityAdded += OnNewPong;

        pingEntity = contexts.Meta.Pool.GetObject();

        if (socket.UserId == -1)
            pingEntity.AddComponent<PongComponent>();
        else
            pingEntity.AddComponent<PingComponent>();

        socket.WriteSocket(new MetaContextCreateEntityCommand(pingEntity));
    }

    private void OnNewPong(Entity obj)
    {
        RocketLog.Log("Pong! " + obj.CreationIndex);
        if (socket.UserId == -1)
            return;
        if (pingEntity == null || pingEntity.GetComponent<PingComponent>() == null)
            return;
        long lastPongRecieve = (long)(ServerTimeStamp.ServerNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        OnPingUpdated(lastPongRecieve - pingEntity.GetComponent<PingComponent>().toTicks);
    }

    private void OnNewEntity(Entity obj)
    {
        if (socket.UserId != -1)
            return;
        pingEntity.GetComponent<PongComponent>().toTicks = (long)(ServerTimeStamp.ServerNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        socket.WriteSocket(new MetaContextUpdateComponentCommand(pingEntity.GetComponent<PongComponent>(), pingEntity.CreationIndex), obj.Owner);
        RocketLog.Log("Send Pong to: " + pingEntity.Owner);
    }

    public override void Destroy()
    {
    }

    public override void Execute(float deltaTime)
    {
        if(socket.UserId != -1 && pingEntity.Alive)
        {
            RocketLog.Log("Update ping");
            pingEntity.GetComponent<PingComponent>().toTicks = (long)(ServerTimeStamp.ServerNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            socket.WriteSocket(new MetaContextUpdateComponentCommand(pingEntity.GetComponent<PingComponent>(), pingEntity.CreationIndex));
        }
    }


}
