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
        Tree
    }

    public Team team;
    public Type type;
    public Class _class;
    public List<Tribe> tribes;
    public bool token;

    public int cost;
    public int atk;
    public int HP;
    private int maxHP;
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

    private TextMeshProUGUI atkUI;
    private TextMeshProUGUI hpUI;
    private SpriteRenderer SR;
    private Sprite baseSprite;

    protected bool selected = true;
	protected List<BoxCollider2D> choices = new();
	private Camera cam;

    private bool frozen;

    private CardInfo cardInfo;

	// Start is called before the first frame update
	void Start()
    {
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		SR = GetComponent<SpriteRenderer>();
        baseSprite = SR.sprite;
        maxHP = HP;
        atkUI = transform.Find("ATK").GetComponent<TextMeshProUGUI>();
        atkUI.text = atk + "";
        hpUI = transform.Find("HP").GetComponent<TextMeshProUGUI>();
        hpUI.text = HP + "";
        if (gravestone)
        {
            SR.sprite = AllCards.Instance.gravestoneSprite;
            atkUI.gameObject.SetActive(false);
            hpUI.gameObject.SetActive(false);
        }
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
					if (bc.bounds.Contains((Vector2)cam.ScreenToWorldPoint(Input.mousePosition)))
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
        if (type == Type.Unit) GameManager.Instance.TriggerEvent("OnCardPlay", this);
        yield return GameManager.Instance.ProcessEvents();
        yield return GameManager.Instance.HandleHeroBlocks();
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
        if (antihero > 0)
        {
            if (AHactive && GetTarget(col).GetComponent<Card>() != null)
            {
                AHactive = false;
                RaiseAttack(-antihero);
            }
			if (!AHactive && GetTarget(col).GetComponent<Hero>() != null)
			{
				AHactive = true;
				RaiseAttack(antihero);
			}
		}
        yield return null;
    }

	/// <summary>
	/// Called whenever a card on the field is hurt
	/// </summary>
	/// <param name="hurt"> The card that received damage </param>
	protected virtual IEnumerator OnCardHurt(Damagable hurt)
	{
		yield return null;
	}

	/// <summary>
	/// Called whenever a card on the field dies
	/// </summary>
	/// <param name="died"> The card that died </param>
	protected virtual IEnumerator OnCardDeath(Card died)
    {
		if (antihero > 0)
		{
			if (!AHactive && GetTarget(col).GetComponent<Hero>() != null)
			{
				AHactive = true;
				RaiseAttack(antihero);
			}
		}
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
		if (antihero > 0)
		{
			if (AHactive && GetTarget(col).GetComponent<Card>() != null)
			{
				AHactive = false;
				RaiseAttack(-antihero);
			}
			if (!AHactive && GetTarget(col).GetComponent<Hero>() != null)
			{
				AHactive = true;
				RaiseAttack(antihero);
			}
		}
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

    protected virtual IEnumerator OnCardDraw(Team team)
    {
        yield return null;
    }

    public virtual IEnumerator Attack()
    {
        if (frozen)
        {
            yield return new WaitForSeconds(1);
            frozen = false;
            SR.material.color = Color.white;
            yield break;
        }
        if (atk <= 0 || gravestone) yield break;
        if (!nextDoor && splash == 0)
        {
            Damagable target = GetTarget(col);
            yield return target.ReceiveDamage(atk, bullseye, deadly, freeze);
		    // animation
        }
        else
        {
			Damagable[] target = new Damagable[]{ null, null, null };
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                if (splash > 0 && i != 0) target[i+1] = Tile.zombieTiles[0, col + i].planted;
                else target[i+1] = GetTarget(col + i);
            }
            int[] dealt = new int[3];
            for (int i = 0; i < 3; i++) if (target[i] != null) yield return target[i].ReceiveDamage(atk, bullseye, deadly, freeze);
			// animation
		}
	}

    public override IEnumerator ReceiveDamage(int dmg, bool bullseye = false, bool deadly = false, bool freeze = false)
    {//Debug.Log(row + " " + col + " got hit for " + dmg);
        if (gravestone) yield break;
        dmg -= armor;
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
        if (dmg > 0)
        {
            yield return HitVisual();
            if (deadly) hitByDeadly = true;
            GameManager.Instance.TriggerEvent("OnCardHurt", this);
            if (freeze) Freeze();
        }
        if ((HP <= 0 || hitByDeadly))
        {
            died = true;
            GameManager.Instance.TriggerEvent("OnCardDeath", this);
        }
    }

	public void Destroy()
	{
        died = true;
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
		atkUI.text = atk + "";
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

    public IEnumerator Reveal()
    {
        gravestone = false;
		SR.sprite = baseSprite;
		atkUI.gameObject.SetActive(true);
		hpUI.gameObject.SetActive(true);
        //play animation
        yield return OnThisPlay();
    }

    public void Freeze()
    {
        frozen = true;
        SR.material.color = Color.blue;
        GameManager.Instance.TriggerEvent("OnCardFreeze", this);
    }

    private Damagable GetTarget(int col)
    {
		Tile[,] opponentTiles = team == Team.Plant ? Tile.zombieTiles : Tile.plantTiles;
		if (opponentTiles[1, col].planted != null && !opponentTiles[1, col].planted.died) return opponentTiles[1, col].planted;
		if (opponentTiles[0, col].planted != null && !opponentTiles[0, col].planted.died) return opponentTiles[0, col].planted;
		if (team == Team.Plant) return GameManager.Instance.zombieHero;
        return GameManager.Instance.plantHero;
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
		StartCoroutine(cardInfo.Show(this));
	}

}
