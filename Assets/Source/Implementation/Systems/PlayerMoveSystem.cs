using System;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using Implementation.Components;
using RocketWorks;
using System.Collections.Generic;

namespace Implementation.Systems
{
    class PlayerMoveSystem : SystemBase
    {
        private Group inputGroup;
        private Group playerGroup;

        private Dictionary<TransformComponent, Vector2[]> oldStates;
        private Dictionary<MovementComponent, Vector2[]> velocities;
        private int currentIndex = 0;

        public override void Initialize(Contexts contexts)
        {
            inputGroup = contexts.Main.Pool.GetGroup(typeof(AxisComponent), typeof(PlayerIdComponent));
            playerGroup = contexts.Main.Pool.GetGroup(typeof(PlayerIdComponent), typeof(MovementComponent), typeof(TransformComponent));
            oldStates = new Dictionary<TransformComponent, Vector2[]>();
            velocities = new Dictionary<MovementComponent, Vector2[]>();
        }

        public override void Execute(float deltaTime)
        {
            var newInput = inputGroup.NewEntities;
            for (int j = 0; j < playerGroup.Count; j++)
            {
                TransformComponent trans = playerGroup[j].GetComponent<TransformComponent>();
                MovementComponent move = playerGroup[j].GetComponent<MovementComponent>();
                if (!oldStates.ContainsKey(trans))
                    oldStates.Add(trans, new Vector2[400]);
                if (!velocities.ContainsKey(move))
                    velocities.Add(move, new Vector2[400]);
                oldStates[trans][currentIndex] = trans.position;
                for (int i = 0; i < newInput.Count; i++)
                {
                    if (newInput[i].Composition == 0)
                        continue;
                    if(newInput[i].GetComponent<PlayerIdComponent>().id == playerGroup[j].GetComponent<PlayerIdComponent>().id)
                    {
                        Vector2 input = newInput[i].GetComponent<AxisComponent>().input;
                        float timeDiff = (float)(newInput[i].GetComponent<AxisComponent>().time.Subtract(DateTime.UtcNow).TotalMilliseconds) * -.001f;

                        int stepsBack = Mathf.RoundToInt(timeDiff / .016f);
                        int index = currentIndex - stepsBack;
                        while (index < 0)
                            index += 400;

                        //trans.position = oldStates[trans][index];
                        //move.velocity = velocities[move][index];

                        input.Normalize();

                        Vector2 prevAcc = move.acceleration;

                        Vector2 prevVel = move.velocity;
                        move.velocity = input * .5f;
                        //move.velocity += (move.acceleration - prevAcc) * timeDiff;
                        //move.velocity -= move.friction * move.velocity * timeDiff;
                        trans.position += (move.velocity - prevVel) * timeDiff;

                        //Processed
                        newInput[i].Reset();
                    }
                }
            }
            currentIndex++;
            currentIndex %= 400;
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
