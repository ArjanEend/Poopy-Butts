using Implementation.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIDVisualizer : ComponentUpdaterBase<PlayerIdComponent, OwnerComponent> {

    [SerializeField]
    private MeshRenderer[] renders;

    [SerializeField]
    private SkinnedMeshRenderer[] skinnedRenderers;

    [SerializeField]
    private Color[] colors;

    [SerializeField]
    private string property;

    public override void Init(PlayerIdComponent component)
    {
        RocketLog.Log("PlayerID Visualizer");
        System.Random rand = new System.Random(component.id);
        Color col = colors[rand.Next(0, colors.Length - 1)];

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor(property, col);
        for(int i = 0; i < renders.Length; i++)
        {
            renders[i].SetPropertyBlock(block);
        }

        for(int i = 0; i < skinnedRenderers.Length; i++)
        {
            skinnedRenderers[i].SetPropertyBlock(block);
        }
    }

    public override void Init(OwnerComponent component)
    {
        if (component.playerReference.Entity == null)
            return;
        var playerId = component.playerReference.Entity.GetComponent<PlayerIdComponent>();
        Init(playerId);
    }

    public override void OnUpdate(OwnerComponent component)
    {
        Init(component);
    }

    public override void OnUpdate(PlayerIdComponent component)
    {
        Init(component);
    }
}
