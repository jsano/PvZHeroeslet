using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;
        //play animation
        CallLeftToRight("OnCardPlay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CallLeftToRight(string methodName)
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
        for (int i = 0; i < 5; i++)
        {
            if (z[1 + 2*i] != -1) m.Invoke(first[1, i].planted, new[] { this });
            if (z[2*i] != -1) m.Invoke(first[0, i].planted, new[] { this });

            if (p[1 + 2*i] != -1) m.Invoke(second[1, i].planted, new[] { this });
            if (p[2*i] != -1) m.Invoke(second[0, i].planted, new[] { this });
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
        if (died == this) Destroy(gameObject);
    }

    public void Attack()
    {
        //attack opponent card in col
        CallLeftToRight("OnCardAttack");
    }

    public void ReceiveDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0) CallLeftToRight("OnCardDeath");
    }

    public void Heal(int amount, bool raiseCap)
    {
        HP += amount;
        if (raiseCap) maxHP += amount;
        else HP = Mathf.Min(maxHP, HP);
    }

}
