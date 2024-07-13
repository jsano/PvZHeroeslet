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
        CallLeftToRight("OnCardPlay", null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CallLeftToRight(string methodName, Card arg) //probably ienumerator??
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
            if (first[1, i].planted != null) m.Invoke(first[1, i].planted, args);
            if (first[0, i].planted != null) m.Invoke(first[0, i].planted, args);

            if (second[1, i].planted != null) m.Invoke(second[1, i].planted, args);
            if (second[0, i].planted != null) m.Invoke(second[0, i].planted, args);
        }
    }

    /// <summary>
    /// Called whenever a card is played
    /// </summary>
    /// <param name="played"> The card that was played </param>
    protected void OnCardPlay(Card played)
    {
        Debug.Log(row + " " + col);
    }

    /// <summary>
    /// Called whenever a card on the field attacks something
    /// </summary>
    /// <param name="source"> The card that attacked </param>
    /// <param name="recipient"> The card that received damage </param>
    protected void OnCardAttack(Card source, Card recipient)
    {

    }

    /// <summary>
    /// Called whenever a card on the field dies
    /// </summary>
    /// <param name="died"> The card that died </param>
    protected void OnCardDeath(Card died)
    {

    }

    public void Attack()
    {
        Tile[,] target = Tile.tileObjects;
        if (GameManager.Instance.team == team) target = Tile.opponentTiles;
        Card target1;
        if (target[1, col].planted != null) target1 = target[1, col].planted;
        else if (target[0, col].planted != null) target1 = target[0, col].planted;
        else target1 = this; //temp
        target1.ReceiveDamage(atk);
        //play animation
        CallLeftToRight("OnCardAttack", target1);
    }

    private void ReceiveDamage(int dmg)
    {Debug.Log(gameObject.GetInstanceID() + " " + row + " " + col + " got hit for " + dmg);
        HP -= dmg;
        hpUI.text = Mathf.Max(0, HP) + "";
    }
    
    public void DieIf0()
    {
        if (HP <= 0)
        {
            CallLeftToRight("OnCardDeath", null);
            Destroy(gameObject);
        }
    }

    public void Heal(int amount, bool raiseCap)
    {
        HP += amount;
        if (raiseCap) maxHP += amount;
        else HP = Mathf.Min(maxHP, HP);
        hpUI.text = HP + "";
    }

}
