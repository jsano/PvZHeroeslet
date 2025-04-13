using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SuperpowerDeckCard : MonoBehaviour, IDragHandler, IPointerUpHandler
{

    public int ID;
    private CardInfo cardInfo;
    private DeckBuilder DB;
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DB = FindAnyObjectByType<DeckBuilder>(FindObjectsInactive.Include).GetComponent<DeckBuilder>();
        Card orig = AllCards.Instance.cards[ID];
        image.sprite = orig.GetComponent<SpriteRenderer>().sprite;
        cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
    }

    public void ShowCardInfo()
    {
        cardInfo.Show(AllCards.Instance.cards[ID]);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!eventData.dragging) cardInfo.Show(AllCards.Instance.cards[ID]);
        else DB.UpdateSuperpowerOrder(transform, ID);
    }
}
