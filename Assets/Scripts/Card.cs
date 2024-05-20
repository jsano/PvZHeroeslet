using System.Collections;
using System.Collections.Generic;
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
        transform.parent.BroadcastMessage("OnCardPlay", this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log(row + " " + col);
        if (!IsOwner) Tile.opponentTiles[row, col].Place(this);
        else Tile.tileObjects[row, col].Place(this);
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
        transform.parent.BroadcastMessage("OnCardAttack", this);
    }

    public void ReceiveDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0) transform.parent.BroadcastMessage("OnCardDeath", this);
    }

    public void Heal(int amount, bool raiseCap)
    {
        HP += amount;
        if (raiseCap) maxHP += amount;
        else HP = Mathf.Min(maxHP, HP);
    }

}
