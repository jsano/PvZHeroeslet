using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private CardInfo cardInfo;

	[HideInInspector] public bool interactable = false;
    public SpriteRenderer image;
	public TextMeshProUGUI atkUI;
	public TextMeshProUGUI hpUI;
    public TextMeshProUGUI costUI;

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
    private bool overridden;

    public static FinalStats MakeDefaultFS(int id)
    {
		Card c = AllCards.Instance.cards[id];
		return new FinalStats()
		{
			hp = c.HP,
			atk = c.atk,
			abilities = "",
			ID = id,
			cost = c.cost,
		};
	}

    public void OverrideFS(FinalStats fs)
    {
        overridden = true;
        finalStats = fs;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = transform.position;
        if (!interactable) return;
        transform.localScale = Vector3.one * 1.2f;

        GetComponent<SpriteRenderer>().sortingOrder += 10;
        image.sortingOrder += 10;
        atkUI.transform.parent.GetComponent<Canvas>().sortingOrder += 10;

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
                    if (Tile.CanPlantInCol(j, tileObjects, finalStats.abilities.Contains("teamUp") || orig.teamUp, finalStats.abilities.Contains("amphibious") || orig.amphibious))
                    {
                        if (i == 0 || 
                            i == 1 && (finalStats.abilities.Contains("teamUp") || orig.teamUp || tileObjects[0, j].planted != null && tileObjects[0, j].planted.teamUp))
                            validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                    }
                        
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
        if (Vector3.Distance(transform.position, startPos) < 0.1) StartCoroutine(cardInfo.Show(finalStats));
        if (!interactable) return;
        transform.localScale = Vector3.one;
        GetComponent<SpriteRenderer>().sortingOrder -= 10;
        image.sortingOrder -= 10;
        atkUI.transform.parent.GetComponent<Canvas>().sortingOrder -= 10;

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
            finalStats.cost = 1;
            GameManager.Instance.HoldTrickRpc(GameManager.Instance.team);
        }
		transform.position = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        orig = AllCards.Instance.cards[ID];
		image.sprite = orig.GetComponent<SpriteRenderer>().sprite;
        if (!overridden)
        {
            finalStats.hp = orig.HP;
            finalStats.atk = orig.atk;
            finalStats.abilities = "";
		    finalStats.ID = ID;
            finalStats.cost = orig.cost;
        }

        if (GameManager.Instance.team == Card.Team.Plant) tileObjects = Tile.plantTiles;
        else tileObjects = Tile.zombieTiles;

        cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!interactable) image.material.color = Color.gray;
        else image.material.color = Color.white;

        if (orig.type == Card.Type.Trick)
        {
			atkUI.text = "";
			hpUI.text = "";
        }
        else
        {
            atkUI.text = finalStats.atk + "";
		    hpUI.text = finalStats.hp + "";
        }
        costUI.text = finalStats.cost + "";
	}

    public int GetCost()
    {
        return finalStats.cost;
    }

    public void ChangeCost(int amount)
    {
        finalStats.cost += amount;
        costUI.text = finalStats.cost + "";
    }

    public void ChangeAttack(int amount)
    {
        finalStats.atk += amount;
        atkUI.text = finalStats.atk + "";
    }

    public void ChangeHP(int amount)
    {
        finalStats.hp += amount;
        hpUI.text = finalStats.hp + "";
    }

    public void AddAbility(string ability)
    {
        if (finalStats.abilities.Length == 0) finalStats.abilities += ability;
        else finalStats.abilities += " - " + ability;
    }

    /// <summary>
	/// Called whenever a card is played
	/// </summary>
	/// <param name="played"> The card that was played </param>
	protected virtual IEnumerator OnCardPlay(Card played)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field is hurt
    /// </summary>
    /// <param name="hurt"> [The card that received damage, the card that dealt the damage, the final amount dealt] </param>
    protected virtual IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field dies
    /// </summary>
    /// <param name="died"> The card that died </param>
    protected virtual IEnumerator OnCardDeath(Card died)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field moves
    /// </summary>
    /// <param name="moved"> The card that moved </param>
    protected virtual IEnumerator OnCardMoved(Card moved)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field gets frozen
    /// </summary>
    /// <param name="frozen"> The card that froze </param>
    protected virtual IEnumerator OnCardFreeze(Card frozen)
    {
        yield return null;
    }

    protected virtual IEnumerator OnTurnStart()
    {
        yield return null;
    }

    protected virtual IEnumerator OnTurnEnd()
    {
        yield return null;
    }

    protected virtual IEnumerator OnCardDraw(Card.Team team)
    {
        yield return null;
    }

}
