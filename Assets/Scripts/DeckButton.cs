using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{

    [HideInInspector]
    public string deckName;
    [HideInInspector]
    public bool forLobby;
    private DeckList DL;
    private Image image;
    public TextMeshProUGUI nameText;

    public static bool deleting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!forLobby) DL = transform.parent.GetComponent<DeckList>();
        image = GetComponent<Image>();
        nameText.text = deckName;
    }

    void Update()
    {
        if (deleting) image.color = Color.red;
        else image.color = Color.white;
    }

    public void OnClick()
    {
        if (forLobby)
        {
            UserAccounts.GameStats.DeckName = deckName;
            FindAnyObjectByType<LobbyManager>(FindObjectsInactive.Include).GetComponent<LobbyManager>().ChoseDeck();
            return;
        }
        if (deleting)
        {
            DL.Remove(deckName);
            deleting = false;
        }
        else DL.LoadDeck(deckName);
        
    }

}
