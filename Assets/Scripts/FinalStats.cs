using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// A wrapper for holding information about the stats a unit should have when planted. This is what HandCards use as its card's stats,
/// and what the network receives when either side plays a unit. This allows cards and HandCards to have different stats during initialization without
/// being restricted to the default prefab values: simply change its fields as needed
/// </summary>
public class FinalStats : INetworkSerializable
{

    public int atk;
    public int hp;
    /// <summary>
    /// Any <b>additional</b> abilities that a card has. Should be stored in a " - " separated list (since that's the only way to serialize it...)
    /// </summary>
    public string abilities;
    public int ID;
    public int cost;

    /// <summary>
    /// If true, the edits persist even when the card is bounced after playing
    /// </summary>
    public bool permanent;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref atk);
        serializer.SerializeValue(ref hp);
        serializer.SerializeValue(ref abilities);
        serializer.SerializeValue(ref ID);
        serializer.SerializeValue(ref cost);
        serializer.SerializeValue(ref permanent);
    }

    /// <summary>
    /// Creates a FinalStats instance with the default prefab values for the given card ID
    /// </summary>
    public FinalStats(int id, bool permanent = false)
    {
        Card c = AllCards.Instance.cards[id];
        hp = c.HP;
        atk = c.atk;
        abilities = "";
        ID = id;
        cost = c.cost;

        this.permanent = permanent;
    }

    public FinalStats()
    {

    }

}
