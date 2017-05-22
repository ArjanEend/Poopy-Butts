#pragma warning disable
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using PoopyButts.Components;
using Implementation.Components;
using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
////// AUTO GENERATED ////////
public partial class MainEntity : Entity
{
public MovementComponent MovementComponent()
{
return (MovementComponent)components[0];
}
public HealthComponent HealthComponent()
{
return (HealthComponent)components[1];
}
public AttackComponent AttackComponent()
{
return (AttackComponent)components[2];
}
public PlayerIdComponent PlayerIdComponent()
{
return (PlayerIdComponent)components[3];
}
public TransformComponent TransformComponent()
{
return (TransformComponent)components[4];
}
public VisualizationComponent VisualizationComponent()
{
return (VisualizationComponent)components[5];
}
public LerpToComponent LerpToComponent()
{
return (LerpToComponent)components[6];
}
public CircleCollider CircleCollider()
{
return (CircleCollider)components[7];
}
public OwnerComponent OwnerComponent()
{
return (OwnerComponent)components[8];
}
public SpawnerComponent SpawnerComponent()
{
return (SpawnerComponent)components[9];
}
public Tilemap Tilemap()
{
return (Tilemap)components[10];
}
}
#pragma warning restore
