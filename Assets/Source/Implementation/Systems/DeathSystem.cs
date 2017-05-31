using System;
using RocketWorks.Systems;
using Implementation.Components;
using RocketWorks.Grouping;

namespace Implementation.Systems
{
    public class DeathSystem : SystemBase
    {
        private Group healthGroup;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            healthGroup = contexts.Main.Pool.GetGroup(typeof(HealthComponent));
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
            for(int i = healthGroup.Count - 1; i >= 0; i--)
            {
                if(healthGroup[i].GetComponent<HealthComponent>().health <= 0f)
                {
                    healthGroup[i].Reset();
                }
            }
        }
    }
}
