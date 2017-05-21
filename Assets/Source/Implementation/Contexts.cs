﻿using Implementation.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
using System;
using PoopyButts.Components;

public partial class Contexts
{
    private EntityContext<MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, LerpToComponent, CircleCollider, PickupComponent, Stomach, PoopComponent, Tilemap> mainContext = 
        new EntityContext<MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, LerpToComponent, CircleCollider, PickupComponent, Stomach, PoopComponent, Tilemap>();

    private EntityContext<AxisComponent, PlayerIdComponent, ButtonComponent> inputContext =
        new EntityContext<AxisComponent, PlayerIdComponent, ButtonComponent>();

    private EntityContext<PlayerIdComponent, PingComponent, PongComponent, PlayerInfo> metaContext =
        new EntityContext<PlayerIdComponent, PingComponent, PongComponent, PlayerInfo>();

    private EntityContext<PlayerIdComponent, MessageComponent> messageContext = 
        new EntityContext<PlayerIdComponent, MessageComponent>();

    public EntityContext Main { get { return mainContext; } }
    public EntityContext Input { get { return inputContext; } }
    public EntityContext Message { get { return messageContext; } }
    public EntityContext Meta { get { return metaContext; } }
}
