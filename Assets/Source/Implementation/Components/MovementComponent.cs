﻿using RocketWorks;
using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class MovementComponent : IComponent, IEstimatable<MovementComponent>
    {
        public Vector3 velocity;
        public float friction;
        public Vector3 acceleration;

        public void Estimate(MovementComponent against, float deltaTime, bool local)
        {
            Vector3 delta = against.velocity - velocity;
            if (delta.Magnitude() > .8f || !local)
            {
                velocity = against.velocity;//Vector2.Lerp(velocity, against.velocity, deltaTime * 10f);
                //velocity += delta * deltaTime;
            }
        }

        public void Estimate(object against, float deltaTime, bool local)
        {
            Estimate((MovementComponent)against, deltaTime, local);
        }
    }
}
