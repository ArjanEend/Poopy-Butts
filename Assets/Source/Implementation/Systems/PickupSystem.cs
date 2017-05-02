using System;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using PoopyButts.Components;
using RocketWorks;
using RocketWorks.Networking;
using RocketWorks.Commands;

public class PickupSystem : SystemBase {

    private Group pickups;
    private Group players;

    private SocketController network;

    public PickupSystem(SocketController network)
    {
        this.network = network;
    }

    public override void Initialize(Contexts contexts)
    {
        base.Initialize(contexts);
        pickups = contexts.Main.Pool.GetGroup(typeof(PickupComponent), typeof(CircleCollider));
        players = contexts.Main.Pool.GetGroup(typeof(Stomach), typeof(CircleCollider));
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Execute(float deltaTime)
    {
        for (int i = 0; i < pickups.Count; i++)
        {
            MainEntity pickup = (MainEntity)pickups[i];
            if (!pickup.Enabled)
                continue;
            for (int j = 0; j < players.Count; j++)
            {
                MainEntity player = (MainEntity)players[j];

                if (pickup.TransformComponent().position == player.TransformComponent().position)
                    continue;

                Vector2 diff = pickup.TransformComponent().position - player.TransformComponent().position;
                Vector2 mid = (pickup.TransformComponent().position + player.TransformComponent().position) * .5f;
                float dist = Vector2.Distance(pickup.TransformComponent().position, player.TransformComponent().position);

                float overlap = dist - (pickup.CircleCollider().radius + player.CircleCollider().radius);
                if (overlap < 0f)
                {
                    player.GetComponent<Stomach>().pickups.Add(pickup);
                    pickup.Enabled = false;
                    network.WriteSocket(new MainContextUpdateComponentCommand(player.Stomach(), player.CreationIndex));
                    RocketLog.Log("Player picked up an object!", this);
                }
            }
        }
    }
}
