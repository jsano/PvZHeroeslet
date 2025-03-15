using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour
{

    public int ID;
    private CardInfo cardInfo;
    private DeckBuilder DB;
    public Button add;
    public Button remove;
    public SpriteRenderer image;
    public TextMeshProUGUI atkUI;
    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI costUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
        DB = FindAnyObjectByType<DeckBuilder>(FindObjectsInactive.Include).GetComponent<DeckBuilder>();
    }

    public void Add()
    {
        DB.Add(ID);
    }

    public void Remove()
    {
        DB.Remove(ID);
    }

    public void ShowCardInfo()
    {
        StartCoroutine(cardInfo.Show(AllCards.Instance.cards[ID]));
    }

}
