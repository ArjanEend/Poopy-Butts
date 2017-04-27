using Implementation.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
using System;
using PoopyButts.Components;

public partial class Contexts
{
    private EntityContext<AxisComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, LerpToComponent, CircleCollider> mainContext = 
        new EntityContext<AxisComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, LerpToComponent, CircleCollider>();

    private EntityContext<PlayerIdComponent, PingComponent, PongComponent> metaContext =
        new EntityContext<PlayerIdComponent, PingComponent, PongComponent>();

    private EntityContext<PlayerIdComponent, MessageComponent> messageContext = 
        new EntityContext<PlayerIdComponent, MessageComponent>();

    public EntityContext Main { get { return mainContext; } }
    public EntityContext Message { get { return messageContext; } }
    public EntityContext Meta { get { return metaContext; } }
}
