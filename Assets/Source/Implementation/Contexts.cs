using Implementation.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
using System;

public partial class Contexts
{
    private EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, PingComponent, PongComponent> mainContext;

    public EntityContext MainContext { get { return mainContext; } }

    partial void Populate()
    {
        throw new NotImplementedException();
    }

}
