using System;
using System.Reflection;
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

    [HideInInspector] public Card.Team team;

    /// <summary>
    /// Called the instant a HandCard is dropped onto the board.
    /// </summary>
    /// <param name="played"> [The team that played, the ID of the card that was played] </param>
    protected virtual void OnHandCardPlayImmediate(Tuple<Card.Team, int> played)
    {

    }

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

    public static List<object> CallAllImmediate(string name, object arg)
    {
        List<object> result = new ();
        foreach (Transform t in GameManager.Instance.playerBuffs)
        {
            result.Add(t.GetComponent<Buff>().GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(t.GetComponent<Buff>(), new object[] { arg }));
        }
        foreach (Transform t in GameManager.Instance.opponentBuffs)
        {
            result.Add(t.GetComponent<Buff>().GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(t.GetComponent<Buff>(), new object[] { arg }));
        }
        return result;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowBuffInfo);
    }

    public static bool PlayerHasBuff(string name, Card.Team team)
    {
        Transform search = GameManager.Instance.team == team ? GameManager.Instance.playerBuffs : GameManager.Instance.opponentBuffs;
        foreach (Transform t in search) if (AllCards.InstanceToPrefab(t.GetComponent<Buff>()).name == name) return true;
        return false;
    }

    public Sprite GetImage()
    {
        return transform.Find("BG/Mask/Image").GetComponent<Image>().sprite;
    }

    public void ShowBuffInfo()
    {
        BuffInfo.Instance.Show(this);
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
    /// Called whenever a card on the field is hurt
    /// </summary>
    /// <param name="hurt"> [The card that received damage, the card that dealt the damage, the final amount dealt, hero column relative to other simultaneous calls] </param>
    protected virtual IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card on the field dies
	/// </summary>
	/// <param name="died"> [The card that died, the card that destroyed it] </param>
    protected virtual IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field moves
    /// </summary>
    /// <param name="moved"> The card that moved </param>
    protected virtual IEnumerator OnCardMoved(Card moved)
    {
        yield return null;
    }

    /// <summary>
    /// Called whenever a card on the field gets frozen
    /// </summary>
    /// <param name="frozen"> The card that froze </param>
    protected virtual IEnumerator OnCardFreeze(Card frozen)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card on the field gets healed. NOT called when a card's max HP is raised
	/// </summary>
	/// <param name="healed"> The card that got healed </param>
	protected virtual IEnumerator OnCardHeal(Tuple<Card, int> healed)
    {
        yield return null;
    }

    /// <summary>
	/// Identical to OnCardHeal, but for heroes. Called even when max HP is raised
	/// </summary>
	protected virtual IEnumerator OnHeroHeal(Tuple<Hero, int> healed)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card changes attack or max HP. Not called when healed
	/// </summary>
	protected virtual IEnumerator OnCardStatsChanged(Tuple<Card, int, int> changed)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card does a bonus attack
	/// </summary>
	protected virtual IEnumerator OnCardBonusAttack(Card attacked)
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card is bounced
	/// </summary>
	protected virtual IEnumerator OnCardBounce(Card bounced)
    {
        yield return null;
    }

    /// <summary>
	/// Called at the start of turn
	/// </summary>
	protected virtual IEnumerator OnTurnStart()
    {
        yield return null;
    }

    /// <summary>
	/// Called at the end of turn
	/// </summary>
    protected virtual IEnumerator OnTurnEnd()
    {
        yield return null;
    }

    /// <summary>
	/// Called whenever a card gets drawn by a player
	/// </summary>
    /// <param name="team">The team that drew the card</param>
    protected virtual IEnumerator OnCardDraw(Card.Team team)
    {
        yield return null;
    }

}
