using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

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

        private bool disconnected;

        private Vector3 startPos;

        public void Init(SocketController controller)
        {
            startPos = transform.position;
            this.controller = controller;
            clientButton.onClick.AddListener(Connect);
            serverButton.onClick.AddListener(StartServer);
            controller.DisconnectEvent += OnDisconnect;
        }

        private void Connect()
        {
            controller.SetupSocket(false);
            controller.Connect(ipInput.text, 9001);
            transform.position = new Vector3(0, 1, 0) * Screen.height * 5f;
            disconnected = false;
        }

        private void OnDisconnect()
        {
            disconnected = true;
        }

        private void Update()
        {
            if (transform.position != (UnityEngine.Vector3)startPos && disconnected)
            {
                transform.position = startPos;
                GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i] != null)
                        GameObject.Destroy(objects[i]);
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
            }
        }

        private void OnSceneChange(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChange;
            PoopyGame.Main();
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