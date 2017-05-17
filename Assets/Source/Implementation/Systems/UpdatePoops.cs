using Implementation.Components;
using RocketWorks.Systems;
using System;
using RocketWorks.Grouping;
using RocketWorks.Entities;
using RocketWorks;

namespace Implementation.Systems
{
    public class UpdatePoops : SystemBase
    {
        private Group poopGroup;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            poopGroup = contexts.Main.Pool.GetGroup(typeof(PoopComponent), typeof(MovementComponent), typeof(TransformComponent));
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < poopGroup.Count; i++)
            {
                Vector2 heading = 
                    poopGroup[i].GetComponent<PoopComponent>().playerRef.Entity.GetComponent<TransformComponent>().position -
                    poopGroup[i].GetComponent<TransformComponent>().position;

                if (heading.Magnitude() < .6f)
                    heading = Vector2.zero;
                
                for(int j = 0; j < poopGroup.Count; j++)
                {
                    Entity first = poopGroup[i];
                    Entity second = poopGroup[j];
                    

                     
                }

                heading = heading.Normalized();
                poopGroup[i].GetComponent<MovementComponent>().acceleration = heading * .3f;
            }
        }
    }
}
