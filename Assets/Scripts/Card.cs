using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

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
        Trick,
        Terrain
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
    private bool initializedStats = false;

    public bool amphibious;
    public int antihero;
    private bool AHactive;
    public int armor;
    public int bullseye;
    public int deadly;
    private bool hitByDeadly;
    public int doubleStrike;
    public int frenzy;
    public bool baseGravestone { get; private set; }
    public bool gravestone;
    public int strengthHeart;
    public bool hunt;
    public int overshoot;
    public int splash;
    public int strikethrough;
    public bool teamUp;
    public int untrickable;
    public bool nextDoor;
    public bool freeze;

    public string description;
    public string lore;

    [HideInInspector] public int row;
    [HideInInspector] public int col;
    public int oldRow { get; private set; }
    public int oldCol { get; private set; }

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
    private Image atkSprite;
    private Image hpSprite;
    protected SpriteRenderer SR;
    private Sprite baseSprite;

    /// <summary>
    /// If this card on the player's side requires a selecting choice to be made, set this to false so it can start detecting mouse input.
    /// </summary>
    protected bool selected = true;
    /// <summary>
    /// If this card on the player's side requires a selecting choice to be made, populate this with all the valid <b>tile or hero</b> choices it can make
    /// </summary>
	protected List<BoxCollider2D> choices = new();
    private float timer = 10;

    protected bool frozen;

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
        baseGravestone = gravestone;

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
                var field = GetType().GetField(name);
                if (field.FieldType == typeof(Int32)) field.SetValue(this, value.Length == 0 ? (int)field.GetValue(this) + 1 : int.Parse(value));
                else field.SetValue(this, true);
            }
            playedCost = sourceFS.cost;
        }
        // The only way for a card to not have a FinalStats is if it was instantiated by something, in which case it should always be free
        else playedCost = 0;
        maxHP = HP;

        initializedStats = true;
        UpdateAntihero();

        atkUI = transform.Find("ATK").GetComponentInChildren<TextMeshProUGUI>();
        hpUI = transform.Find("HP").GetComponentInChildren<TextMeshProUGUI>();
        atkSprite = atkUI.GetComponentInParent<Image>();
        hpSprite = hpUI.GetComponentInParent<Image>();
        if (type == Type.Unit)
        {
            atkUI.text = atk + "";
            hpUI.text = HP + "";
        }
        else
        {
            atkSprite.gameObject.SetActive(false);
            hpSprite.gameObject.SetActive(false);
        }
        if (type == Type.Terrain) SR.sortingOrder = -1;
        if (gravestone) Hide();
        else
        {
            //play animation
            // Trick play GameEvents should always process last chronologially, so force it to be added first on the stack
            if (type != Type.Unit) GameManager.Instance.TriggerEvent("OnCardPlay", this); 
            StartCoroutine(OnThisPlay());
        }
		cardInfo = FindAnyObjectByType<CardInfo>(FindObjectsInactive.Include).GetComponent<CardInfo>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!selected)
        {
            // Show targets visual
            foreach (BoxCollider2D bc in choices)
            {
                if (bc.GetComponent<Tile>() != null) bc.GetComponent<Tile>().ToggleTarget(true);
                else bc.GetComponent<Hero>().ToggleTarget(true);
            }

            if (Input.GetMouseButtonDown(0))
			{
				foreach (BoxCollider2D bc in choices)
				{
					if (bc.bounds.Contains((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)))
					{
                        // Hide targets visual
                        foreach (BoxCollider2D bc1 in choices)
                        {
                            if (bc1.GetComponent<Tile>() != null) bc1.GetComponent<Tile>().ToggleTarget(false);
                            else bc1.GetComponent<Hero>().ToggleTarget(false);
                        }

                        selected = true;
                        Tile t = bc.GetComponent<Tile>();
                        if (t != null) GameManager.Instance.SelectingChosenRpc(t.isPlantTile ? Team.Plant : Team.Zombie, t.row, t.col);
                        else GameManager.Instance.SelectingChosenRpc(bc.GetComponent<Hero>().team, -1, -1);
                        break;
					}
				}
			}

            timer -= Time.deltaTime;
            GameManager.Instance.timerImage.gameObject.SetActive(true);
            GameManager.Instance.timerImage.fillAmount = 0.18f + 0.64f * timer / 10;
            if (timer <= 0)
            {
                foreach (BoxCollider2D bc1 in choices)
                {
                    if (bc1.GetComponent<Tile>() != null) bc1.GetComponent<Tile>().ToggleTarget(false);
                    else bc1.GetComponent<Hero>().ToggleTarget(false);
                }
                selected = true;
                var bc = choices[0];
                Tile t = bc.GetComponent<Tile>();
                if (t != null) GameManager.Instance.SelectingChosenRpc(t.isPlantTile ? Team.Plant : Team.Zombie, t.row, t.col);
                else GameManager.Instance.SelectingChosenRpc(bc.GetComponent<Hero>().team, -1, -1);
            }
		}

        atkSprite.sprite = GetAttackIcon();
        hpSprite.sprite = GetHPIcon();
	}

    /// <summary>
    /// Override with the card's effect when a selecting choice is made. Base method clears selection so base should be called at the beginning
    /// </summary>
    protected virtual IEnumerator OnSelection(BoxCollider2D bc)
    {
        GameManager.Instance.ClearSelection();
        GameManager.Instance.timerImage.gameObject.SetActive(false);
        yield return null;
	}

	/// <summary>
	/// Called right when this card is played. Base method waits for all pending cards to spawn, triggers <c>OnCardPlay</c>, calls <c>ProcessEvents</c>, and toggles <c>waitingOnBlock=false</c>
	/// </summary>
	/// <param name="played"> The card that was played </param>
	protected virtual IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(0.1f); // this only exists to give time for rpcs to instantiate before processing events (rough fix)
        GameManager.Instance.currentlySpawningCards -= 1;
        GameManager.Instance.waitingOnBlock = false;
        yield return new WaitUntil(() => GameManager.Instance.currentlySpawningCards == 0); // this exists for cards that spawn cards that spawn cards
        if (type == Type.Unit) GameManager.Instance.TriggerEvent("OnCardPlay", this);
        yield return GameManager.Instance.ProcessEvents();
        playedCost = 0; // For any consecutive gravestone reveals
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
        if (hunt && played.type == Type.Unit && played.team != team && (amphibious || played.col != 4))
        {
            if (team == Team.Plant && !Tile.CanPlantInCol(played.col, Tile.plantTiles, teamUp, amphibious) || team == Team.Zombie && Tile.zombieTiles[0, played.col].planted != null) yield break;
            yield return new WaitForSeconds(1);
            Move(row, played.col);
        }
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
	/// <param name="died"> [The card that died, the card that destroyed it] </param>
	protected virtual IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
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
	protected virtual IEnumerator OnCardStatsChanged(Card changed)
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
	/// Called during tricks phase
	/// </summary>
    public virtual IEnumerator OnZombieTricks()
    {
        yield return null;
    }

    public virtual IEnumerator BeforeCombat()
    {
        if (overshoot > 0)
        {
            yield return new WaitForSeconds(1);
            yield return AttackFX(team == Team.Plant ? Tile.zombieHeroTiles[col] : Tile.plantHeroTiles[col]);
            yield return team == Team.Plant ? GameManager.Instance.zombieHero.ReceiveDamage(overshoot, this) : GameManager.Instance.plantHero.ReceiveDamage(overshoot, this);
        }
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
        
        int finalAtk = strengthHeart > 0 ? HP : atk;
        if (finalAtk <= 0 || gravestone) yield break;

        yield return new WaitForSeconds(0.25f);

        // Single lane targets
        if (!nextDoor && splash == 0)
        {
            List<Damagable> targets = GetTargets(col);
            yield return AttackFX(targets[0]);

            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(finalAtk, this, bullseye > 0, deadly > 0, freeze));
            yield return null;
            // If this card has frenzy, and any of its targets died after its attack, signal to GameManager
            if (frenzy > 0)
            {
                foreach (Damagable c in targets) if (c.GetComponent<Card>() != null && c.GetComponent<Card>().died) GameManager.Instance.frenzyActivate = this;
            }
        }
        else if (splash > 0)
        {
            List<Damagable>[] targets = new List<Damagable>[] { null, null, null };
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                // For splash damage, its next door targets (not in-lane) should only hit units and not the hero 
                if (i == 0) targets[i + 1] = GetTargets(col + i);
                else if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) targets[i + 1] = new() { Tile.zombieTiles[0, col + i].planted };
            }
            yield return AttackFX(targets[1][0]);

            for (int i = 0; i < 3; i++) if (targets[i] != null) foreach (Damagable c in targets[i]) StartCoroutine(c.ReceiveDamage(i == 1 ? finalAtk : splash, this, bullseye > 0, deadly > 0, freeze));
        }
        else if (nextDoor)
        {
            List<Damagable>[] targets = new List<Damagable>[] { null, null, null };
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                targets[i + 1] = GetTargets(col + i);
            }
            List<Damagable> temp = new();
            for (int i = 0; i < 3; i++) if (targets[i] != null) temp.Add(targets[i][0]);
            yield return AttackFXs(temp);

            for (int i = 0; i < 3; i++) if (targets[i] != null) foreach (Damagable c in targets[i]) StartCoroutine(c.ReceiveDamage(finalAtk, this, bullseye > 0, deadly > 0, freeze));
        }
        yield return new WaitForSeconds(0.1f); // this only exists so all the receive damages get sent before the other units attack. TODO: fix??
	}

    /// <summary>
	/// Makes this card perform a bonus attack. Identical to <c>Attack</c> but also triggers and processes events
	/// </summary>
    public IEnumerator BonusAttack()
    {
        if (team == Team.Plant)
        {
            Card s = Tile.IsOnField("Bonus Track Buckethead");
            if (s != null) yield break;
        }
        if (team == Team.Zombie)
        {
            Card s = Tile.IsOnField("Wing-nut");
            if (s != null) yield break;
        }

        GameManager.Instance.DisableHandCards();
        yield return Attack();
        GameManager.Instance.TriggerEvent("OnCardBonusAttack", this);
        yield return GameManager.Instance.ProcessEvents();
    }

    /// <summary>
	/// Called when this unit receives any form of damage. Ignores if it's in a gravestone or invulnerable. 
    /// If the final damage dealt > 0, applies any effects and triggers <c>OnCardHurt</c>
	/// </summary>
    /// <param name="heroCol">Never pass in directly. See <c>Tile.ReceiveDamage</c></param>
    public override IEnumerator ReceiveDamage(int dmg, Card source, bool bullseye = false, bool deadly = false, bool freeze = false, int heroCol = -1)
    {
        if (gravestone || invulnerable) yield break;
        dmg = Mathf.Max(0, dmg - armor);
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
                GameManager.Instance.TriggerEvent("OnCardDeath", new Tuple<Card, Card>(this, source));
            }

            if (isDamaged()) hpUI.color = new Color(1, 0.5f, 0.5f);

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
        if (gravestone)
        {
            SR.sprite = baseSprite;
            if (team != GameManager.Instance.team) GameManager.Instance.UpdateRemaining(playedCost, team);
        }
        GameManager.Instance.TriggerEvent("OnCardDeath", new Tuple<Card, Card>(this, null));
    }

    /// <summary>
	/// Heals HP by the given amount. Ignores if it's in a gravestone. Also triggers <c>OnCardHeal</c> if not raising the maxHP.
    /// 
	/// </summary>
    /// <param name="raiseCap">If true, this will affect the maxHP, which also means it won't be considered damaged if amount is negative</param>
	public override IEnumerator Heal(int amount)
    {
        if (gravestone) yield break;
        int HPBefore = HP;
        HP += amount;
        HP = Mathf.Min(maxHP, HP);
        if (amount > 0 && HPBefore < maxHP) GameManager.Instance.TriggerEvent("OnCardHeal", new Tuple<Card, int>(this, maxHP - HPBefore));
        hpUI.text = HP + "";
        if (!isDamaged()) hpUI.color = Color.white;
        yield return GameManager.Instance.ProcessEvents();
    }

    /// <summary>
    /// Raises attack and HP by the given amounts. Ignores if it's in a gravestone. Updates UI. Attack won't go below 0.
    /// If HP ends up as 0 afterwards, marks this card to be destroyed and triggers <c>OnCardDeath</c> so that it's destroyed during the next <c>ProcessEvent</c>, and updates any anti-hero immediately.
    /// </summary>
    public override void ChangeStats(int atkAmount, int hpAmount)
	{
        if (gravestone) return;
		atk += atkAmount;
        atk = Mathf.Max(0, atk);
		if (atkUI != null) atkUI.text = atk + "";
        
        maxHP += hpAmount;
        HP += hpAmount;
        if (hpUI != null) hpUI.text = HP + "";

        if (atkAmount > 0 || hpAmount > 0) GameManager.Instance.TriggerEvent("OnCardStatsChanged", this);
        if (HP <= 0)
        {
            died = true;
            for (int i = 0; i < 2; i++)
            {
                if (Tile.plantTiles[i, col].HasRevealedPlanted()) Tile.plantTiles[i, col].planted.UpdateAntihero();
                if (Tile.zombieTiles[i, col].HasRevealedPlanted()) Tile.zombieTiles[i, col].planted.UpdateAntihero();
            }
            GameManager.Instance.TriggerEvent("OnCardDeath", new Tuple<Card, Card>(this, null));
        }
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
        if (!initializedStats) return;
        if (antihero > 0)
        {
            if (AHactive && GetTargets(col)[0].GetComponent<Card>() != null)
            {
                AHactive = false;
                ChangeStats(-antihero, 0);
            }
            if (!AHactive && GetTargets(col)[0].GetComponent<Tile>() != null)
            {
                AHactive = true;
                ChangeStats(antihero, 0);
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
        atkSprite.gameObject.SetActive(true);
        hpSprite.gameObject.SetActive(true);
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
        atkSprite.gameObject.SetActive(false);
        hpSprite.gameObject.SetActive(false);
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
        StartCoroutine(BounceHelper());
    }

    private IEnumerator BounceHelper()
    {
        SR.sortingLayerID = 0;
        GetComponent<Canvas>().sortingLayerID = 0;
        yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID(name.Substring(0, name.IndexOf("("))), sourceFS.permanent ? sourceFS : null, false);
        Destroy(gameObject);
    }

    /// <summary>
    /// Move this card to this new row/column visually and internally. Triggers a card moved GameEvent
    /// </summary>
    /// <param name="nrow"></param>
    /// <param name="ncol"></param>
    public void Move(int nrow, int ncol)
    {
        oldRow = row;
        oldCol = col;
        if (team == Team.Plant)
        {
            Tile.plantTiles[row, col].Unplant();
            Tile.plantTiles[nrow, ncol].Plant(this);
        }
        else
        {
            Tile.zombieTiles[row, col].Unplant();
            Tile.zombieTiles[nrow, ncol].Plant(this);
        }
        GameManager.Instance.TriggerEvent("OnCardMoved", this);
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
		if (team == Team.Plant) ret.Add(Tile.zombieHeroTiles[col]);
        else ret.Add(Tile.plantHeroTiles[col]);
        if (strikethrough > 0) return ret;
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

    protected IEnumerator AttackFX(Damagable dest)
    {
        GameObject g = Instantiate(AllCards.Instance.attackFX, transform.position, Quaternion.identity);
        g.GetComponent<AttackFX>().destination = dest.transform;
        yield return new WaitForSeconds(0.25f);
    }

    protected IEnumerator AttackFXs(List<Damagable> dests)
    {
        foreach (Damagable d in dests)
        {
            GameObject g = Instantiate(AllCards.Instance.attackFX, transform.position, Quaternion.identity);
            g.GetComponent<AttackFX>().destination = d.transform;
        }
        yield return new WaitForSeconds(0.25f);
    }

    void OnMouseDown()
	{
        // Don't show card info if the player is currently selecting something. TODO: doesn't work for trick selected
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
                Card c;
                if (GameManager.Instance.team == Team.Plant) c = Tile.plantTiles[row, col].planted;
                else c = Tile.zombieTiles[row, col].planted;
				if (c != null && !c.selected) return;
			}
		}
        // Don't show gravestone card info for the plant perspective
		if (GameManager.Instance.team == Team.Zombie || !gravestone) cardInfo.Show(this);
	}

    public Sprite GetAttackIcon()
    {
        var icons = AllCards.Instance;
        List<Sprite> ret = new();
        if (strikethrough > 0) ret.Add(icons.strikethroughSprite);
        if (antihero > 0) ret.Add(icons.antiheroSprite);
        if (bullseye > 0) ret.Add(icons.bullseyeSprite);
        if (deadly > 0) ret.Add(icons.deadlySprite);
        if (doubleStrike > 0) ret.Add(icons.doubleStrikeSprite);
        if (frenzy > 0) ret.Add(icons.frenzySprite);
        if (overshoot > 0) ret.Add(icons.overshootSprite);
        if (ret.Count > 1) return icons.multiSprite;
        if (ret.Count == 0) return icons.attackSprite;
        return ret[0];
}

    public Sprite GetHPIcon()
    {
        var icons = AllCards.Instance;
        List<Sprite> ret = new();
        if (armor > 0) ret.Add(icons.armorSprite);
        if (untrickable > 0) ret.Add(icons.untrickableSprite);
        if (frozen) ret.Add(icons.frozenSprite);
        if (invulnerable) ret.Add(icons.invulnerableSprite);
        if (strengthHeart > 0) ret.Add(icons.strengthHeartSprite);
        if (ret.Count > 1) return icons.multiSprite;
        if (ret.Count == 0) return icons.HPSprite;
        return ret[0];
    }

}
