using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [HideInInspector] public int row;
    [HideInInspector] public int col;
    [HideInInspector] public int playedCost;
    public FinalStats sourceFS;

    public GameObject specialHandCard;
    private TextMeshProUGUI atkUI;
    private TextMeshProUGUI hpUI;
    private SpriteRenderer SR;
    private Sprite baseSprite;

    protected bool selected = true;
	protected List<BoxCollider2D> choices = new();

    private bool frozen;

    private CardInfo cardInfo;

    void Awake()
    {
        if (!gravestone)
        {
            GameManager.Instance.currentlySpawningCards += 1;
            GameManager.Instance.DisableHandCards();
        }
        else GameManager.Instance.EnablePlayableHandCards();
    }

    // Start is called before the first frame update
    void Start()
    {
		SR = GetComponent<SpriteRenderer>();
        baseSprite = SR.sprite;
        maxHP = HP;
        baseHP = HP;
        baseAtk = atk;
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
                        StartCoroutine(OnSelection(bc));
                        selected = true;
                        break;
					}
				}
			}
		}
	}

    protected virtual IEnumerator OnSelection(BoxCollider2D bc)
    {
        yield return null;
	}

	/// <summary>
	/// Called right when this card is played. Base method checks for deaths, calls OnCardPlay left to right, and then enables handcards
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

	protected virtual IEnumerator OnTurnStart()
    {
        yield return null;
    }

    protected virtual IEnumerator OnTurnEnd()
    {
        yield return null;
    }

    protected virtual IEnumerator OnCardDraw(Team team)
    {
        yield return null;
    }

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
        if (!nextDoor && splash == 0)
        {
            List<Damagable> targets = GetTargets(col);
            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(atk, this, bullseye, deadly, freeze));
            yield return null;
            if (frenzy)
            {
                foreach (Damagable c in targets) if (c.GetComponent<Card>() != null && c.GetComponent<Card>().died) GameManager.Instance.frenzyActivate = this;
            }
        }
        else
        {
            List<Damagable>[] targets = new List<Damagable>[] { null, null, null };
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                if (splash > 0 && i != 0)
                {
                    if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) targets[i + 1] = new() { Tile.zombieTiles[0, col + i].planted };
                }
                else targets[i + 1] = GetTargets(col + i);
            }
            for (int i = 0; i < 3; i++) if (targets[i] != null) foreach (Damagable c in targets[i]) StartCoroutine(c.ReceiveDamage(atk, this, bullseye, deadly, freeze, col + i - 1));
        }
        yield return new WaitForSeconds(0.5f); // TODO: fix??
	}

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

	public override void Heal(int amount, bool raiseCap=false)
    {
        if (gravestone) return;
        HP += amount;
        if (raiseCap) maxHP += amount;
        else HP = Mathf.Min(maxHP, HP);
        hpUI.text = HP + "";
        if (HP <= 0)
        {
            died = true;
            GameManager.Instance.TriggerEvent("OnCardDeath", this);
        }
    }

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

    public override bool isDamaged()
    {
        return HP < maxHP;
    }

    public virtual bool IsValidTarget(BoxCollider2D bc)
    {
        return true;
    }

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

    public void Hide()
    {
        atk = baseAtk;
        HP = baseHP;
        gravestone = true;
        SR.sprite = AllCards.Instance.gravestoneSprite;
        atkUI.gameObject.SetActive(false);
        hpUI.gameObject.SetActive(false);
    }

    public void Freeze()
    {
        frozen = true;
        SR.material.color = Color.blue;
        GameManager.Instance.TriggerEvent("OnCardFreeze", this);
    }

    public void Bounce()
    {
        if (team == Team.Plant) Tile.plantTiles[row, col].Unplant();
        else Tile.zombieTiles[row, col].Unplant();
        GameManager.Instance.GainHandCard(team, AllCards.NameToID(name.Substring(0, name.IndexOf("("))));
        Destroy(gameObject);
    }

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

    public override void ToggleInvulnerability(bool active)
    {
        invulnerable = active;
        if (active) SR.material.color = Color.yellow;
        else if (SR.material.color != Color.blue) SR.material.color = Color.white;
    }

    void OnMouseDown()
	{
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
		if (GameManager.Instance.team == Team.Zombie || !gravestone) cardInfo.Show(this);
	}

}
