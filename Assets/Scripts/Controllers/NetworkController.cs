using RocketWorks.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace RocketWorks.Networking
{
    public class NetworkController : MonoBehaviour
    {
        [SerializeField]
        private InputField ipInput;

        [SerializeField]
        private Button serverButton;

        [SerializeField]
        private Button clientButton;

        private SocketController controller;

        public void Init(SocketController controller)
        {
            this.controller = controller;
            clientButton.onClick.AddListener(Connect);
            serverButton.onClick.AddListener(StartServer);
        }

        private void Connect()
        {
            controller.SetupSocket(false);
            controller.Connect(ipInput.text, 9001);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            controller.Update();
        }

        private void StartServer()
        {
            new PoopyGameServer();
        }

        private void OnDestroy()
        {
            controller.CloseSocket();
        }
    }
}