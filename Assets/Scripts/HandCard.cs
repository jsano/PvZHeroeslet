using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class HandCard : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public int ID;
    public Card orig { get; private set; }
    /// <summary>
    /// The starting position that this should snap back to when let go
    /// </summary>
    private Vector2 startPos;
    /// <summary>
    /// The reference to the Tile array that represents the player's half of the board
    /// </summary>
    private Tile[,] tileObjects;
    /// <summary>
    /// All of the valid tiles/heroes that this can legally be played on
    /// </summary>
	private HashSet<BoxCollider2D> validChoices = new();

    private CardInfo cardInfo;

	[HideInInspector] public bool interactable = false;
    public Image image;
	public TextMeshProUGUI atkUI;
	public TextMeshProUGUI hpUI;
    public TextMeshProUGUI costUI;
    public GameObject info;

    private FinalStats finalStats;
    [HideInInspector] public bool conjured = false; // Maybe somehow track the card that conjured??

    /// <summary>
    /// If this HandCard should have different stats than its prefab values, then call this right after instantiating. Otherwise it'll use the prefab values
    /// </summary>
    /// <param name="fs"></param>
    public void OverrideFS(FinalStats fs)
    {
        finalStats = fs;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = transform.position;
        
        // Layer this above all other handcards
        GetComponent<SpriteRenderer>().sortingOrder += 10;
        GetComponent<Canvas>().sortingOrder += 10;
        
        if (!interactable) return;
        if (GetComponent<SpriteRenderer>().sortingLayerName != "Error") transform.localScale = Vector3.one;

        // Recalculate at every pointer down since the board state can change throughout the game
        validChoices.Clear();
        if (orig.type == Card.Type.Trick)
        {
            // If this is a trick, use its IsValidTarget method to determine where it can be played
            foreach (Tile t in Tile.plantTiles) if (orig.IsValidTarget(t.GetComponent<BoxCollider2D>())) validChoices.Add(t.GetComponent<BoxCollider2D>());
            foreach (Tile t in Tile.zombieTiles) if (orig.IsValidTarget(t.GetComponent<BoxCollider2D>())) validChoices.Add(t.GetComponent<BoxCollider2D>());
            if (orig.IsValidTarget(GameManager.Instance.plantHero.GetComponent<BoxCollider2D>())) validChoices.Add(GameManager.Instance.plantHero.GetComponent<BoxCollider2D>());
            if (orig.IsValidTarget(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>())) validChoices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());
        }
        else if (orig.type == Card.Type.Terrain)
        {
            foreach (Tile t in Tile.terrainTiles) validChoices.Add(t.GetComponent<BoxCollider2D>());
        }
        else
        {
            // If this is a unit, first see if a column has at least 1 space where it can be planted, then see which one(s)
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Tile.CanPlantInCol(j, tileObjects, finalStats.abilities.Contains("teamUp") || orig.teamUp, finalStats.abilities.Contains("amphibious") || orig.amphibious))
                    {
                        if (i == 0 ||
                            i == 1 && (finalStats.abilities.Contains("teamUp") || orig.teamUp ||
                                        tileObjects[0, j].planted != null && tileObjects[0, j].planted.teamUp || tileObjects[1, j].planted != null && tileObjects[1, j].planted.teamUp))
                            validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                    }
                    if (tileObjects[i, j].HasRevealedPlanted() && orig.evolution != Card.Tribe.Animal && !orig.gravestone) {
                        if (orig.evolution == Card.Tribe.Moss) {
                            if (tileObjects[i, j].planted.teamUp) validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                        }
                        else if (orig.evolution == Card.Tribe.Seed)
                        {
                            if (tileObjects[i, j].planted.team == orig.team) validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                        }
                        else if (tileObjects[i, j].planted.tribes.Contains(orig.evolution)) validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                    }
                    if (tileObjects[i, j].HasRevealedPlanted() && tileObjects[i, j].planted.fusion && !orig.gravestone) validChoices.Add(tileObjects[i, j].GetComponent<BoxCollider2D>());
                }
            }
        }
        // Show targets visual only if it's not a global trick
        if (orig.type == Card.Type.Trick && orig.GetType().GetMethod("IsValidTarget").DeclaringType == typeof(Card)) GameManager.Instance.boardHighlight.color += new Color(0, 0, 0, 0.6f);
        else foreach (BoxCollider2D bc in validChoices)
        {
            if (bc.GetComponent<Tile>() != null) bc.GetComponent<Tile>().ToggleTarget(true);
            else bc.GetComponent<Hero>().ToggleTarget(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable) return;
        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(eventData.position);
        if (validChoices.Count < 22) foreach (BoxCollider2D bc in validChoices)
		{
            Tile t = bc.GetComponent<Tile>();
            if (t == null) continue; // TODO: change
            if (bc.bounds.Contains((Vector2)Camera.main.ScreenToWorldPoint(eventData.position))) t.ToggleHighlight(true);
            else t.ToggleHighlight(false);
		}
	}

    public void OnPointerUp(PointerEventData eventData)
    {
        // Hide targets visual
        foreach (BoxCollider2D bc in validChoices)
        {
            Tile t = bc.GetComponent<Tile>();
            if (t != null)
            {
                t.ToggleHighlight(false);
                t.ToggleTarget(false);
            }
            else bc.GetComponent<Hero>().ToggleTarget(false);
        }
        GameManager.Instance.boardHighlight.color = new Color(1, 1, 1, 0);

        // Show card UI if it wasn't currently dragging. Only play if it was dragging
        if (!eventData.dragging)
        {
            cardInfo.Show(orig, finalStats);
        }
        transform.position = startPos;

        if (GetComponent<SpriteRenderer>().sortingLayerName != "Error") transform.localScale = Vector3.one * 0.9f;
        // Revert layering from pointer down
        GetComponent<SpriteRenderer>().sortingOrder -= 10;
        GetComponent<Canvas>().sortingOrder -= 10;
        
        if (!interactable) return;

        // If the pointer let go at a valid choice, play this card
        if (!(orig.type == Card.Type.Trick && orig.GetType().GetMethod("IsValidTarget").DeclaringType == typeof(Card)))
        {
            foreach (BoxCollider2D bc in validChoices)
            {
                Tile t = bc.GetComponent<Tile>();
                if (bc.bounds.Contains((Vector2)Camera.main.ScreenToWorldPoint(eventData.position)))
                {
                    if (orig.type == Card.Type.Unit)
                    {
                        if (finalStats.cost < 0) finalStats.cost = 0;
                        GameManager.Instance.PlayCardRpc(finalStats, t.row, t.col, true);
                        transform.SetParent(null);
                        Destroy(gameObject);
                    }
                    else if (orig.IsValidTarget(bc))
                    {
                        if (finalStats.cost < 0) finalStats.cost = 0;
                        if (t == null) GameManager.Instance.PlayTrickRpc(finalStats, -1, -1, bc.GetComponent<Hero>().team == Card.Team.Plant);
                        else GameManager.Instance.PlayTrickRpc(finalStats, t.row, t.col, t.isPlantTile);
                        transform.SetParent(null);
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            // Global trick
            if (GameManager.Instance.boardHighlight.GetComponent<BoxCollider2D>().bounds.Contains((Vector2)Camera.main.ScreenToWorldPoint(eventData.position)))
            {
                if (finalStats.cost < 0) finalStats.cost = 0;
                GameManager.Instance.PlayTrickRpc(finalStats, 1, 2, GameManager.Instance.team == Card.Team.Plant); // Params shouldn't matter beyond visual
                transform.SetParent(null);
                Destroy(gameObject);
            }
        }
        // If this is a superpower HandCard created from a block, hold on to it if the pointer let go at the "HandCard area"
        if (GameManager.Instance.waitingOnBlock && transform.parent != null && transform.parent.GetComponent<BoxCollider2D>().bounds.Contains((Vector2)Camera.main.ScreenToWorldPoint(eventData.position)))
        {
            GameManager.Instance.UpdateHandCardPositions();
            startPos = transform.position;
            interactable = false;
            GameManager.Instance.HoldTrickRpc(GameManager.Instance.team);
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        orig = AllCards.Instance.cards[ID];
		image.sprite = orig.GetComponent<SpriteRenderer>().sprite;
        // If no stat override was given, use the prefab values
        if (finalStats == null) finalStats = new FinalStats(ID);

        atkUI.GetComponentInParent<Image>().sprite = orig.GetAttackIcon();
        hpUI.GetComponentInParent<Image>().sprite = orig.GetHPIcon();
        if (orig.type == Card.Type.Unit)
        {
            atkUI.text = finalStats.atk + (orig.team == Card.Team.Plant ? GameManager.Instance.plantPermanentAttackBonus : GameManager.Instance.zombiePermanentAttackBonus) + 
                                            (ID == AllCards.NameToID("Clique Peas") ? GameManager.Instance.cliquePeas : 0) + "";
            hpUI.text = finalStats.hp + (orig.team == Card.Team.Plant ? GameManager.Instance.plantPermanentHPBonus : GameManager.Instance.zombiePermanentHPBonus) +
                                            (ID == AllCards.NameToID("Clique Peas") ? GameManager.Instance.cliquePeas : 0) + "";
        }
        else
        {
            if (orig.type == Card.Type.Terrain) {
                var rectTransform = image.GetComponent<RectTransform>();
                rectTransform.offsetMax = new(rectTransform.offsetMax.x, 0.5f);
                rectTransform.offsetMin = new(rectTransform.offsetMin.x, -0.5f);
            }
            atkUI.transform.parent.gameObject.SetActive(false);
            hpUI.transform.parent.gameObject.SetActive(false);
        }
        if (ID == AllCards.NameToID("Clique Peas")) finalStats.cost += GameManager.Instance.cliquePeas;
        costUI.text = finalStats.cost + "";

        if (orig.team == Card.Team.Zombie) costUI.GetComponentInParent<Image>().sprite = AllCards.Instance.brainUI;

        if (GameManager.Instance.team == Card.Team.Plant) tileObjects = Tile.plantTiles;
        else tileObjects = Tile.zombieTiles;

        cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!interactable) image.color = Color.gray;
        else image.color = Color.white;
	}

    public int GetCost()
    {
        return finalStats.cost;
    }

    public void ChangeCost(int amount)
    {
        finalStats.cost += amount;
        costUI.text = Math.Max(0, finalStats.cost) + "";
    }

    public void ChangeAttack(int amount, bool absolute = false)
    {
        if (orig.type != Card.Type.Unit) return;
        finalStats.atk += amount;
        if (absolute) finalStats.atk = amount;
        atkUI.text = finalStats.atk + (orig.team == Card.Team.Plant ? GameManager.Instance.plantPermanentAttackBonus : GameManager.Instance.zombiePermanentAttackBonus) +
                                        (ID == AllCards.NameToID("Clique Peas") ? GameManager.Instance.cliquePeas : 0) + "";
    }

    public void ChangeHP(int amount, bool absolute = false)
    {
        if (orig.type != Card.Type.Unit) return;
        finalStats.hp += amount;
        if (absolute) finalStats.hp = amount;
        hpUI.text = finalStats.hp + (orig.team == Card.Team.Plant ? GameManager.Instance.plantPermanentHPBonus : GameManager.Instance.zombiePermanentHPBonus) +
                                        (ID == AllCards.NameToID("Clique Peas") ? GameManager.Instance.cliquePeas : 0) + "";
    }

    public void AddAbility(string ability)
    {
        if (orig.type != Card.Type.Unit) return;
        if (finalStats.abilities.Length == 0) finalStats.abilities += ability;
        else finalStats.abilities += " - " + ability;
    }

    public void ShowInfo()
    {
        info.SetActive(true);
        info.GetComponentInChildren<TextMeshProUGUI>().text = orig.description;
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
    /// <param name="hurt"> [The card that received damage, the card that dealt the damage, the final amount dealt, hero column relative to other simultaneous calls] </param>
    protected virtual IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field dies
    /// </summary>
    /// <param name="died"> The card that died </param>
    protected virtual IEnumerator OnCardDeath(Tuple<Card, Card> died)
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

    /// <summary>
	/// Called whenever a card on the field gets healed. NOT called when a card's max HP is raised
	/// </summary>
	/// <param name="healed"> The card that got healed </param>
	protected virtual IEnumerator OnCardHeal(Tuple<Card, int> healed)
    {
        yield return null;
    }

    /// <summary>
	/// Identical to OnCardHeal, but for heroes. Called even when max HP is raised
	/// </summary>
	protected virtual IEnumerator OnHeroHeal(Tuple<Hero, int> healed)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card changes attack or max HP. Not called when healed
	/// </summary>
	protected virtual IEnumerator OnCardStatsChanged(Tuple<Card, int, int> changed)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card does a bonus attack
	/// </summary>
	protected virtual IEnumerator OnCardBonusAttack(Card attacked)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card is bounced
	/// </summary>
	protected virtual IEnumerator OnCardBounce(Card bounced)
    {
        yield return null;
    }

    /// <summary>
	/// Called at the start of turn
	/// </summary>
	protected virtual IEnumerator OnTurnStart()
    {
        yield return null;
    }

    /// <summary>
	/// Called at the end of turn
	/// </summary>
    protected virtual IEnumerator OnTurnEnd()
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card gets drawn by a player
	/// </summary>
    /// <param name="team">The team that drew the card</param>
    protected virtual IEnumerator OnCardDraw(Card.Team team)
    {
        yield return null;
    }

}
