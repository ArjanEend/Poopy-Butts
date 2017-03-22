using RocketWorks;
using RocketWorks.Entities;
using System;

namespace Implementation.Components
{
    public partial class MovementComponent : IComponent
    {
        public Vector2 velocity;
        public float friction;
        public Vector2 acceleration;
    }
}
