using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Multiplayer;

public class StartButtons : NetworkBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        SceneManager.LoadScene("SampleScene");
    }

    public void StartGame()
    {
        Debug.Log(NetworkManager.Singleton.IsHost);
        if (NetworkManager.Singleton.IsHost)
            NetworkManager.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        else SceneManager.LoadScene("SampleScene");
    }

}
