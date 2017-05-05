using System;
using RocketWorks.Systems;
using RocketWorks;
using Implementation.Components;
using RocketWorks.Grouping;
using PoopyButts.Components;

namespace Implementation.Systems
{
    class PoopCollision : SystemBase
    {
        private Group poops;
        private Group players;

        public override void Initialize(Contexts contexts)
        {
            poops = contexts.Main.Pool.GetGroup(typeof(PoopComponent), typeof(TransformComponent), typeof(CircleCollider));
            players = contexts.Main.Pool.GetGroup(typeof(TransformComponent), typeof(MovementComponent), typeof(CircleCollider));
        }

        public override void Execute(float deltaTime)
        {
            for (int i = 0; i < poops.Count; i++)
            {
                MainEntity pickup = (MainEntity)poops[i];
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
                        //Do something
                        RocketLog.Log("Player hits a turd!", this);
                    }
                }
            }
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
