using Implementation.Components;
using RocketWorks.Systems;
using System;
using RocketWorks.Grouping;
using RocketWorks.Entities;
using RocketWorks;

namespace Implementation.Systems
{
    public class UpdateUnits : SystemBase
    {
        private Group unitGroup;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            unitGroup = contexts.Main.Pool.GetGroup(typeof(OwnerComponent), typeof(MovementComponent), typeof(TransformComponent));
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < unitGroup.Count; i++)
            {
                OwnerComponent firstPoop = unitGroup[i].GetComponent<OwnerComponent>();
                if (firstPoop.playerReference.Entity == null)
                    continue;

                Vector2 heading = 
                    firstPoop.playerReference.Entity.GetComponent<TransformComponent>().position -
                    unitGroup[i].GetComponent<TransformComponent>().position;

                if (heading.Magnitude() < .6f)
                    heading = Vector2.zero;
                
                for(int j = 0; j < unitGroup.Count; j++)
                {
                    if (i == j)
                        continue;
                    Entity first = unitGroup[i];
                    Entity second = unitGroup[j];
                    OwnerComponent secondPoop = unitGroup[j].GetComponent<OwnerComponent>();
                    TransformComponent firstTrans = unitGroup[i].GetComponent<TransformComponent>();
                    TransformComponent secondTrans = unitGroup[j].GetComponent<TransformComponent>();

                    if(firstPoop.playerReference.Entity == secondPoop.playerReference.Entity)
                    {
                        //Behaviour if stuff is the same
                    } else
                    {
                        //Behaviour if enemy
                        float dist = Vector3.Distance(secondTrans.position, firstTrans.position);
                        if(dist < 1f)
                        {
                            heading = (Vector2)(secondTrans.position - firstTrans.position);
                            break;
                        }
                    }
                }

                if(heading.Magnitude() > 1f)
                    heading = heading.Normalized();

                unitGroup[i].GetComponent<MovementComponent>().acceleration = heading * 15f;
            }
        }
    }
}
