using System;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using Implementation.Components;
using RocketWorks;

namespace Implementation.Systems
{
    class PlayerMoveSystem : SystemBase
    {
        private Group inputGroup;
        private Group playerGroup;

        public override void Initialize(Contexts contexts)
        {
            inputGroup = contexts.Main.Pool.GetGroup(typeof(AxisComponent), typeof(PlayerIdComponent));
            playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(MovementComponent), typeof(TransformComponent));
        }

        public override void Execute()
        {
            var newInput = inputGroup.NewEntities;
            for(int i = 0; i < newInput.Count; i++)
            {
                for(int j = 0; j < playerGroup.Count; j++)
                {
                    if(newInput[i].GetComponent<PlayerIdComponent>().id == playerGroup[j].GetComponent<PlayerIdComponent>().id)
                    {
                        Vector2 input = newInput[i].GetComponent<AxisComponent>().input;
                        RocketLog.Log("Set speed to: " + input.ToString());
                        playerGroup[j].GetComponent<MovementComponent>().acceleration = input * .01f;
                    }
                }
                newInput[i].Reset();
            }
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
