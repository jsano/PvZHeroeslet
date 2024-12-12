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
        MegaGrow,
        Guardian
    }

    public enum Trait
    {
        Pea,
        Nut
    }

    public Team team;
    public Type type;
    public Class _class;
    public Trait[] traits;

    public int cost;
    public int atk;
    public int HP;
    private int maxHP;

    public int armor;
    public bool strikethrough;
    public bool deadly;
    public bool frenzy;
    public bool teamUp;
    public int overshoot;

    [HideInInspector] public int row;
    [HideInInspector] public int col;

    private TextMeshProUGUI atkUI;
    private TextMeshProUGUI hpUI;
    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        maxHP = HP;
        atkUI = transform.Find("ATK").GetComponent<TextMeshProUGUI>();
        atkUI.text = atk + "";
        hpUI = transform.Find("HP").GetComponent<TextMeshProUGUI>();
        hpUI.text = HP + "";
        //play animation
        StartCoroutine(GameManager.CallLeftToRight("OnCardPlay", this));
    }

    // Update is called once per frame
    void Update()
    {
        
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

    protected virtual IEnumerator OnTurnStart()
    {
        yield return null;
    }

    public IEnumerator Attack()
    {
        Tile[,] target = Tile.tileObjects;
        if (GameManager.Instance.team == team) target = Tile.opponentTiles;
        Damagable target1 = null;
        if (target[1, col].planted != null) target1 = target[1, col].planted;
        else if (target[0, col].planted != null) target1 = target[0, col].planted;
        if (target1 == null) 
        {
            if (team == GameManager.Instance.team) target1 = GameManager.Instance.opponent;
            else target1 = GameManager.Instance.player;
        }
		int dealt = target1.ReceiveDamage(atk);
		// animation
		yield return new WaitForSeconds(1);
        //
        if (dealt > 0)
        {
            yield return GameManager.CallLeftToRight("OnCardAttack", this);
		    yield return GameManager.CallLeftToRight("OnCardHurt", target1);
        }
	}

    public override int ReceiveDamage(int dmg)
    {//Debug.Log(row + " " + col + " got hit for " + dmg);
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
		if (dmg > 0) StartCoroutine(HitVisual());
		return dmg;
    }
    
    public IEnumerator DieIf0()
    {
        if (HP <= 0)
        {
            yield return GameManager.CallLeftToRight("OnCardDeath", this);
            Destroy(gameObject);
        }
        yield return null;
    }

    public void Heal(int amount, bool raiseCap)
    {
        HP += amount;
        if (raiseCap) maxHP += amount;
        else HP = Mathf.Min(maxHP, HP);
        hpUI.text = HP + "";
    }

	public void RaiseAttack(int amount)
	{
		atk += amount;
		atkUI.text = atk + "";
	}

	private IEnumerator HitVisual()
	{
		SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
		yield return new WaitForSeconds(0.1f);
		SR.material.color = Color.white;
	}

}
