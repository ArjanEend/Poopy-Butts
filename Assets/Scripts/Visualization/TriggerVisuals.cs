using Implementation.Components;
using UnityEngine;

public class TriggerVisuals : ComponentVisualizerBase<TriggerComponent>
{
    public override void Init(TriggerComponent component)
    {
        transform.localScale = Vector3.one * component.radius;
    }
}
