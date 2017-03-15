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
        EntityPool pool = context.MainContext.Pool;
        pingGroup = pool.GetGroup(typeof(PingComponent));
        pingGroup.OnEntityAdded += OnNewEntity;
        pongGroup = pool.GetGroup(typeof(PongComponent));
        pongGroup.OnEntityAdded += OnNewPong;

        pingEntity = contexts.MainContext.Pool.GetObject();

        if (socket.UserId == -1)
            pingEntity.AddComponent<PongComponent>();
        else
            pingEntity.AddComponent<PingComponent>();

        socket.WriteSocket(new MainContextCTXCreateEntityComman(pingEntity));
    }

    private void OnNewPong(Entity obj)
    {
        RocketLog.Log("Pong! " + obj.CreationIndex);
        if (socket.UserId == -1)
            return;
        if (pingEntity == null || pingEntity.GetComponent<PingComponent>() == null)
            return;
        long lastPongRecieve = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
        OnPingUpdated(pingEntity.GetComponent<PingComponent>().toTicks - lastPongRecieve);
    }

    private void OnNewEntity(Entity obj)
    {
        if (socket.UserId != -1)
            return;
        pingEntity.GetComponent<PongComponent>().toTicks = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
        socket.WriteSocket(new UpdateComponentCommand(pingEntity.GetComponent<PongComponent>(), pingEntity.CreationIndex));
        RocketLog.Log("Send Pong");
    }

    public override void Destroy()
    {
    }

    public override void Execute()
    {
        if(socket.UserId != -1)
        {
            RocketLog.Log("Update ping");
            pingEntity.GetComponent<PingComponent>().toTicks = (long)(new DateTime(1970, 1, 1) - DateTime.UtcNow).TotalMilliseconds;
            socket.WriteSocket(new UpdateComponentCommand(pingEntity.GetComponent<PingComponent>(), pingEntity.CreationIndex));
        }
    }


}
