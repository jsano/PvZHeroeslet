using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Multiplayer;

public class EndScreenButtons : NetworkBehaviour
{

    private int rematchers = 0;

    public Button rematchButton;
    public Button quitButton;
    
    public void Rematch(Button b)
    {
        rematchButton.interactable = false;
        quitButton.gameObject.SetActive(false);
        RematchRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void QuitRpc()
    {
        SessionManager.Instance.LeaveSession();
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Shutdown();
            Destroy(NetworkManager.gameObject);
        }
        rematchButton.interactable = false;
        rematchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Opponent left";
        quitButton.gameObject.SetActive(true);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RematchRpc()
    {
        rematchers += 1;
        if (rematchers == 2 && NetworkManager.Singleton.IsHost) NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void Quit()
    {
        QuitRpc();
        AudioManager.Instance.PlayMusic("Menu");
        SceneManager.LoadScene("Start");
    }

}
