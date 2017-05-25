using Implementation.Components;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using RocketWorks.Entities;
using RocketWorks.Networking;
using RocketWorks.Commands;

namespace Implementation.Systems
{
    public class PlayerInput : SystemBase
    {
        private Group buttonGroup;
        private Group playerGroup;
        private Group followGroup;
        private Group unitGroup;
        private SocketController socket;

        public PlayerInput(SocketController socket)
        {
            this.socket = socket;
        }

        public override void Initialize(Contexts contexts)
        {
            base.Initialize(contexts);

            buttonGroup = contexts.Input.Pool.GetGroup(typeof(ButtonComponent), typeof(PlayerIdComponent));
            playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(TriggerComponent));
            unitGroup = contexts.Main.Pool.GetGroup(typeof(OwnerComponent), typeof(MovementComponent));
            followGroup = contexts.Main.Pool.GetGroup(typeof(OwnerComponent), typeof(MovementComponent), typeof(FollowComponent));

            buttonGroup.OnEntityAdded += OnNewButton;
        }

        private void OnNewButton(Entity obj)
        {
            PlayerIdComponent id = obj.GetComponent<PlayerIdComponent>();
            ButtonComponent button = obj.GetComponent<ButtonComponent>();

            Entity player = null;
            for (int j = 0; j < playerGroup.Count; j++)
            {
                TransformComponent trans = playerGroup[j].GetComponent<TransformComponent>();
                MovementComponent move = playerGroup[j].GetComponent<MovementComponent>();
                if (id.id == playerGroup[j].GetComponent<PlayerIdComponent>().id)
                {
                    player = playerGroup[j];
                    break;
                }
            }

            if (button.id == 1)
            {
                TransformComponent axis = obj.GetComponent<TransformComponent>();
                SendUnit(player, axis);
            }
        }

        private void SendUnit(Entity player, TransformComponent axis)
        {
            for (int i = 0; i < followGroup.Count; i++)
            {
                Entity ent = followGroup[i];

                OwnerComponent owner = ent.GetComponent<OwnerComponent>();
                if (owner.playerReference == player && ent.HasComponent<FollowComponent>())
                {
                    ent.RemoveComponent<FollowComponent>();
                    var guard = ent.AddComponent<GuardComponent>();
                    guard.position = axis.position;
                    socket.WriteSocket(new MainContextUpdateComponentCommand(guard, ent.CreationIndex));
                    socket.WriteSocket(new MainContextRemoveComponentCommand(ent.GetIndex<FollowComponent>(), ent.CreationIndex));
                    return;
                }
            }
        }

        private void GatherUnits(Entity player)
        {
            TriggerComponent trigger = player.GetComponent<TriggerComponent>();
            var objects = trigger.GhostObject.OverlappingPairs;
            for(int i = 0; i < objects.Count; i++)
            {
                Entity ent = objects[i].UserObject as Entity;
                if(ent != null && unitGroup.Contains(ent))
                {
                    OwnerComponent owner = ent.GetComponent<OwnerComponent>();
                    if(owner.playerReference == player)
                    {
                        if(!ent.HasComponent<FollowComponent>())
                        {
                            var comp = ent.AddComponent<FollowComponent>();
                            socket.WriteSocket(new MainContextUpdateComponentCommand(comp, ent.CreationIndex));
                        }
                    }
                }
            }
        }

        public override void Destroy()
        {
        }

        public override void Execute(float deltaTime)
        {
            for (int j = 0; j < playerGroup.Count; j++)
            {
                PlayerIdComponent id = playerGroup[j].GetComponent<PlayerIdComponent>();
                for (int i = 0; i < buttonGroup.Count; i++)
                {
                    PlayerIdComponent buttonId = buttonGroup[i].GetComponent<PlayerIdComponent>();
                    ButtonComponent button = buttonGroup[i].GetComponent<ButtonComponent>();
                    if (buttonGroup[i].Composition == 0 || buttonId.id != id.id)
                        continue;
                    if (button.id == 2)
                    {
                        GatherUnits(playerGroup[j]);
                    }
                }
            }
        }
    }
}
