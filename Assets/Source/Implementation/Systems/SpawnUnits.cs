using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Commands;
using RocketWorks.Entities;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Systems;
using System;
using System.Collections.Generic;

namespace Implementation.Systems
{
    public class SpawnUnits : SystemBase
    {
        private const int UNIT_LIMIT = 3;

        private SocketController socket;
        
        private Group spawnerGroup;

        private Random random = new Random(DateTime.Now.Millisecond);
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
                OwnerComponent owner = spawnerGroup[j].GetComponent<OwnerComponent>();
                if (owner.playerReference.Entity == null)
                    continue;
                if (elapsedTime - spawner.lastTime > spawner.interval)
                {
                    spawner.lastTime = elapsedTime;
                    if (spawner.unitsSpawned >= UNIT_LIMIT)
                        continue;

                    spawner.unitsSpawned++;
                    socket.WriteSocket(new MainContextUpdateComponentCommand(spawner, spawnerGroup[j].CreationIndex));

                    TransformComponent trans = spawnerGroup[j].GetComponent<TransformComponent>();

                    var newEntity = contexts.Main.Pool.GetObject();
                    newEntity.DestroyEvent += OnUnitDestroy;
                    RocketWorks.Vector3 offset = new RocketWorks.Vector3(random.Next(-100, 100) * .001f, .1f, random.Next(-100, 100) * .001f);
                    newEntity.AddComponent<TransformComponent>().position = trans.position + offset.Normalized() * .3f;
                    newEntity.AddComponent<VisualizationComponent>().resourceId = "Unit";
                    newEntity.AddComponent<OwnerComponent>().playerReference = owner.playerReference;
                    newEntity.AddComponent<MovementComponent>().friction = .5f;
                    newEntity.AddComponent<CircleCollider>().radius = .05f;
                    newEntity.AddComponent<HealthComponent>().health = 5f;
                    newEntity.AddComponent<TriggerComponent>().radius = 1.3f;
                    newEntity.AddComponent<LerpToComponent>();

                    RocketLog.Log("Spawning unit with ID: " + newEntity.CreationIndex, newEntity);
                    socket.WriteSocket(new MainContextCreateEntityCommand(newEntity));
                }
            }
        }

        private void OnUnitDestroy(Entity ent)
        {
            OwnerComponent owner = ent.GetComponent<OwnerComponent>();
            for (int j = 0; j < spawnerGroup.Count; j++)
            {
                if (spawnerGroup[j].GetComponent<OwnerComponent>().playerReference == ent.GetComponent<OwnerComponent>().playerReference)
                    spawnerGroup[j].GetComponent<SpawnerComponent>().unitsSpawned--;
            }
        }
    }
}
