using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour
{

    public int ID;
    [HideInInspector]
    public bool hideButtons = false;
    private CardInfo cardInfo;
    private DeckBuilder DB;
    public Button add;
    public Button remove;
    public Image image;
    public TextMeshProUGUI atkUI;
    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI costUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
        DB = FindAnyObjectByType<DeckBuilder>(FindObjectsInactive.Include).GetComponent<DeckBuilder>();
        Card orig = AllCards.Instance.cards[ID];
        image.sprite = orig.GetComponent<SpriteRenderer>().sprite;
        if (orig.type == Card.Type.Trick)
        {
            atkUI.text = "";
            hpUI.text = "";
        }
        else
        {
            atkUI.text = orig.atk + "";
            hpUI.text = orig.HP + "";
        } 
        costUI.text = orig.cost + "";
        if (hideButtons) transform.Find("Buttons").gameObject.SetActive(false);
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
        StartCoroutine(cardInfo.Show(HandCard.MakeDefaultFS(ID)));
    }

}
