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

    public int cost;
    public int atk;
    public int HP;
    private int maxHP;

    public bool amphibious;
    public bool antihero;
    public int armor;
    public bool bullseye;
    public bool deadly;
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

    [HideInInspector] public int row;
    [HideInInspector] public int col;
    [HideInInspector] public int playedCost;

    private TextMeshProUGUI atkUI;
    private TextMeshProUGUI hpUI;
    private SpriteRenderer SR;
    private Sprite baseSprite;

    protected bool selecting;
	protected List<BoxCollider2D> choices = new();
	private Camera cam;

    private bool frozen;

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
		    StartCoroutine(OnThisPlay());
		}
    }

	// Update is called once per frame
	void Update()
	{
		if (selecting)
        {
			if (Input.GetMouseButtonDown(0))
			{
				foreach (BoxCollider2D bc in choices)
				{
					if (bc.bounds.Contains((Vector2)cam.ScreenToWorldPoint(Input.mousePosition)))
					{
                        StartCoroutine(OnSelection(bc));
						selecting = false;
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
        GameManager.Instance.DisableHandCards();
        yield return GameManager.Instance.CheckDeaths();
        yield return GameManager.CallLeftToRight("OnCardPlay", this);
		GameManager.Instance.EnablePlayableHandCards();
        GameManager.Instance.waitingOnBlock = false;
        if (type == Type.Trick) Destroy(gameObject);
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
    /// Called whenever a card on the field attacks something
    /// </summary>
    /// <param name="source"> The card that attacked </param>
    protected virtual IEnumerator OnCardAttack(Card source)
    {
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
        if (!nextDoor)
        {
            Damagable target = null;
            Tile[,] opponentTiles = team == Team.Plant ? Tile.zombieTiles : Tile.plantTiles;
            if (opponentTiles[1, col].planted != null) target = opponentTiles[1, col].planted;
            else if (opponentTiles[0, col].planted != null) target = opponentTiles[0, col].planted;
            if (target == null) 
            {
                if (team == Team.Plant) target = GameManager.Instance.zombieHero;
                else target = GameManager.Instance.plantHero;
            }
		    int dealt = target.ReceiveDamage(atk, bullseye);
		    // animation
		    yield return new WaitForSeconds(1);
            //
            yield return GameManager.CallLeftToRight("OnCardAttack", this);
            if (dealt > 0)
            {    
		        yield return GameManager.CallLeftToRight("OnCardHurt", target);
            }
        }
        else
        {
			Damagable[] target = new Damagable[]{ null, null, null };
			Tile[,] opponentTiles = team == Team.Plant ? Tile.zombieTiles : Tile.plantTiles;
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                if (opponentTiles[1, col+i].planted != null) target[i+1] = opponentTiles[1, col+i].planted;
			    else if (opponentTiles[0, col+i].planted != null) target[i+1] = opponentTiles[0, col+i].planted;
			    if (target[i+1] == null)
			    {
				    if (team == Team.Plant) target[i+1] = GameManager.Instance.zombieHero;
				    else target[i+1] = GameManager.Instance.plantHero;
			    }
            }
            int[] dealt = new int[3];
            for (int i = 0; i < 3; i++) if (target[i] != null) dealt[i] = target[i].ReceiveDamage(atk, bullseye);
			// animation
			yield return new WaitForSeconds(1);
			//
            yield return GameManager.CallLeftToRight("OnCardAttack", this);
			for (int i = 0; i < 3; i++)
            {
                if (dealt[i] > 0)
			    {   
				    yield return GameManager.CallLeftToRight("OnCardHurt", target[i]);
			    }
            }
		}
	}

    public override int ReceiveDamage(int dmg, bool bullseye = false)
    {//Debug.Log(row + " " + col + " got hit for " + dmg);
        if (gravestone) return 0;
        dmg -= armor;
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
		if (dmg > 0) StartCoroutine(HitVisual());
		return dmg;
    }
    
    public IEnumerator DieIfZero()
    {
        if (HP <= 0)
        {
            yield return GameManager.CallLeftToRight("OnCardDeath", this);
            Destroy(gameObject);
        }
        yield return null;
    }

	public IEnumerator Destroy()
	{
		yield return GameManager.CallLeftToRight("OnCardDeath", this);
		Destroy(gameObject);
	}

	public override void Heal(int amount, bool raiseCap)
    {
        if (gravestone) return;
        HP += amount;
        if (raiseCap) maxHP += amount;
        else HP = Mathf.Min(maxHP, HP);
        hpUI.text = HP + "";
    }

	public void RaiseAttack(int amount)
	{
        if (gravestone) return;
		atk += amount;
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

    public IEnumerator Freeze()
    {
        frozen = true;
        SR.material.color = Color.blue;
        yield return GameManager.CallLeftToRight("OnCardFreeze", this);
    }

}
