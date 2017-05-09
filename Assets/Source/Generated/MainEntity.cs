#pragma warning disable
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using Implementation.Components;
using PoopyButts.Components;
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
public PlayerIdComponent PlayerIdComponent()
{
return (PlayerIdComponent)components[1];
}
public TransformComponent TransformComponent()
{
return (TransformComponent)components[2];
}
public VisualizationComponent VisualizationComponent()
{
return (VisualizationComponent)components[3];
}
public LerpToComponent LerpToComponent()
{
return (LerpToComponent)components[4];
}
public CircleCollider CircleCollider()
{
return (CircleCollider)components[5];
}
public PickupComponent PickupComponent()
{
return (PickupComponent)components[6];
}
public Stomach Stomach()
{
return (Stomach)components[7];
}
public PoopComponent PoopComponent()
{
return (PoopComponent)components[8];
}
public Tilemap Tilemap()
{
return (Tilemap)components[9];
}
}
#pragma warning restore
