using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public int ID;
    private Camera cam;
    private Vector2 startPos;

    [HideInInspector] public bool interactable = false;
    public SpriteRenderer image;

    [Serializable]
    public struct mods : INetworkSerializable
    {
        public int atk;
        public int hp;
        public string abilities;
        public int ID;
        public int cost;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref atk);
			serializer.SerializeValue(ref hp);
			serializer.SerializeValue(ref abilities);
            serializer.SerializeValue(ref ID);
			serializer.SerializeValue(ref cost);
		}
	}
    private mods Mods;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.localScale = Vector3.one * 1.2f;
        startPos = transform.position;
        GameManager.Instance.selecting = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.position = (Vector2) cam.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.localScale = Vector3.one;
        GameManager.Instance.selecting = null;
        foreach (Tile t in Tile.tileObjects)
        {
            if (t.GetComponent<BoxCollider2D>().bounds.Contains((Vector2) cam.ScreenToWorldPoint(eventData.position)))
            {
                GameManager.Instance.PlayCardRpc(Mods, t.row, t.col, GameManager.Instance.team);
				GameManager.Instance.UpdateBrains(-Mods.cost);
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
        Card orig = AllCards.Instance.cards[ID];
		image.sprite = orig.GetComponent<SpriteRenderer>().sprite;
        Mods.hp = orig.HP;
        Mods.atk = orig.atk;
        Mods.abilities = "";
        Mods.ID = ID;
        Mods.cost = orig.cost;
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactable) image.material.color = Color.gray;
        else image.material.color = Color.white;
    }

    public int GetCost()
    {
        return Mods.cost;
    }

}
