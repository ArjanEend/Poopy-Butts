using Implementation.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVisuals : ComponentUpdaterBase<SpawnerComponent>
{

    [SerializeField]
    private GameObject[] indicators;


    public override void Init(SpawnerComponent component)
    {

    }

    public override void OnUpdate(SpawnerComponent component)
    {
        for(int i = 0; i < indicators.Length; i++)
        {
            indicators[i].SetActive(i > component.unitsSpawned);
        }
    }
}
