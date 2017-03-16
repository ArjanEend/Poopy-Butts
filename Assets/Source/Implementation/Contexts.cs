using Implementation.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
using System;

public partial class Contexts
{
    private EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, PingComponent, PongComponent> mainContext;

    public EntityContext Main { get { return mainContext == null ? 
                new EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, PingComponent, PongComponent>() :
                mainContext; } }

}
