﻿using System;
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
        private Group pingGroup;
        private Group messageGroup;

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
            EntityPool pool = contexts.Main.Pool;
            pId = pool.GetIndexOf(typeof(PlayerIdComponent));
            userGroup = pool.GetGroup(typeof(PlayerIdComponent));
            pingGroup = pool.GetGroup(typeof(PongComponent));
            messageGroup = pool.GetGroup(typeof(MessageComponent));
            itemGroup = pool.GetGroup(typeof(TransformComponent), typeof(MovementComponent), typeof(VisualizationComponent));
        }

        public override void Execute()
        {
            List<Entity> users = userGroup.NewEntities;
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].GetComponent<PlayerIdComponent>().id == -1)
                    continue;
                for (int k = 0; k < userGroup.Count; k++)
                {
                    controller.WriteSocket(new MainContextCreateEntityCommand(users[i]), userGroup[k].GetComponent<PlayerIdComponent>(pId).id);
                    controller.WriteSocket(new MainContextCreateEntityCommand(userGroup[k]), users[i].GetComponent<PlayerIdComponent>(pId).id);
                }
                RocketLog.Log("User: " + i, this);
                for(int j = 0; j < itemGroup.Count; j++)
                {
                    RocketLog.Log("Send generic object" + itemGroup[j].CreationIndex, this);
                    controller.WriteSocket(new MainContextCreateEntityCommand(itemGroup[j]), users[i].GetComponent<PlayerIdComponent>(pId).id);
                }
                for (int j = 0; j < messageGroup.Count; j++)
                {
                    RocketLog.Log("Send message object" + messageGroup[j].CreationIndex, this);
                    controller.WriteSocket(new MainContextCreateEntityCommand(messageGroup[j]), users[i].GetComponent<PlayerIdComponent>(pId).id);
                }
                for (int j = 0; j < pingGroup.Count; j++)
                {
                    RocketLog.Log("Send pong object", this);
                    controller.WriteSocket(new MainContextCreateEntityCommand(pingGroup[j]), users[i].GetComponent<PlayerIdComponent>(pId).id);
                }
            }
        }
    }
}
