using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Commands;
using RocketWorks.Grouping;
using RocketWorks.Networking;
using RocketWorks.Systems;
using System;

namespace Implementation.Systems
{
    public class PoopSystem : SystemBase
    {
        private SocketController socket;

        private Group inputGroup;
        private Group playerGroup;

        public PoopSystem(SocketController socket)
        {
            this.socket = socket;
        }

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);
            inputGroup = contexts.Input.Pool.GetGroup(typeof(ButtonComponent), typeof(PlayerIdComponent));
            playerGroup = contexts.Main.Pool.GetGroup(typeof(Stomach), typeof(PlayerIdComponent), typeof(TransformComponent));
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Execute(float deltaTime)
        {
            var input = inputGroup.NewEntities;
            for (int i = 0; i < input.Count; i++)
            {
                for(int j = 0; j < playerGroup.Count; j++)
                {
                    if (playerGroup[j].GetComponent<PlayerIdComponent>().id != input[i].GetComponent<PlayerIdComponent>().id)
                        continue;

                    Stomach stomach = playerGroup[i].GetComponent<Stomach>();
                    if (stomach.pickups.Count == 0)
                        continue;

                    float turdSize = .1f;
                    for(int p = 0; p < stomach.pickups.Count; p++)
                    {
                        turdSize += stomach.pickups[i].Entity.GetComponent<PickupComponent>().radius * .8f;
                        socket.WriteSocket(new MainContextDestroyEntityCommand(stomach.pickups[i]));
                        stomach.pickups[i].Entity.Reset();
                        stomach.pickups.Clear();
                        socket.WriteSocket(new MainContextUpdateComponentCommand(stomach, playerGroup[i].CreationIndex));
                    }

                    var newEntity = contexts.Main.Pool.GetObject();
                    newEntity.AddComponent<TransformComponent>().position = playerGroup[j].GetComponent<TransformComponent>().position;
                    newEntity.AddComponent<VisualizationComponent>().resourceId = "Turd";
                    newEntity.AddComponent<PoopComponent>().size = turdSize;
                    newEntity.GetComponent<PoopComponent>().playerRef = playerGroup[j];
                    newEntity.AddComponent<MovementComponent>().friction = .5f;
                    //newEntity.AddComponent<LerpToComponent>();
                    newEntity.AddComponent<CircleCollider>().radius = turdSize * .3f;

                    socket.WriteSocket(new MainContextCreateEntityCommand(newEntity));
                }
            }
        }
    }
}
