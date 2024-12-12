using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;

public class Card : NetworkBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;
        atkUI = transform.Find("ATK").GetComponent<TextMeshProUGUI>();
        atkUI.text = atk + "";
        hpUI = transform.Find("HP").GetComponent<TextMeshProUGUI>();
        hpUI.text = HP + "";
        //play animation
        StartCoroutine(CallLeftToRight("OnCardPlay", this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CallLeftToRight(string methodName, Card arg)
    {
        MethodInfo m = typeof(Card).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        Tile[,] first = Tile.tileObjects;
        Tile[,] second = Tile.opponentTiles;
        if (GameManager.Instance.team == Team.Plant)
        {
            first = Tile.opponentTiles;
            second = Tile.tileObjects;
        }

        for (int i = 0; i < 5; i++)
        {
            if (first[1, i].planted != null) yield return first[1, i].planted.StartCoroutine(methodName, arg);
            if (first[0, i].planted != null) yield return first[0, i].planted.StartCoroutine(methodName, arg);

			if (second[1, i].planted != null) yield return second[1, i].planted.StartCoroutine(methodName, arg);
			if (second[0, i].planted != null) yield return second[0, i].planted.StartCoroutine(methodName, arg);
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
	protected virtual IEnumerator OnCardHurt(Card hurt)
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

    public IEnumerator Attack()
    {
        Tile[,] target = Tile.tileObjects;
        if (GameManager.Instance.team == team) target = Tile.opponentTiles;
        int dealt = 0;
        Card target1 = null;
        if (target[1, col].planted != null) target1 = target[1, col].planted;
        else if (target[0, col].planted != null) target1 = target[0, col].planted;
        if (target1 != null) dealt = target1.ReceiveDamage(atk);
        else
        {
            if (team == GameManager.Instance.team) dealt = GameManager.Instance.opponent.ReceiveDamage(atk);
            else dealt = GameManager.Instance.player.ReceiveDamage(atk);
        }
        // animation
        yield return new WaitForSeconds(1);
        //
        if (dealt > 0)
        {
            yield return CallLeftToRight("OnCardAttack", this);
		    yield return CallLeftToRight("OnCardHurt", target1);
        }
	}

    private int ReceiveDamage(int dmg)
    {//Debug.Log(row + " " + col + " got hit for " + dmg);
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
        return dmg;
    }
    
    public IEnumerator DieIf0()
    {
        if (HP <= 0)
        {
            yield return CallLeftToRight("OnCardDeath", this);
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

}
