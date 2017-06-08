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
        private Group followGroup;
        private Random random = new Random();

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            unitGroup = contexts.Main.Pool.GetGroup(typeof(OwnerComponent), typeof(MovementComponent), typeof(TransformComponent));

            followGroup = contexts.Main.Pool.GetGroup(typeof(FollowComponent));
            followGroup.OnEntityAdded += OnNewFOllow;
        }

        private void OnNewFOllow(Entity obj)
        {
            RocketLog.Log("NEW FOLLOW");
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < unitGroup.Count; i++)
            {
                OwnerComponent firstPoop = unitGroup[i].GetComponent<OwnerComponent>();

                Vector3 heading = Vector3.zero;


                TransformComponent firstTrans = unitGroup[i].GetComponent<TransformComponent>();
                TriggerComponent trigger = unitGroup[i].GetComponent<TriggerComponent>();
                if (trigger.GhostObject != null)
                {
                    var objects = trigger.GhostObject.OverlappingPairs;
                    for (int x = 0; x < objects.Count; x++)
                    {
                        if (objects[x] is BulletSharp.GhostObject)
                            continue;
                        Entity ent = objects[x].UserObject as Entity;
                        if (ent != null)
                        {
                            Entity first = unitGroup[i];
                            Entity second = ent;
                            OwnerComponent secondPoop = ent.GetComponent<OwnerComponent>();
                            PlayerIdComponent playerId = ent.GetComponent<PlayerIdComponent>();
                            TransformComponent secondTrans = ent.GetComponent<TransformComponent>();

                            if (playerId != null && firstPoop.playerReference == second)
                            {
                                //Behaviour for flocking leader
                                float d = Vector3.Distance(firstTrans.position, secondTrans.position);
                                if (d > 0 && d <= .5)
                                {
                                    Vector3 repulse = firstTrans.position - secondTrans.position;
                                    repulse.Normalize();
                                    repulse /= d;
                                    heading += repulse * 500f;
                                }
                                continue;
                            }
                            if (secondPoop != null && firstPoop.playerReference.Entity == secondPoop.playerReference.Entity)
                            {
                                //Behaviour for flocking
                                float d = Vector3.Distance(firstTrans.position, secondTrans.position);
                                if (d > 0 && d <= .5)
                                {
                                    Vector3 repulse = firstTrans.position - secondTrans.position;
                                    repulse.Normalize();
                                    repulse /= d;
                                    heading += repulse * 2f;
                                }
                            }
                        }
                    }
                }

                if ((firstPoop.playerReference.Entity == null || !firstPoop.playerReference.Entity.Alive) && !unitGroup[i].HasComponent<GuardComponent>())
                {
                    unitGroup[i].RemoveComponent<FollowComponent>();
                    unitGroup[i].AddComponent<GuardComponent>().position = firstTrans.position;
                }

                if (unitGroup[i].HasComponent<FollowComponent>())
                    heading += firstPoop.playerReference.Entity.GetComponent<TransformComponent>().position -
                unitGroup[i].GetComponent<TransformComponent>().position;

                if (unitGroup[i].HasComponent<GuardComponent>())
                    heading += unitGroup[i].GetComponent<GuardComponent>().position -
                unitGroup[i].GetComponent<TransformComponent>().position;

                if(unitGroup[i].HasComponent<AttackComponent>())
                        heading += (unitGroup[i].GetComponent<AttackComponent>().target.Entity.GetComponent<TransformComponent>().position -
                            unitGroup[i].GetComponent<TransformComponent>().position) * 5f;

                heading += new Vector3(random.Next(-50, 50) * .002f, random.Next(-50, 50) * .002f, random.Next(-50, 50) * .002f);

                if (heading.Magnitude() > 1f)
                heading = heading.Normalized();

                unitGroup[i].GetComponent<MovementComponent>().acceleration = heading * 1100f;
            }
        }
    }
}
