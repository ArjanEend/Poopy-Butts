using Implementation.Components;
using RocketWorks.Systems;
using System;
using RocketWorks.Grouping;
using RocketWorks.Entities;

namespace Implementation.Systems
{
    public class AttackTrigger : SystemBase
    {
        private Group unitGroup;
        private Group healthGroup;
        private object firstPoop;

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);

            unitGroup = contexts.Main.Pool.GetGroup(typeof(OwnerComponent), typeof(TransformComponent));
            healthGroup = contexts.Main.Pool.GetGroup(typeof(HealthComponent), typeof(TransformComponent));
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
            for (int i = 0; i < unitGroup.Count; i++)
            {
                AttackComponent attack = unitGroup[i].GetComponent<AttackComponent>();
                
                if(attack != null)
                {
                    if (!attack.target.Entity.Alive)
                        unitGroup[i].RemoveComponent<AttackComponent>();
                    continue;
                }

                TriggerComponent trigger = unitGroup[i].GetComponent<TriggerComponent>();
                var objects = trigger.GhostObject.OverlappingPairs;
                for (int x = 0; x < objects.Count; x++)
                {
                    Entity ent = objects[x].UserObject as Entity;
                    if (ent != null && healthGroup.Contains(ent))
                    {
                        Entity first = unitGroup[i];
                        Entity second = ent;
                        OwnerComponent firstPoop = first.GetComponent<OwnerComponent>();
                        OwnerComponent secondPoop = ent.GetComponent<OwnerComponent>();
                        PlayerIdComponent playerId = ent.GetComponent<PlayerIdComponent>();
                        TransformComponent firstTrans = ent.GetComponent<TransformComponent>();
                        TransformComponent secondTrans = ent.GetComponent<TransformComponent>();

                        if (playerId != null && firstPoop.playerReference == second)
                        {
                            //Behaviour for flocking leader
                            continue;
                        }

                        if (attack == null)
                        {
                            attack = new AttackComponent();
                            attack.damage = 1f;
                            attack.target = ent;
                            first.AddComponent(attack);
                        }
                    }
                }
            }
        }
    }
}
