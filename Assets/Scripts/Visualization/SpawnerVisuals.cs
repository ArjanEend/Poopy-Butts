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
        for (int i = 0; i < indicators.Length; i++)
        {
            RocketLog.Log("active: " + (i > component.unitsSpawned));
            indicators[i].SetActive(component.unitsSpawned > i);
        }
    }

    public override void OnRemove(SpawnerComponent component)
    {

    }

    public override void OnUpdate(SpawnerComponent component)
    {
        Init(component);   
    }
}
