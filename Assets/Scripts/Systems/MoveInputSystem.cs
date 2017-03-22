using UnityEngine;
using System.Collections;
using RocketWorks.Systems;
using RocketWorks.Pooling;
using System;
using RocketWorks.Entities;
using RocketWorks.Grouping;

using Vector2 = RocketWorks.Vector2;

public class MoveInputSystem : SystemBase
{
    private int playerId;

    private EntityPool pool;
    private Group group;

    private Vector2 prevInput;

    public MoveInputSystem(int playerId) : base()
    {
        tickRate = .1f;
        this.playerId = playerId;
    }

    public override void Initialize(Contexts contexts)
    {
        this.pool = contexts.Main.Pool;
        this.group = pool.GetGroup(typeof(AxisComponent));

    }

    public override void Execute()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input == prevInput)
            return;

        prevInput = input;

        Entity entity = pool.GetObject();
        PlayerIdComponent component = entity.AddComponent(new PlayerIdComponent());
        component.id = playerId;
        AxisComponent aComponent = entity.AddComponent(new AxisComponent());
        aComponent.input = input;
    }

    public override void Cleanup()
    {
        /*for (int i = group.Count - 1; i >= 0; i--)
        {
            group[i].Reset();
        }*/
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }
}
