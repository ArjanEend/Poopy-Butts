using Implementation.Components;

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
}
