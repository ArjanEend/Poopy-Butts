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
        private Group userGroup;
        private Group itemGroup;
        private Group playerGroup;
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
            EntityPool mainPool = contexts.Main.Pool;
            userGroup = contexts.Meta.Pool.GetGroup(typeof(PlayerIdComponent)).SetMatching(true);
            playerGroup = mainPool.GetGroup(typeof(PlayerIdComponent), typeof(TransformComponent));
            pingGroup = contexts.Meta.Pool.GetGroup(typeof(PongComponent));
            messageGroup = contexts.Message.Pool.GetGroup(typeof(MessageComponent));
            itemGroup = mainPool.GetGroup(typeof(TransformComponent), typeof(VisualizationComponent));
        }

        public override void Execute(float deltaTime)
        {
            List<Entity> users = userGroup.NewEntities;
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].GetComponent<PlayerIdComponent>().id == -1)
                    continue;
                for (int k = 0; k < userGroup.Count; k++)
                {
                    controller.WriteSocket(new MetaContextCreateEntityCommand(users[i]), userGroup[k].GetComponent<PlayerIdComponent>().id);
                    controller.WriteSocket(new MetaContextCreateEntityCommand(userGroup[k]), users[i].GetComponent<PlayerIdComponent>().id);
                    for (int j = 0; j < playerGroup.Count; j++)
                    {
                        controller.WriteSocket(new MainContextCreateEntityCommand(playerGroup[j]), userGroup[k].GetComponent<PlayerIdComponent>().id);
                    }
                }
                
                RocketLog.Log("User: " + users[i].GetComponent<PlayerIdComponent>().id, this);
                for(int j = 0; j < itemGroup.Count; j++)
                {
                    RocketLog.Log("Send generic object" + itemGroup[j].CreationIndex, this);
                    controller.WriteSocket(new MainContextCreateEntityCommand(itemGroup[j]), users[i].GetComponent<PlayerIdComponent>().id);
                }
                for (int j = 0; j < messageGroup.Count; j++)
                {
                    RocketLog.Log("Send message object" + messageGroup[j].CreationIndex, this);
                    controller.WriteSocket(new MessageContextCreateEntityCommand(messageGroup[j]), users[i].GetComponent<PlayerIdComponent>().id);
                }
                for (int j = 0; j < pingGroup.Count; j++)
                {
                    RocketLog.Log("Send pong object", this);
                    controller.WriteSocket(new MetaContextCreateEntityCommand(pingGroup[j]), users[i].GetComponent<PlayerIdComponent>().id);
                }
            }
        }
    }
}
