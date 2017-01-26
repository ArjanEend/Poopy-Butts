using UnityEngine;
using System.Collections;
using RocketWorks.Base;
using RocketWorks.Entities;

public class PoopyGame : GameBase {

    [RuntimeInitializeOnLoadMethod]
	private static void Main () {
        new PoopyGame();
	}

    public PoopyGame() : base()
	{	
		TestSystem system = new TestSystem ();
		systemManager.AddSystem (system);
        systemManager.AddSystem(new MoveInputSystem(0));
        systemManager.AddSystem(new MoveUpdateSystem());
        
		for (int i = 0; i < 100; i++) 
		{
			Entity ent = entityPool.GetObject ();
			TestComponent comp = ent.AddComponent<TestComponent> (new TestComponent());
			comp.Offset = i * 1.5f;
			GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
			go.transform.position = new Vector3 (i, 0f, 0f);
			comp.visuals = go.transform;
            if(i % 23 == 0)
            {
                var playerComp = ent.AddComponent<PlayerIdComponent>(new PlayerIdComponent());
                playerComp.id = 0;
            }
		}

    }

}
