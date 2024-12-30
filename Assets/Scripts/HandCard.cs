using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public int ID;
    private Card orig;
    private Camera cam;
    private Vector2 startPos;
	private List<Tile> validTiles = new();

	[HideInInspector] public bool interactable = false;
    public SpriteRenderer image;

    [Serializable]
    public struct FinalStats : INetworkSerializable
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
    private FinalStats finalStats;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.localScale = Vector3.one * 1.2f;
        startPos = transform.position;

        validTiles.Clear();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (j == 4 && !finalStats.abilities.Contains("amphibious") && !orig.amphibious) continue;
                /*if (i == 1 && !finalStats.abilities.Contains("teamup") && !orig.teamUp && (Tile.tileObjects[0, j].planted == null || !Tile.tileObjects[0, j].planted.teamUp)) continue;
                if (i == 0 && Tile.tileObjects[0, j].planted != null && !Tile.tileObjects[0, j].planted.teamUp) continue;
				if (i == 0 && Tile.tileObjects[1, j].planted != null && !Tile.tileObjects[1, j].planted.teamUp) continue;*/
                if (Tile.tileObjects[0, j].planted != null && Tile.tileObjects[1, j].planted != null) continue;
				bool hasTeamup = false;
				if (Tile.tileObjects[0, j].planted != null && Tile.tileObjects[0, j].planted.teamUp) hasTeamup = true;
                if (Tile.tileObjects[1, j].planted != null && Tile.tileObjects[1, j].planted.teamUp) hasTeamup = true;
                if (hasTeamup || finalStats.abilities.Contains("teamup") || orig.teamUp) validTiles.Add(Tile.tileObjects[i, j]);
                else if (i == 0 && Tile.tileObjects[0, j].planted == null) validTiles.Add(Tile.tileObjects[i, j]);
			}
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.position = (Vector2) cam.ScreenToWorldPoint(eventData.position);
		foreach (Tile t in validTiles)
		{
            if (t.GetComponent<BoxCollider2D>().bounds.Contains((Vector2)cam.ScreenToWorldPoint(eventData.position))) t.ToggleHighlight(true);
            else t.ToggleHighlight(false);
		}
	}

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.localScale = Vector3.one;
        foreach (Tile t in validTiles)
        {
            t.ToggleHighlight(false);
            if (t.GetComponent<BoxCollider2D>().bounds.Contains((Vector2) cam.ScreenToWorldPoint(eventData.position)))
            {
                GameManager.Instance.PlayCardRpc(finalStats, t.row, t.col, GameManager.Instance.team);
				GameManager.Instance.UpdateBrains(-finalStats.cost);
				Destroy(gameObject);
            }
        }
        transform.position = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        orig = AllCards.Instance.cards[ID];
		image.sprite = orig.GetComponent<SpriteRenderer>().sprite;
        finalStats.hp = orig.HP;
        finalStats.atk = orig.atk;
        finalStats.abilities = "";
		finalStats.ID = ID;
        finalStats.cost = orig.cost;
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactable) image.material.color = Color.gray;
        else image.material.color = Color.white;
    }

    public int GetCost()
    {
        return finalStats.cost;
    }

}
