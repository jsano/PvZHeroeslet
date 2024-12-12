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
        StartCoroutine(CallLeftToRight("OnCardPlay", null));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CallLeftToRight(string methodName, Card arg)
    {
        MethodInfo m = typeof(Card).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        NetworkList<int> z = GameManager.Instance.zombies;
        NetworkList<int> p = GameManager.Instance.plants;
        Tile[,] first = Tile.tileObjects;
        Tile[,] second = Tile.opponentTiles;
        if (GameManager.Instance.team == Team.Plant)
        {
            first = Tile.opponentTiles;
            second = Tile.tileObjects;
        }

        Object[] args = methodName == "OnCardAttack" ? new[] { this, arg } : new[] { this };
        for (int i = 0; i < 5; i++)
        {
            if (first[1, i].planted != null) yield return StartCoroutine(methodName, first[1, i].planted);//, args);
            if (first[0, i].planted != null) yield return StartCoroutine(methodName, first[0, i].planted);

			if (second[1, i].planted != null) yield return StartCoroutine(methodName, second[1, i].planted);
			if (second[0, i].planted != null) yield return StartCoroutine(methodName, second[0, i].planted);
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
    /// <param name="recipient"> The card that received damage </param>
    protected virtual IEnumerator OnCardAttack(Card source)//, Card recipient)
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
        Card target1 = null;
        if (target[1, col].planted != null) target1 = target[1, col].planted;
        else if (target[0, col].planted != null) target1 = target[0, col].planted;
        if (target1 != null) target1.ReceiveDamage(atk);
        else
        {
            if (team == GameManager.Instance.team) GameManager.Instance.opponent.ReceiveDamage(atk);
            else GameManager.Instance.player.ReceiveDamage(atk);
        }
        int until = GameManager.Instance.playingAnimations;
        GameManager.Instance.playingAnimations += 1;
        // animation
        yield return new WaitForSeconds(1);
        GameManager.Instance.playingAnimations -= 1;
        //
        yield return new WaitUntil(() => GameManager.Instance.playingAnimations == until);
        yield return CallLeftToRight("OnCardAttack", target1);
    }

    private void ReceiveDamage(int dmg)
    {Debug.Log(row + " " + col + " got hit for " + dmg);
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
    }
    
    public IEnumerator DieIf0()
    {
        if (HP <= 0)
        {
            yield return CallLeftToRight("OnCardDeath", null);
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
