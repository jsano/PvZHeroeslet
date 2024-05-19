using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerDownHandler
{

    [HideInInspector] public int ID;
    private Camera cam;
    private Vector2 startPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.2f;
        startPos = transform.position;
        GameManager.selecting = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2) cam.ScreenToWorldPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        GameManager.selecting = null;
        foreach (Tile t in Tile.tileObjects)
        {
            if (t.GetComponent<BoxCollider2D>().bounds.Contains((Vector2) cam.ScreenToWorldPoint(eventData.position)))
            {
                Card card = Instantiate(AllCards.Instance.cards[ID], GameObject.Find("PlayedCards").transform).GetComponent<Card>();
                card.transform.position = t.transform.position;
                t.planted = card;
                card.row = t.row;
                card.col = t.col;
                Destroy(gameObject);
                return;
            }
        }
        transform.position = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
