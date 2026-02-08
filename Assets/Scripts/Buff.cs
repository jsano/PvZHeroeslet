using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{

    public enum Rarity { 
        Common,
        Rare,
        Legendary,
        Duo
    }

    public Rarity rarity;
    public string description;
    public Card.Class buffClass;
    public string lore;

    public Image image;

    /// <summary>
    /// Called the instant a card is played.
    /// </summary>
    /// <param name="played"> The card that was played </param>
    protected virtual void OnCardPlayImmediate(Card played)
    {

    }

    /// <summary>
    /// Called the instant a card is played.
    /// </summary>
    /// <param name="hurt"> [The card that received damage, the card that dealt the damage, the final amount dealt] </param>
    protected virtual int OnCardHurtImmediate(Tuple<Damagable, Card, int> hurt)
    {
        return 0;
    }

    public static List<object> CallAll(string name, object arg)
    {
        List<object> result = new ();
        foreach (Transform t in GameManager.Instance.playerBuffs)
        {
            result.Add(t.GetComponent<Buff>().GetType().GetMethod(name).Invoke(t.GetComponent<Buff>(), new object[] { arg }));
        }
        foreach (Transform t in GameManager.Instance.opponentBuffs)
        {
            result.Add(t.GetComponent<Buff>().GetType().GetMethod(name).Invoke(t.GetComponent<Buff>(), new object[] { arg }));
        }
        return result;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ShowBuffInfo()
    {
        BuffInfo.Instance.Show(this);
    }

}
