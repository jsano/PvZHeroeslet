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
    public GameObject note;
    public GameObject banUI;
    public GameObject chooseUI;
    public Button lockIn;
    public GameObject code;

    private int chosenTeam; // 0: plant, 1: zombie, 2: either
    private int opponentChosenTeam;
    private Team team;
    private List<LobbyUIButton> bans = new();
    private int hero;

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
        note.SetActive(true);
        lockIn.gameObject.SetActive(true);
        title.text = "Choose your team...";
    }

    private void BanPhase()
    {
        LeanTween.moveLocalX(teamUI, -500, 0.5f).setEaseOutQuad();
        LeanTween.moveLocalX(note, -500, 0.5f).setEaseOutQuad();
        LeanTween.moveLocalX(banUI, 0, 0.5f).setEaseOutQuad();
        if (team == Team.Plant) banUI.transform.Find("BansZ").gameObject.SetActive(true);
        else banUI.transform.Find("BansP").gameObject.SetActive(true);
        title.text = "Choose your bans...";
    }

    private void ChoosePhase()
    {
        LeanTween.moveLocalX(banUI, -500, 0.5f).setEaseOutQuad();
        LeanTween.moveLocalX(chooseUI, 0, 0.5f).setEaseOutQuad();
        if (team == Team.Plant) chooseUI.transform.Find("ChooseP").gameObject.SetActive(true);
        else chooseUI.transform.Find("ChooseZ").gameObject.SetActive(true);
        title.text = "Choose your deck...";
    }

    public void LockedIn()
    {
        lockIn.interactable = false;
        if (phase == 0)
        {
            foreach (Transform t in teamUI.transform) t.GetComponent<Button>().interactable = false;
            LockInTeamRpc(IsHost, chosenTeam);
        }
        else if (phase == 1)
        {
            foreach (Transform _t in banUI.transform) foreach (Transform t in _t) t.GetComponent<Button>().interactable = false;
            LockInBanRpc(bans[0].ID, bans[1].ID, IsHost);
        }
        else if (phase == 2)
        {
            foreach (Transform _t in chooseUI.transform) foreach (Transform t in _t.Find("Heroes")) t.GetComponent<Button>().interactable = false;
            LockInGameRpc(IsHost, hero);
        }
    }

    
    [Rpc(SendTo.ClientsAndHost)]
    private void LockInTeamRpc(bool host, int team)
    {
        if (IsHost != host) opponentChosenTeam = team;

        ready += 1;
        if (ready == 2)
        {
            ready = 0;
            phase += 1;
            if (IsHost)
            {
                if (chosenTeam == opponentChosenTeam)
                {
                    bool roll = UnityEngine.Random.Range(0, 2) == 0;
                    AssignTeamRpc(true, roll);
                    AssignTeamRpc(false, !roll);
                }
                else
                {
                    if (chosenTeam == 2) AssignTeamRpc(true, opponentChosenTeam == 0 ? false : true);
                    else AssignTeamRpc(true, chosenTeam == 0 ? true : false);
                    if (opponentChosenTeam == 2) AssignTeamRpc(false, chosenTeam == 0 ? false : true);
                    else AssignTeamRpc(false, opponentChosenTeam == 0 ? true : false);
                }
                TeamAssignCompleteRpc();
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TeamAssignCompleteRpc()
    {
        BanPhase();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void AssignTeamRpc(bool host, bool plant)
    {
        if (IsHost == host)
        {
            team = plant ? Team.Plant : Team.Zombie;
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
            ChoosePhase();
        }
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void LockInGameRpc(bool host, int id)
    {
        if (IsHost != host)
        {
            if (team == Team.Plant) UserAccounts.GameStats.ZombieHero = id;
            else UserAccounts.GameStats.PlantHero = id;
        }
        ready += 1;
        if (ready == 2)
        {
            GetComponent<StartButtons>().ChangeNetworkScene("Game");
        }
    }

    public void TeamButton(int t)
    {
        chosenTeam = t;
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
        if (team == Team.Plant) UserAccounts.GameStats.PlantHero = b.ID;
        else UserAccounts.GameStats.ZombieHero = b.ID;
        hero = b.ID;

        //todo: lead to deck
        lockIn.interactable = true;
    }

}
