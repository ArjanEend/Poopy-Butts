#pragma warning disable
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using PoopyButts.Components;
using RocketWorks.Entities;
using System.Collections.Generic;
////// AUTO GENERATED ////////
public partial class MainEntity : Entity
{
public AxisComponent AxisComponent()
{
return (AxisComponent)components[0];
}
public MovementComponent MovementComponent()
{
return (MovementComponent)components[1];
}
public PlayerIdComponent PlayerIdComponent()
{
return (PlayerIdComponent)components[2];
}
public TransformComponent TransformComponent()
{
return (TransformComponent)components[3];
}
public VisualizationComponent VisualizationComponent()
{
return (VisualizationComponent)components[4];
}
public LerpToComponent LerpToComponent()
{
return (LerpToComponent)components[5];
}
public CircleCollider CircleCollider()
{
return (CircleCollider)components[6];
}
public PickupComponent PickupComponent()
{
return (PickupComponent)components[7];
}
public Stomach Stomach()
{
return (Stomach)components[8];
}
}
#pragma warning restore
