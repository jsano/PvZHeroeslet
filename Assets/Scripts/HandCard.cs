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
    private Tile[,] tileObjects;
	private List<BoxCollider2D> validChoices = new();

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

        validChoices.Clear();
        if (orig.type == Card.Type.Trick)
        {
			foreach (Tile t in Tile.plantTiles) validChoices.Add(t.GetComponent<BoxCollider2D>());
			foreach (Tile t in Tile.zombieTiles) validChoices.Add(t.GetComponent<BoxCollider2D>());
			validChoices.Add(GameManager.Instance.plantHero.GetComponent<BoxCollider2D>());
			validChoices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());
		}
        else
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (j == 4 && !finalStats.abilities.Contains("amphibious") && !orig.amphibious) continue;
                    if (tileObjects[0, j].planted != null && tileObjects[1, j].planted != null) continue;
				    bool hasTeamup = false;
				    if (tileObjects[0, j].planted != null && tileObjects[0, j].planted.teamUp) hasTeamup = true;
                    if (tileObjects[1, j].planted != null && tileObjects[1, j].planted.teamUp) hasTeamup = true;
                    if (hasTeamup || finalStats.abilities.Contains("teamup") || orig.teamUp) validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                    else if (i == 0 && tileObjects[0, j].planted == null) validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
			    }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.position = (Vector2) cam.ScreenToWorldPoint(eventData.position);
		foreach (BoxCollider2D bc in validChoices)
		{
            Tile t = bc.GetComponent<Tile>();
            if (t == null) continue; // TODO: change
            if (bc.bounds.Contains((Vector2)cam.ScreenToWorldPoint(eventData.position)))
            {
                if (orig.type == Card.Type.Unit || orig.IsValidTarget(bc)) t.ToggleHighlight(true);
                else t.ToggleHighlight(false);
			}
            else t.ToggleHighlight(false);
		}
	}

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.localScale = Vector3.one;
        foreach (BoxCollider2D bc in validChoices)
        {
            Tile t = bc.GetComponent<Tile>();
			if (t != null) t.ToggleHighlight(false);
            if (bc.bounds.Contains((Vector2) cam.ScreenToWorldPoint(eventData.position)))
            {
                if (orig.type == Card.Type.Unit)
                {
                    GameManager.Instance.PlayCardRpc(finalStats, t.row, t.col);
					Destroy(gameObject);
				}
                else if (orig.IsValidTarget(bc))
                {
                    if (t == null) GameManager.Instance.PlayTrickRpc(finalStats, -1, -1, bc.GetComponent<Hero>().team == Card.Team.Plant);
                    else GameManager.Instance.PlayTrickRpc(finalStats, t.row, t.col, t.isPlantTile);
                    Destroy(gameObject);
                }
            }
        }
        if (GameManager.Instance.waitingOnBlock && transform.parent.GetComponent<BoxCollider2D>().bounds.Contains((Vector2)cam.ScreenToWorldPoint(eventData.position)))
        {
            
            startPos = cam.ScreenToWorldPoint(eventData.position);
            interactable = false;
            GameManager.Instance.HoldTrickRpc();
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

        /*if (orig.type == Card.Type.Trick)
        {
            if (orig.target == Card.Target.Plants || orig.target == Card.Target.PlantsAndHero) tileObjects = Tile.plantTiles;
            else if (orig.target == Card.Target.Zombies || orig.target == Card.Target.ZombiesAndHero) tileObjects = Tile.zombieTiles;
        }
        else
        {*/
        if (GameManager.Instance.team == Card.Team.Plant) tileObjects = Tile.plantTiles;
        else tileObjects = Tile.zombieTiles;
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
