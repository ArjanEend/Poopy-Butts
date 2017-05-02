using System;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Entities;
using Implementation.Components;
using RocketWorks;
using RocketWorks.Commands;
using PoopyButts.Components;

public class SpawnPickupSystem : SystemBase {

    private Group pickupGroup;

    private SocketController socket;

    private Random random;

    public SpawnPickupSystem(SocketController socket)
    {
        this.socket = socket;
    }

    public override void Initialize(Contexts contexts)
    {
        base.Initialize(contexts);
        this.tickRate = 4f;
        this.pickupGroup = contexts.Main.Pool.GetGroup(typeof(PickupComponent));
        this.random = new Random();
    }

    public override void Destroy()
    {
    }

    public override void Execute(float deltaTime)
    {
        int activePickups = 0;
        for(int i = 0; i < pickupGroup.Count; i++)
        {
            if(pickupGroup[i].Enabled)
                activePickups++;
        }

        if(activePickups < 5)
        {
            Entity pickup = contexts.Main.Pool.GetObject(true);
            pickup.AddComponent<PickupComponent>().radius = .3f;
            pickup.AddComponent<CircleCollider>().radius = .3f;
            pickup.AddComponent<TransformComponent>().position = new Vector2(-3.5f + (float)random.NextDouble() * 7f, -3.5f + (float)random.NextDouble() * 7f);
            pickup.AddComponent<VisualizationComponent>().resourceId = "Orange";
            socket.WriteSocket(new MainContextCreateEntityCommand(pickup));
        }

    }
}
