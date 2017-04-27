using Implementation.Components;
using PoopyButts.Components;
using RocketWorks;
using RocketWorks.Grouping;
using RocketWorks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Source.Implementation.Systems
{
    public class CircleCollisionSystem : SystemBase
    {
        private Group collisionGroup;
        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            collisionGroup = contexts.Main.Pool.GetGroup(typeof(CircleCollider), typeof(TransformComponent));
        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < collisionGroup.Count; i++)
            {
                MainEntity ent = (MainEntity)collisionGroup[i];
                for(int j = 0; j < collisionGroup.Count; j++)
                {
                    if (i == j)
                        continue;
                    MainEntity other = (MainEntity)collisionGroup[j];

                    if (ent.TransformComponent().position == other.TransformComponent().position)
                        continue;

                    Vector2 diff = ent.TransformComponent().position - other.TransformComponent().position;
                    Vector2 mid = (ent.TransformComponent().position + other.TransformComponent().position) * .5f;
                    float dist = Vector2.Distance(ent.TransformComponent().position, other.TransformComponent().position);
                    float overlap = dist - (ent.CircleCollider().radius + other.CircleCollider().radius);
                    if (overlap < 0f)
                    {
                        if(ent.MovementComponent() != null && other.MovementComponent() != null)
                        {
                            ent.TransformComponent().position = mid + ent.CircleCollider().radius * diff / dist;
                            other.TransformComponent().position = mid + other.CircleCollider().radius * -diff / dist;
                        }
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
