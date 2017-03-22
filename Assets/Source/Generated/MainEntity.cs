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
public MessageComponent MessageComponent()
{
return (MessageComponent)components[1];
}
public MovementComponent MovementComponent()
{
return (MovementComponent)components[2];
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
public PingComponent PingComponent()
{
return (PingComponent)components[6];
}
public PongComponent PongComponent()
{
return (PongComponent)components[7];
}
}
