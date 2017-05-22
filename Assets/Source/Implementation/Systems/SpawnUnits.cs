using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Commands;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Systems;
using System;

namespace Implementation.Systems
{
    public class SpawnUnits : SystemBase
    {
        private SocketController socket;
        
        private Group spawnerGroup;

        private float elapsedTime = 0f;

        public SpawnUnits(SocketController socket)
        {
            this.socket = socket;
        }

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            spawnerGroup = contexts.Main.Pool.GetGroup(typeof(SpawnerComponent), typeof(OwnerComponent), typeof(TransformComponent));
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
            elapsedTime += deltaTime;
            for (int j = 0; j < spawnerGroup.Count; j++)
            {
                SpawnerComponent spawner = spawnerGroup[j].GetComponent<SpawnerComponent>();
                if (elapsedTime - spawner.lastTime > spawner.interval)
                {
                    spawner.lastTime = elapsedTime;
                    socket.WriteSocket(new MainContextUpdateComponentCommand(spawner, spawnerGroup[j].CreationIndex));

                    TransformComponent trans = spawnerGroup[j].GetComponent<TransformComponent>();
                    OwnerComponent owner = spawnerGroup[j].GetComponent<OwnerComponent>();

                    var newEntity = contexts.Main.Pool.GetObject();
                    newEntity.AddComponent<TransformComponent>().position = spawnerGroup[j].GetComponent<TransformComponent>().position;
                    newEntity.AddComponent<VisualizationComponent>().resourceId = "Unit";
                    newEntity.AddComponent<OwnerComponent>().playerReference = spawnerGroup[j];
                    newEntity.AddComponent<MovementComponent>().friction = .5f;
                    newEntity.AddComponent<CircleCollider>().radius = .1f;
                    newEntity.AddComponent<LerpToComponent>();

                    socket.WriteSocket(new MainContextCreateEntityCommand(newEntity));
                }
            }
        }
    }
}
