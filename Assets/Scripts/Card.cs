using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : Damagable
{

    public enum Team
    {
        Plant,
        Zombie
    }

    public enum Type
    {
        Unit,
        Trick
    }

    public enum Class
    {
        Guardian,
        Kabloom,
        MegaGrow,
        Smarty,
        Solar,
        Beastly,
        Brainy,
        Crazy,
        Hearty,
        Sneaky
    }

    public enum Tribe
    {
        Animal,
        Barrel,
        Bean,
        Berry,
        Cactus,
        Corn,
        Dancing,
        Dragon,
        Flower,
        Flytrap,
        Fruit,
        Gargantuar,
        Gourmet,
        History,
        Imp,
        Leafy,
        Mime,
        Monster,
        Moss,
        Mushroom,
        Mustache,
        Nut,
        Party,
        Pea,
        Pet,
        Pinecone,
        Pirate,
        Professional,
        Root,
        Science,
        Seed,
        Sports,
        Squash,
        Superpower,
        Tree
    }

    public Team team;
    public Type type;
    public Class _class;
    public List<Tribe> tribes;
    public bool token;

    public int cost;
    public int atk;
    private int baseAtk;
    public int HP;
    private int maxHP;
    private int baseHP;
    public bool died { get; private set; }

    public bool amphibious;
    public int antihero;
    private bool AHactive;
    public int armor;
    public bool bullseye;
    public bool deadly;
    private bool hitByDeadly;
    public bool doubleStrike;
    public bool frenzy;
    public bool gravestone;
    public bool hunt;
    public int overshoot;
    public int splash;
    public bool strikethrough;
    public bool teamUp;
    public bool untrickable;
    public bool nextDoor;
    public bool freeze;

    public string description;
    public string lore;

    [HideInInspector] public int row;
    [HideInInspector] public int col;
    /// <summary>
    /// Different from <c>cost</c> where this is the amount this card was actually played for (after all deductions, etc.)
    /// </summary>
    public int playedCost { get; private set; }
    /// <summary>
    /// The FinalStats instance this got its stats from. Ok to be null (and will be if it's instantiated by another card)
    /// </summary>
    public FinalStats sourceFS;

    /// <summary>
    /// Use if this card has a unique HandCard that reacts to GameEvents (ex. Trickster)
    /// </summary>
    public GameObject specialHandCard;
    private TextMeshProUGUI atkUI;
    private TextMeshProUGUI hpUI;
    private SpriteRenderer SR;
    private Sprite baseSprite;

    /// <summary>
    /// If this card on the player's side requires a selecting choice to be made, set this to false so it can start detecting mouse input.
    /// Different from <c>GameManager.Instance.selecting</c> since this is just for toggling mouse input and won't immediately resume game flow
    /// </summary>
    protected bool selected = true;
    /// <summary>
    /// If this card on the player's side requires a selecting choice to be made, populate this with all the valid choices it can make
    /// </summary>
	protected List<BoxCollider2D> choices = new();

    private bool frozen;

    private CardInfo cardInfo;

    void Awake()
    {
        if (!gravestone)
        {
            GameManager.Instance.currentlySpawningCards += 1;
            // Unless this is a gravestone, disable HandCards so that it can be enabled again at the end of OnThisPlay
            GameManager.Instance.DisableHandCards();
        }
        else GameManager.Instance.EnablePlayableHandCards();
    }

    // Start is called before the first frame update
    void Start()
    {
		SR = GetComponent<SpriteRenderer>();
        baseSprite = SR.sprite;
        baseHP = HP;
        baseAtk = atk;

        if (sourceFS != null)
        {
            atk = sourceFS.atk;
            HP = sourceFS.hp;
            string[] abilities = sourceFS.abilities.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in abilities)
            {
                string name = "";
                string value = "";
                for (int i = 0; i < s.Length; i++)
                {
                    if (char.IsDigit(s[i])) value += s[i];
                    else name += s[i];
                }
                GetType().GetField(name).SetValue(this, value.Length == 0 ? true : int.Parse(value));
            }
            playedCost = sourceFS.cost;
        }
        // The only way for a card to not have a FinalStats is if it was instantiated by something, in which case it should always be free
        else playedCost = 0;
        maxHP = HP;

        UpdateAntihero();

        atkUI = transform.Find("ATK").GetComponent<TextMeshProUGUI>();
        hpUI = transform.Find("HP").GetComponent<TextMeshProUGUI>();
        if (type == Type.Unit)
        {
            atkUI.text = atk + "";
            hpUI.text = HP + "";
        }
        else
        {
            atkUI.text = "";
            hpUI.text = "";
        }
        if (gravestone) Hide();
        else
        {
            //play animation
            // Trick play GameEvents should always process last chronologially, so force it to be added first on the stack
            if (type == Type.Trick) GameManager.Instance.TriggerEvent("OnCardPlay", this); 
            StartCoroutine(OnThisPlay());
        }
		cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.Instance.selecting && !selected)
        {
			if (Input.GetMouseButtonDown(0))
			{
				foreach (BoxCollider2D bc in choices)
				{
					if (bc.bounds.Contains((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)))
					{
                        selected = true;
                        StartCoroutine(OnSelection(bc));
                        break;
					}
				}
			}
		}
	}

    /// <summary>
    /// Override with the card's effect when a selecting choice is made
    /// </summary>
    protected virtual IEnumerator OnSelection(BoxCollider2D bc)
    {
        yield return null;
	}

	/// <summary>
	/// Called right when this card is played. Base method waits for all pending cards to spawn, triggers <c>OnCardPlay</c>, calls <c>ProcessEvents</c>, and toggles <c>waitingOnBlock=false</c>
	/// </summary>
	/// <param name="played"> The card that was played </param>
	protected virtual IEnumerator OnThisPlay()
	{
        if (GameManager.Instance.selecting) yield return new WaitUntil(() => GameManager.Instance.selecting == false);
        yield return new WaitForSeconds(0.1f); // this only exists to give time for rpcs to instantiate before processing events (rough fix)
        GameManager.Instance.currentlySpawningCards -= 1;
        yield return new WaitUntil(() => GameManager.Instance.currentlySpawningCards == 0); // this exists for cards that spawn cards that spawn cards
        if (type == Type.Unit) GameManager.Instance.TriggerEvent("OnCardPlay", this);
        yield return GameManager.Instance.ProcessEvents();
        playedCost = 0;
        GameManager.Instance.waitingOnBlock = false;
        if (type == Type.Trick)
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }
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
	protected virtual IEnumerator OnCardDeath(Card died)
    {
        if (died == this)
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
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
	/// Called whenever a card or hero on the field gets healed. NOT called when a card's max HP is raised
	/// </summary>
	/// <param name="healed"> The card that got healed </param>
	protected virtual IEnumerator OnHeal(Card healed)
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
    protected virtual IEnumerator OnCardDraw(Team team)
    {
        yield return null;
    }

    /// <summary>
	/// Makes this card attack its target. Ignores if it's in a gravestone, and unfreezes if frozen. Its target, if damaged, will each asynchronously call <c>ReceiveDamage</c>
	/// </summary>
    public virtual IEnumerator Attack()
    {
        if (frozen)
        {
            yield return new WaitForSeconds(0.5f);
            frozen = false;
            SR.material.color = Color.white;
            yield break;
        }
        if (atk <= 0 || gravestone) yield break;

        yield return new WaitForSeconds(0.5f);
        // animation

        // Single lane targets
        if (!nextDoor && splash == 0)
        {
            List<Damagable> targets = GetTargets(col);
            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(atk, this, bullseye, deadly, freeze));
            yield return null;
            // If this card has frenzy, and any of its targets died after its attack, signal to GameManager
            if (frenzy)
            {
                foreach (Damagable c in targets) if (c.GetComponent<Card>() != null && c.GetComponent<Card>().died) GameManager.Instance.frenzyActivate = this;
            }
        }
        // Multi lane targets
        else
        {
            List<Damagable>[] targets = new List<Damagable>[] { null, null, null };
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                // For splash damage, its next door targets (not in-lane) should only hit units and not the hero 
                if (splash > 0 && i != 0)
                {
                    if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) targets[i + 1] = new() { Tile.zombieTiles[0, col + i].planted };
                }
                else targets[i + 1] = GetTargets(col + i);
            }
            for (int i = 0; i < 3; i++) if (targets[i] != null) foreach (Damagable c in targets[i]) StartCoroutine(c.ReceiveDamage(atk, this, bullseye, deadly, freeze, col + i - 1));
        }
        yield return new WaitForSeconds(0.5f); // this only exists so all the receive damages get sent before the other units attack. TODO: fix??
	}

    /// <summary>
	/// Called when this unit receives any form of damage. Ignores if it's in a gravestone or invulnerable. 
    /// If the final damage dealt > 0, applies any effects and triggers <c>OnCardHurt</c>
	/// </summary>
    /// <param name="heroCol">For cards that hit multi-lane, the hero could be hit alongside other units. This is what the hero's "column" should be registered as to maintain proper order</param>
    public override IEnumerator ReceiveDamage(int dmg, Card source, bool bullseye = false, bool deadly = false, bool freeze = false, int heroCol = -1)
    {
        if (gravestone || invulnerable) yield break;
        dmg -= armor;
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
        if (dmg > 0)
        {
            GameManager.Instance.TriggerEvent("OnCardHurt", new Tuple<Damagable, Card, int, int>(this, source, dmg, heroCol));
            if (deadly) hitByDeadly = true;
            if ((HP <= 0 || hitByDeadly) && !died)
            {
                died = true;
                for (int i = 0; i < 2; i++)
                {
                    if (Tile.plantTiles[i, col].HasRevealedPlanted()) Tile.plantTiles[i, col].planted.UpdateAntihero();
                    if (Tile.zombieTiles[i, col].HasRevealedPlanted()) Tile.zombieTiles[i, col].planted.UpdateAntihero();
                }
                GameManager.Instance.TriggerEvent("OnCardDeath", this);
            }

            yield return HitVisual();
            if (freeze) Freeze();
        }
    }

    /// <summary>
	/// Marks this card to be destroyed and triggers <c>OnCardDeath</c> so that it's destroyed during the next <c>ProcessEvent</c>. Updates any anti-hero immediately.
    /// If this is a gravestone, updates the gold UI for the opponent's perspective
	/// </summary>
	public void Destroy()
	{
        if (died) return;
        died = true;
        for (int i = 0; i < 2; i++)
        {
            if (Tile.plantTiles[i, col].HasRevealedPlanted()) Tile.plantTiles[i, col].planted.UpdateAntihero();
            if (Tile.zombieTiles[i, col].HasRevealedPlanted()) Tile.zombieTiles[i, col].planted.UpdateAntihero();
        }
        if (gravestone && team != GameManager.Instance.team) GameManager.Instance.UpdateRemaining(playedCost, team);
		GameManager.Instance.TriggerEvent("OnCardDeath", this);
	}

    /// <summary>
	/// Raises HP by the given amount. Ignores if it's in a gravestone. Also triggers <c>OnHeal</c> if not raising the maxHP.
    /// If HP ends up as 0 afterwards, marks this card to be destroyed and triggers <c>OnCardDeath</c> so that it's destroyed during the next <c>ProcessEvent</c>, and updates any anti-hero immediately.
	/// </summary>
    /// <param name="raiseCap">If true, this will affect the maxHP, which also means it won't be considered damaged if amount is negative</param>
	public override void Heal(int amount, bool raiseCap=false)
    {
        if (gravestone) return;
        HP += amount;
        if (raiseCap) maxHP += amount;
        else 
        {
            HP = Mathf.Min(maxHP, HP);
            GameManager.Instance.TriggerEvent("OnHeal", this);
        }
        hpUI.text = HP + "";
        if (HP <= 0)
        {
            died = true;
            for (int i = 0; i < 2; i++)
            {
                if (Tile.plantTiles[i, col].HasRevealedPlanted()) Tile.plantTiles[i, col].planted.UpdateAntihero();
                if (Tile.zombieTiles[i, col].HasRevealedPlanted()) Tile.zombieTiles[i, col].planted.UpdateAntihero();
            }
            GameManager.Instance.TriggerEvent("OnCardDeath", this);
        }
    }

    /// <summary>
    /// Raises attack by the given ammount. Attack won't go below 0. Ignores if it's in a gravestone. Updates UI
    /// </summary>
    public void RaiseAttack(int amount)
	{
        if (gravestone) return;
		atk += amount;
        atk = Mathf.Max(0, atk);
		if (atkUI != null) atkUI.text = atk + "";
	}

	private IEnumerator HitVisual()
	{
		SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
		yield return new WaitForSeconds(0.1f);
		SR.material.color = Color.white;
	}

    /// <summary>
    /// Returns whether or not the current HP is less than maxHP, not including if the HP cap was reduced
    /// </summary>
    public override bool isDamaged()
    {
        return HP < maxHP;
    }

    /// <summary>
    /// For tricks, override with conditions indicating if the trick can be applied to this target
    /// </summary>
    public virtual bool IsValidTarget(BoxCollider2D bc)
    {
        return true;
    }

    /// <summary>
    /// If this unit has an anti-hero ability, check if this hits the opponent hero and update attack and UI
    /// </summary>
    public void UpdateAntihero()
    {
        if (antihero > 0)
        {
            if (AHactive && GetTargets(col)[0].GetComponent<Card>() != null)
            {
                AHactive = false;
                RaiseAttack(-antihero);
            }
            if (!AHactive && GetTargets(col)[0].GetComponent<Hero>() != null)
            {
                AHactive = true;
                RaiseAttack(antihero);
            }
        }
    }

    /// <summary>
    /// Toggles off the gravestone state.
    /// Reveals the card sprite, updates UI, increments <c>GameManager.instance.currentlySpawningCards</c>, disables HandCards, then calls <c>OnThisPlay</c> (where it'll be enabled again)
    /// </summary>
    public IEnumerator Reveal()
    {
        gravestone = false;
		SR.sprite = baseSprite;
		atkUI.gameObject.SetActive(true);
		hpUI.gameObject.SetActive(true);
        UpdateAntihero();
        GameManager.Instance.currentlySpawningCards += 1;
        //play animation
        GameManager.Instance.DisableHandCards();
        yield return OnThisPlay();
    }

    /// <summary>
    /// Toggles on the gravestone state. Hides the card sprite behind a gravestone, and resets stats to prefab values
    /// </summary>
    public void Hide()
    {
        atk = baseAtk;
        HP = baseHP;
        gravestone = true;
        SR.sprite = AllCards.Instance.gravestoneSprite;
        atkUI.gameObject.SetActive(false);
        hpUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Freezes the card, and triggers <c>OnCardFreeze</c>
    /// </summary>
    public void Freeze()
    {
        frozen = true;
        SR.material.color = Color.blue;
        GameManager.Instance.TriggerEvent("OnCardFreeze", this);
    }

    /// <summary>
    /// Removes the card's gameObject from the scene, and the team's player gains a HandCard for this card with prefab values
    /// </summary>
    public void Bounce()
    {
        if (team == Team.Plant) Tile.plantTiles[row, col].Unplant();
        else Tile.zombieTiles[row, col].Unplant();
        GameManager.Instance.GainHandCard(team, AllCards.NameToID(name.Substring(0, name.IndexOf("("))));
        Destroy(gameObject);
    }

    /// <summary>
    /// Gets a list of targets that this card's Attack will hit on the given column. Unless this has strikethrough, it'll likely be only 1 target (prioritizes row 1, then 0, then the hero)
    /// </summary>
    protected List<Damagable> GetTargets(int col)
    {
        List<Damagable> ret = new();
		Tile[,] opponentTiles = team == Team.Plant ? Tile.zombieTiles : Tile.plantTiles;
		if (opponentTiles[1, col].planted != null && !opponentTiles[1, col].planted.died) ret.Add(opponentTiles[1, col].planted);
		if (opponentTiles[0, col].planted != null && !opponentTiles[0, col].planted.died) ret.Add(opponentTiles[0, col].planted);
		if (team == Team.Plant) ret.Add(GameManager.Instance.zombieHero);
        else ret.Add(GameManager.Instance.plantHero);
        if (strikethrough) return ret;
        ret.RemoveRange(1, ret.Count - 1);
        return ret;
    }

    /// <summary>
    /// Toggles the card's invulnerability status
    /// </summary>
    public override void ToggleInvulnerability(bool active)
    {
        invulnerable = active;
        if (active) SR.material.color = Color.yellow;
        else if (SR.material.color != Color.blue) SR.material.color = Color.white;
    }

    void OnMouseDown()
	{
        // Don't show card info if the player is currently selecting something
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
                Card c;
                if (GameManager.Instance.team == Team.Plant) c = Tile.plantTiles[row, col].planted;
                else c = Tile.zombieTiles[row, col].planted;
				if (c != null && GameManager.Instance.selecting) return;
			}
		}
        // Don't show gravestone card info for the plant perspective
		if (GameManager.Instance.team == Team.Zombie || !gravestone) cardInfo.Show(this);
	}

}
