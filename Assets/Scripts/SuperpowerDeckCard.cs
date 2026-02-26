using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SuperpowerDeckCard : MonoBehaviour, IDragHandler, IPointerUpHandler
{

    public int ID;
    private DeckBuilder DB;
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DB = FindAnyObjectByType<DeckBuilder>(FindObjectsInactive.Include).GetComponent<DeckBuilder>();
        Card orig = AllCards.Instance.cards[ID];
        image.sprite = orig.GetComponent<SpriteRenderer>().sprite;

        GetComponent<Image>().color = Color.Lerp(Buff.classColors[orig._class], Color.white, 0.8f);
    }

    /*public void ShowCardInfo()
    {
        cardInfo.Show(AllCards.Instance.cards[ID]);
    }*/

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!eventData.dragging) CardInfo.Instance.Show(AllCards.Instance.cards[ID]);
        else DB.UpdateSuperpowerOrder(transform, ID);
    }
}
