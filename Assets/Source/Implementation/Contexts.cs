using Implementation.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
using System;
using PoopyButts.Components;

public partial class Contexts
{
    private EntityContext<MovementComponent, FollowComponent, GuardComponent, HealthComponent, AttackComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, LerpToComponent, TriggerComponent, CircleCollider, OwnerComponent, SpawnerComponent, Tilemap> mainContext = 
        new EntityContext<MovementComponent, FollowComponent, GuardComponent, HealthComponent, AttackComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, LerpToComponent, TriggerComponent, CircleCollider, OwnerComponent, SpawnerComponent, Tilemap>();

    private EntityContext<AxisComponent, PlayerIdComponent, ButtonComponent, TransformComponent> inputContext =
        new EntityContext<AxisComponent, PlayerIdComponent, ButtonComponent, TransformComponent>();

    private EntityContext<CollisionComponent> physicsContext =
        new EntityContext<CollisionComponent>();

    private EntityContext<PlayerIdComponent, PingComponent, PongComponent, PlayerInfo> metaContext =
        new EntityContext<PlayerIdComponent, PingComponent, PongComponent, PlayerInfo>();

    private EntityContext<PlayerIdComponent, MessageComponent> messageContext = 
        new EntityContext<PlayerIdComponent, MessageComponent>();

    public EntityContext Main { get { return mainContext; } }
    public EntityContext Input { get { return inputContext; } }
    public EntityContext Physics { get { return physicsContext; } }
    public EntityContext Message { get { return messageContext; } }
    public EntityContext Meta { get { return metaContext; } }
}
