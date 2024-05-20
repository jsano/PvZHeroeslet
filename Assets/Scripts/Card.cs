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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (team != GameManager.Instance.team) Tile.opponentTiles[row, col].Place(this);
        else Tile.tileObjects[row, col].Place(this);
    }

    private void CallLeftToRight(string methodName)
    {
        MethodInfo m = typeof(Card).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < 5; i++)
        {
            if (Tile.tileObjects[1, i].planted != null) m.Invoke(Tile.tileObjects[1, i].planted, new[] { this });
            if (Tile.tileObjects[0, i].planted != null) m.Invoke(Tile.tileObjects[0, i].planted, new[] { this });

            if (Tile.opponentTiles[1, i].planted != null) m.Invoke(Tile.opponentTiles[1, i].planted, new[] { this });
            if (Tile.opponentTiles[0, i].planted != null) m.Invoke(Tile.opponentTiles[0, i].planted, new[] { this });
        }
    }

    /// <summary>
    /// Called whenever a card is played
    /// </summary>
    /// <param name="played"> The card that was played </param>
    protected void OnCardPlay(Card played)
    {
        
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
