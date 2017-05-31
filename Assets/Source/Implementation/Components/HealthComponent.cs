using RocketWorks.Entities;
using System;
using System.Collections.Generic;

namespace Implementation.Components
{
    public partial class HealthComponent : IComponent
    {
        public float health = 100f;
        private float lastDamageTime = 0f;
        public float LastDamageTime
        {
            get { return lastDamageTime; }
            set { lastDamageTime = value; }
        }
    }
}
