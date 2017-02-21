using Implementation.Components;
using RocketWorks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Implementation
{
    class Contexts
    {
        public EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent> mainContext;
    }
}
