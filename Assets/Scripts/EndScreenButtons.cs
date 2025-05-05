using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
        StartCoroutine(CheckOpponentQuit());
    }

    private IEnumerator CheckOpponentQuit()
    {
        yield return new WaitUntil(() => SessionManager.Instance.ActiveSession.PlayerCount < 2);
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
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Shutdown();
            Destroy(NetworkManager.gameObject);
        }
        SceneManager.LoadScene("StartSessions");
        AudioManager.Instance.PlayMusic("Menu");
    }

}
