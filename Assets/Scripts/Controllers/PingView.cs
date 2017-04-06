using System;
using UnityEngine;
using UnityEngine.UI;

public class PingView : MonoBehaviour
{
    [SerializeField]
    private Text pingDisplay;

    private long ping;

    public void Initialize(PingSystem system)
    {
        system.OnPingUpdated += OnPingUpdate;
    }

    private void OnPingUpdate(long ping)
    {
        this.ping = ping;
    }

    private void Update()
    {
        pingDisplay.text = ping.ToString();
    }
}
