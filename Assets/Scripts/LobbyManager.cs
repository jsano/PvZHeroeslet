using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class LobbyManager : NetworkBehaviour
{

    public TextMeshProUGUI title;
    public GameObject loading;
    public GameObject teamUI;
    public GameObject banUI;
    public GameObject chooseUI;
    public Button lockIn;
    public GameObject code;

    private Team team;
    private List<LobbyUIButton> bans = new();

    private int ready = 0;
    private int phase = 0; // 0: team, 1: ban, 2: deck, 3: start

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.rotateAroundLocal(loading, Vector3.forward, -360f, 2f).setRepeat(-1);
        if (IsHost) NetworkManager.OnConnectionEvent += P2Joined;
        else TeamPhase();
    }

    private void P2Joined(NetworkManager nm, ConnectionEventData data)
    {
        if (data.EventType == ConnectionEvent.PeerConnected)
        {
            Debug.Log(data.EventType + " " + data.ClientId);
            TeamPhase();
        }
    }

    private void TeamPhase()
    {
        loading.SetActive(false);
        code.SetActive(false);
        teamUI.SetActive(true);
        lockIn.gameObject.SetActive(true);
        title.text = "Choose your team...";
    }

    private void BanPhase()
    {
        LeanTween.moveLocalX(teamUI, -500, 0.5f);
        LeanTween.moveLocalX(banUI, 0, 0.5f);
        if (team == Team.Plant) banUI.transform.Find("BansZ").gameObject.SetActive(true);
        else banUI.transform.Find("BansP").gameObject.SetActive(true);
        title.text = "Choose your bans...";
    }

    private void ChoosePhase()
    {
        LeanTween.moveLocalX(banUI, -500, 0.5f);
        LeanTween.moveLocalX(chooseUI, 0, 0.5f).setEaseOutQuad();
        if (team == Team.Plant) chooseUI.transform.Find("ChooseP").gameObject.SetActive(true);
        else chooseUI.transform.Find("ChooseZ").gameObject.SetActive(true);
        title.text = "Choose your deck...";
    }

    public void LockedIn()
    {
        lockIn.interactable = false;
        if (phase == 0) foreach (Transform t in teamUI.transform) t.GetComponent<Button>().interactable = false;
        if (phase == 1) foreach (Transform _t in banUI.transform) foreach (Transform t in _t) t.GetComponent<Button>().interactable = false;
        if (phase == 2) foreach (Transform _t in chooseUI.transform) foreach (Transform t in _t.Find("Heroes")) t.GetComponent<Button>().interactable = false;

        if (phase == 1) LockInBanRpc(bans[0].ID, bans[1].ID, IsHost);
        else LockInRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void LockInRpc()
    {
        ready += 1;
        if (ready == 2)
        {
            ready = 0;
            phase += 1;
            if (phase == 1) BanPhase();
            if (phase == 2) ChoosePhase();
            if (phase == 3) GetComponent<StartButtons>().ChangeNetworkScene("Game");
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void LockInBanRpc(int ban1, int ban2, bool isHost)
    {
        if (isHost != IsHost)
        {
            foreach (Transform _t in chooseUI.transform) foreach (Transform t in _t.Find("Heroes"))
            {
                LobbyUIButton b = t.GetComponent<LobbyUIButton>();
                if (b.ID == ban1 || b.ID == ban2) b.Disable();
            }

        }

        ready += 1;
        if (ready == 2)
        {
            ready = 0;
            phase += 1;
            if (phase == 1) BanPhase();
            if (phase == 2) ChoosePhase();
            if (phase == 3) GetComponent<StartButtons>().ChangeNetworkScene("Game");
        }
    }

    public void TeamButton(bool plant)
    {
        team = plant ? Team.Plant : Team.Zombie;
        lockIn.interactable = true;
    }
    
    public void BanButton(LobbyUIButton b)
    {
        //NOTE: apparently inspector onclick doesn't have defined order so b.selected might happen after this code (maybe wait1frame?)
        if (b.selected) bans.Add(b);
        else bans.Remove(b);
        
        if (bans.Count >= 2)
        {
            foreach (Transform _t in banUI.transform) foreach (Transform t in _t) if (!t.GetComponent<LobbyUIButton>().selected) t.GetComponent<Button>().interactable = false;
            lockIn.interactable = true;
        }
        else
        {
            foreach (Transform _t in banUI.transform) foreach (Transform t in _t) t.GetComponent<Button>().interactable = true;
            lockIn.interactable = false;
        }
    }

    public void ChooseButton(LobbyUIButton b)
    {
        //useraccounts stuff here

        lockIn.interactable = true;
    }

}
