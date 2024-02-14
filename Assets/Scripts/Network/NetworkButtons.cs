using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkButtons : MonoBehaviour
{
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        Destroy(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Destroy(gameObject);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
