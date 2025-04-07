using System;
using Unity.Netcode;
using UnityEngine;

public class FinalStats : INetworkSerializable
{

    public int atk;
    public int hp;
    public string abilities;
    public int ID;
    public int cost;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref atk);
        serializer.SerializeValue(ref hp);
        serializer.SerializeValue(ref abilities);
        serializer.SerializeValue(ref ID);
        serializer.SerializeValue(ref cost);
    }

    public static FinalStats MakeDefaultFS(int id)
    {
        Card c = AllCards.Instance.cards[id];
        return new FinalStats()
        {
            hp = c.HP,
            atk = c.atk,
            abilities = "",
            ID = id,
            cost = c.cost,
        };
    }

}
