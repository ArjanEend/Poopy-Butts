using UnityEngine;
using System.Collections;
using RocketWorks.Entities;

public class TestComponent : IComponent {
	private float offset = 0f;
	public Transform visuals;
	public float Offset {
		get { return offset; }
		set { offset = value; }
	}
}
