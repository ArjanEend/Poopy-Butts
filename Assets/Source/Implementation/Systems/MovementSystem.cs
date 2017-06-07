using System;
using RocketWorks.Pooling;
using RocketWorks.Systems;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks;

namespace Implementation.Systems
{
    class MovementSystem : SystemBase
    {
        private int mId;
        private int tId;

        private Group group;

        public override void Initialize(Contexts contexts)
        {
            EntityPool pool = contexts.Main.Pool;
            mId = pool.GetIndexOf(typeof(MovementComponent));
            tId = pool.GetIndexOf(typeof(TransformComponent));
            group = pool.GetGroup(typeof(TransformComponent), typeof(MovementComponent));
        }


        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < group.Count; i++)
            {
                group[i].IsDirty = true;
                var t = group[i].GetComponent<TransformComponent>(tId);
                var m = group[i].GetComponent<MovementComponent>(mId);
                m.velocity += m.acceleration * .01f * deltaTime;
                t.position += (Vector3)m.velocity * deltaTime;
                m.velocity -= m.velocity * m.friction * deltaTime;
            }
        }

        
    }
}
