using System;
using RocketWorks.Systems;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Commands;

namespace Implementation.Systems
{
    public class AttackCollisions : SystemBase
    {
        private Group group;
        private SocketController socket;

        public AttackCollisions(SocketController socket)
        {
            this.socket = socket;
        }

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            group = contexts.Physics.Pool.GetGroup(typeof(CollisionComponent));
            //attackGroup = contexts.Main.Pool.GetGroup(typeof(AttackComponent));
            //healthGroup = contexts.Main.Pool.GetGroup(typeof(HealthComponent));
        }

        public override void Destroy()
        {

        }

        public override void Execute(float deltaTime)
        {
            for(int i = 0; i < group.Count; i++)
            {
                CollisionComponent collision = group[i].GetComponent<CollisionComponent>();

                AttackComponent attackA = collision.a.Entity.GetComponent<AttackComponent>();
                AttackComponent attackB = collision.a.Entity.GetComponent<AttackComponent>();
                HealthComponent healthA = collision.a.Entity.GetComponent<HealthComponent>();
                HealthComponent healhtB = collision.a.Entity.GetComponent<HealthComponent>();
                if(attackA != null && healhtB != null)
                {
                    healhtB.health -= attackA.damage;
                    socket.WriteSocket(new MainContextUpdateComponentCommand(healhtB, collision.b.creationIndex));
                }
                if(attackB != null && healthA != null)
                {
                    healthA.health -= attackB.damage;
                    socket.WriteSocket(new MainContextUpdateComponentCommand(healthA, collision.a.creationIndex));
                }
            }

            group.DestroyAll();
        }
    }
}
