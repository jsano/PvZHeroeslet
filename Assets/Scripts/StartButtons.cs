using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Multiplayer;

public class StartButtons : NetworkBehaviour
{
    
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
            NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

}
