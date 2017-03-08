using System;
using RocketWorks.Pooling;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using Implementation.Components;
using System.Collections.Generic;
using RocketWorks.Entities;
using RocketWorks.Networking;
using RocketWorks.Commands;

namespace Implementation.Systems
{
    class SendWorldSystem : SystemBase
    {
        private int pId;

        private Group userGroup;
        private Group itemGroup;

        private SocketController controller;

        public SendWorldSystem(SocketController controller)
        {
            this.controller = controller;
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Initialize(Contexts contexts)
        {
            EntityPool pool = contexts.MainContext.Pool;
            pId = pool.GetIndexOf(typeof(PlayerIdComponent));
            userGroup = pool.GetGroup(typeof(PlayerIdComponent), typeof(PingComponent));
            itemGroup = pool.GetGroup(typeof(TransformComponent), typeof(MovementComponent), typeof(VisualizationComponent));
        }

        public override void Execute()
        {
            List<Entity> users = userGroup.NewEntities;
            for(int i = 0; i < users.Count; i++)
            {
                RocketLog.Log("User: " + i, this);
                for(int j = 0; j < itemGroup.Count; j++)
                {
                    for(int k = 0; k < users.Count; k++)
                    {
                        controller.WriteSocket(new CreateEntityCommand(users[i]), users[k].GetComponent<PlayerIdComponent>(pId).id);
                        controller.WriteSocket(new CreateEntityCommand(users[k]), users[i].GetComponent<PlayerIdComponent>(pId).id);
                    }
                    controller.WriteSocket(new CreateEntityCommand(itemGroup[j]), users[i].GetComponent<PlayerIdComponent>(pId).id);
                }
            }
        }
    }
}
