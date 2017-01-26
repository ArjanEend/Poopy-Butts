using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RocketWorks.Systems;
using RocketWorks.Pooling;
using RocketWorks.Entities;
using RocketWorks.Grouping;

public class TestSystem : SystemBase {

    private Group movingGroup;
    private int compIndex;

	override public void Initialize(EntityPool pool)
	{
		movingGroup = AddGroup(pool.GetGroup(typeof(TestComponent)));
        compIndex = pool.GetIndexOf(typeof(TestComponent));
	}

	override public void Execute()
	{
		float time = Time.time;
		for (int i = 0; i < movingGroup.Count; i++) {
            TestComponent comp = movingGroup[i].GetComponent<TestComponent>(compIndex);
            float offsetTime = time + comp.Offset;
            for (int j = 0; j < movingGroup.Count; j++)
            {
                TestComponent comp2 = movingGroup[j].GetComponent<TestComponent>(compIndex);
                offsetTime += comp2.visuals.transform.position.x;
            }
            Vector3 pos = comp.visuals.position;
            pos.y = Mathf.Sin(offsetTime) * 5f;
            comp.visuals.position = pos;
        }
	}

	override public void Destroy()
	{

	}
}
