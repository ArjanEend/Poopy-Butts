using Implementation.Components;
using RocketWorks.Entities;
using System.Collections.Generic;

public partial class Contexts
{
    private EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, PingComponent> mainContext;

    public EntityContext MainContext { get { return mainContext; } }

    public Contexts()
    {
        contexts = new List<EntityContext>();
        mainContext = new EntityContext<AxisComponent, MessageComponent, MovementComponent, PlayerIdComponent, TransformComponent, VisualizationComponent, PingComponent>();
        contexts.Add(mainContext);
    }
}
