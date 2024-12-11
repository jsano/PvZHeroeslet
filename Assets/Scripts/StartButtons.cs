using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

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

}
