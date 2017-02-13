using System;
using RocketWorks.Pooling;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using Implementation.Components;
using System.Collections.Generic;
using RocketWorks.Entities;
using RocketWorks.Networking;

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

        public override void Initialize(EntityPool pool)
        {
            pId = pool.GetIndexOf(typeof(PlayerIdComponent));
            userGroup = pool.GetGroup(typeof(PlayerIdComponent));
            itemGroup = pool.GetGroup(typeof(TransformComponent), typeof(MovementComponent), typeof(VisualizationComponent));
        }

        public override void Execute()
        {
            List<Entity> users = userGroup.NewEntities;
            for(int i = 0; i < users.Count; i++)
            {
                for(int j = 0; j < itemGroup.Count j++)
                {

                }
            }
        }
    }
}
