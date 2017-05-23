using System;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using Implementation.Components;
using RocketWorks;
using System.Collections.Generic;
using PoopyButts.Components;

namespace Implementation.Systems
{
    class PlayerMoveSystem : SystemBase
    {
        private Group inputGroup;
        private Group playerGroup;

        public override void Initialize(Contexts contexts)
        {
            inputGroup = contexts.Input.Pool.GetGroup(typeof(AxisComponent), typeof(PlayerIdComponent));
            playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(MovementComponent), typeof(CircleCollider), typeof(TransformComponent));
        }

        public override void Execute(float deltaTime)
        {
            var newInput = inputGroup.NewEntities;
            for (int j = 0; j < playerGroup.Count; j++)
            {
                TransformComponent trans = playerGroup[j].GetComponent<TransformComponent>();
                MovementComponent move = playerGroup[j].GetComponent<MovementComponent>();
                CircleCollider col = playerGroup[j].GetComponent<CircleCollider>();

                for (int i = 0; i < newInput.Count; i++)
                {
                    if (newInput[i].Composition == 0)
                        continue;
                    if(newInput[i].GetComponent<PlayerIdComponent>().id == playerGroup[j].GetComponent<PlayerIdComponent>().id)
                    {
                        lock (playerGroup[j])
                        {
                            Vector2 input = newInput[i].GetComponent<AxisComponent>().input;
                            float timeDiff = (float)(newInput[i].GetComponent<AxisComponent>().time.Subtract(DateTime.UtcNow).TotalMilliseconds) * -.001f;

                            input.Normalize();

                            Vector3 prevVel = move.acceleration;
                            float speed = 890f;
                            move.acceleration = new Vector3(input.x, 0f, input.y) * speed;

                            Vector3 movement = (move.acceleration - prevVel) * timeDiff; ;
                            

                            //Processed
                            newInput[i].Reset();
                        }
                    }
                }
            }
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
