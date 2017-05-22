using RocketWorks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Components
{
    public partial class SpawnerComponent : IComponent
    {
        public float interval = 5f;
        public float lastTime = 0f;
    }
}
