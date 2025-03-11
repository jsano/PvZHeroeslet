using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{

    public TextMeshProUGUI title;
    public GameObject loading;
    public GameObject banUI;
    public GameObject chooseUI;
    public Button lockIn;
    public GameObject code;

    private List<LobbyUIButton> bans = new();

    private int ready = 0;
    private bool banCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.rotateAroundLocal(loading, Vector3.forward, -360f, 2f).setRepeat(-1);
        if (IsHost) NetworkManager.OnConnectionEvent += P2Joined;
        else BanPhase();
    }

    private void P2Joined(NetworkManager nm, ConnectionEventData data)
    {
        if (data.EventType == ConnectionEvent.PeerConnected)
        {
            Debug.Log(data.EventType + " " + data.ClientId);
            BanPhase();
        }
    }

    private void BanPhase()
    {
        loading.SetActive(false);
        code.SetActive(false);
        banUI.SetActive(true);
        lockIn.gameObject.SetActive(true);
        title.text = "Choose your bans...";
    }

    private void ChoosePhase()
    {
        LeanTween.moveLocalX(banUI, -500, 0.5f);
        LeanTween.moveLocalX(chooseUI, 0, 0.5f).setEaseOutQuad();//.setOnComplete(() => lockIn.interactable = true);
        title.text = "Choose your deck...";
    }

    public void LockedIn()
    {
        lockIn.interactable = false;
        LockInRpc(bans[0].ID, bans[1].ID, IsHost);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void LockInRpc(int ban1, int ban2, bool isHost)
    {
        if (!banCompleted)
        {
            if (isHost != IsHost)
            {
                Debug.Log("here");
                foreach (Transform t in chooseUI.transform.Find("Heroes"))
                {
                    LobbyUIButton b = t.GetComponent<LobbyUIButton>();
                    if (b.ID == ban1 || b.ID == ban2) b.Disable();
                }
                    
            }
        }

        ready += 1;
        if (ready == 2)
        {
            ready = 0;
            if (banCompleted) GetComponent<StartButtons>().ChangeNetworkScene("Game");
            else
            {
                banCompleted = true;
                ChoosePhase();
            }
        }
    }
    
    public void BanButton(LobbyUIButton b)
    {
        if (b.selected) bans.Add(b);
        else bans.Remove(b);
        if (bans.Count >= 2)
        {
            foreach (Transform t in banUI.transform) if (!t.GetComponent<LobbyUIButton>().selected) t.GetComponent<Button>().interactable = false;
            lockIn.interactable = true;
        }
        else
        {
            foreach (Transform t in banUI.transform) t.GetComponent<Button>().interactable = true;
            lockIn.interactable = false;
        }
    }

    public void ChooseButton(LobbyUIButton b)
    {

        lockIn.interactable = true;
    }

}
