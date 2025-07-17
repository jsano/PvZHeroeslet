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

        atkUI.GetComponentInParent<Image>().sprite = orig.GetAttackIcon();
        hpUI.GetComponentInParent<Image>().sprite = orig.GetHPIcon();
        if (orig.type == Card.Type.Unit)
        {
            atkUI.text = orig.atk + "";
            hpUI.text = orig.HP + "";
        }
        else
        {
            atkUI.transform.parent.gameObject.SetActive(false);
            hpUI.transform.parent.gameObject.SetActive(false);
        } 
        costUI.text = orig.cost + "";
        if (orig.team == Card.Team.Zombie) costUI.GetComponentInParent<Image>().sprite = AllCards.Instance.brainUI;
        if (hideButtons) add.gameObject.SetActive(false);
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
        cardInfo.Show(AllCards.Instance.cards[ID]);
    }

}
