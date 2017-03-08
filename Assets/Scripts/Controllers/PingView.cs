using System;
using UnityEngine;
using UnityEngine.UI;

public class PingView : MonoBehaviour
{
    [SerializeField]
    private Text pingDisplay;

    public void Initialize(PingSystem system)
    {
        system.OnPingUpdated += OnPingUpdate;
    }

    private void OnPingUpdate(long ping)
    {
        pingDisplay.text = ping.ToString();
    }
}
