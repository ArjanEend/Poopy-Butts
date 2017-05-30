using Implementation.Components;
using RocketWorks;
using RocketWorks.Entities;
using RocketWorks.Grouping;
using RocketWorks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Systems
{
    public class LerpSystem : SystemBase
    {
        private Group group;

        public bool writeData = false;

        public LerpSystem(bool writeData)
        {
            this.writeData = writeData;
        }

        public override void Initialize(Contexts contexts)
        {
            group = contexts.Main.Pool.GetGroup(typeof(LerpToComponent), typeof(TransformComponent), typeof(MovementComponent));
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
            if (writeData)
            {
                for (int i = 0; i < group.Count; i++)
                {
                    Entity ent = group[i];
                    ent.GetComponent<LerpToComponent>().position = ent.GetComponent<TransformComponent>().position;
                    ent.GetComponent<LerpToComponent>().velocity = ent.GetComponent<MovementComponent>().velocity;
                }
            }
            else
            {
                for (int i = 0; i < group.Count; i++)
                {
                    Entity ent = group[i];
                    ent.GetComponent<TransformComponent>().position = Vector3.Lerp(ent.GetComponent<TransformComponent>().position, ent.GetComponent<LerpToComponent>().position, deltaTime * 1f);
                    ent.GetComponent<MovementComponent>().velocity = Vector3.Lerp(ent.GetComponent<MovementComponent>().velocity, ent.GetComponent<LerpToComponent>().velocity, deltaTime * 1f);
                }
            }
        }

        
    }
}
