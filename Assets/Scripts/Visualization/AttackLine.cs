using Implementation.Components;
using UnityEngine;
using System;

public class AttackLine : ComponentUpdaterBase<AttackComponent>
{

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform target;

    public override void Init(AttackComponent component)
    {
        target = component.target.Entity.GetComponent<VisualizationComponent>().go.transform;
    }

    public override void OnRemove(AttackComponent component)
    {
        target = null;
    }

    public override void OnUpdate(AttackComponent component)
    {
        if(component != null)
            target = component.target.Entity.GetComponent<VisualizationComponent>().go.transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	

	void Update ()
    {
        lineRenderer.enabled = target != null;
        if (target == null)
            return;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (transform.position + transform.forward + target.position) * .5f);
        lineRenderer.SetPosition(2, target.position);
	}
}
