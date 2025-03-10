using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{

    public TextMeshProUGUI title;
    public GameObject loading;
    public GameObject bans;
    public GameObject choose;
    public Button lockIn;

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
        bans.SetActive(true);
        lockIn.gameObject.SetActive(true);
        title.text = "Choose your bans...";
    }

    private void ChoosePhase()
    {
        LeanTween.moveLocalX(bans, -500, 0.5f);
        LeanTween.moveLocalX(choose, 0, 0.5f).setEaseOutQuad().setOnComplete(() => lockIn.interactable = true);
        title.text = "Choose your deck...";
    }

    public void LockedIn()
    {
        lockIn.interactable = false;
        LockInRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void LockInRpc()
    {
        ready += 1;
        if (ready == 2)
        {
            ready = 0;
            if (banCompleted) GetComponent<StartButtons>().ChangeScene("Game");
            else
            {
                banCompleted = true;
                ChoosePhase();
            }
        }
    }

}
