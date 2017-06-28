using System;
using RocketWorks.Systems;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Commands;
using PoopyButts.Components;
using BulletSharp;
#if UNITY_5
using BulletSharp.Math;
#endif

namespace Implementation.Systems
{
    public class AttackCollisions : SystemBase
    {
        private Group group;
        private SocketController socket;

        private float time = 0f;

        public AttackCollisions(SocketController socket)
        {
            this.socket = socket;
        }

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            group = contexts.Physics.Pool.GetGroup(typeof(CollisionComponent));
        }

        public override void Destroy()
        {

        }

        public override void Execute(float deltaTime)
        {
            time += deltaTime;
            for(int i = 0; i < group.Count; i++)
            {
                CollisionComponent collision = group[i].GetComponent<CollisionComponent>();

                AttackComponent attackA = collision.a.Entity.GetComponent<AttackComponent>();
                AttackComponent attackB = collision.b.Entity.GetComponent<AttackComponent>();
                HealthComponent healthA = collision.a.Entity.GetComponent<HealthComponent>();
                HealthComponent healhtB = collision.b.Entity.GetComponent<HealthComponent>();

                OwnerComponent ownerA = collision.a.Entity.GetComponent<OwnerComponent>();
                OwnerComponent ownerB = collision.b.Entity.GetComponent<OwnerComponent>();

                CircleCollider colliderA = collision.a.Entity.GetComponent<CircleCollider>();
                CircleCollider colliderB = collision.b.Entity.GetComponent<CircleCollider>();

                if (ownerA != null && ownerB != null && ownerA.playerReference != ownerB.playerReference)
                {
                    if (time - healhtB.LastDamageTime > .4f)
                    {
                        if (attackA != null && healhtB != null)
                        {

                            healhtB.LastDamageTime = time;
                            healhtB.health -= attackA.damage;
                            socket.WriteSocket(new MainContextUpdateComponentCommand(healhtB, collision.b.creationIndex));
                            colliderB.RigidBody.ApplyImpulse(colliderB.RigidBody.CenterOfMassPosition - colliderA.RigidBody.CenterOfMassPosition * 50000f, new Vector3());

                        }
                        if (attackB != null && healthA != null)
                        {
                            if (time - healthA.LastDamageTime > .4f)
                            {
                                healthA.LastDamageTime = time;
                                healthA.health -= attackB.damage;
                                socket.WriteSocket(new MainContextUpdateComponentCommand(healthA, collision.a.creationIndex));
                                colliderA.RigidBody.ApplyImpulse(colliderA.RigidBody.CenterOfMassPosition - colliderB.RigidBody.CenterOfMassPosition * 50000f, new Vector3());
                            }
                        }
                    }
                }
            }

            group.DestroyAll();
        }
    }
}
