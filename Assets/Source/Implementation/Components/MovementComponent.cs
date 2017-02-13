using RocketWorks;
using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    [Serializable]
    class MovementComponent : IComponent
    {
        public Vector2 velocity;
        public float friction;
        public float acceleration;
    }
}
