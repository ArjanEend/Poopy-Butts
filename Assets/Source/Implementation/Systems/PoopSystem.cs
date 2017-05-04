using Implementation.Components;
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

                    var newEntity = contexts.Main.Pool.GetObject();
                    newEntity.AddComponent<TransformComponent>().position = playerGroup[j].GetComponent<TransformComponent>().position;
                    newEntity.AddComponent<VisualizationComponent>().resourceId = "Turd";

                    socket.WriteSocket(new MainContextCreateEntityCommand(newEntity));
                }
            }
        }
    }
}
