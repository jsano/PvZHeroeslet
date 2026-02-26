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

    public static Dictionary<Card.Class, Color> classColors = new()
    {
        { Card.Class.Elation, Color.yellow },
        { Card.Class.Misery, Color.blue + Color.yellow * 0.1f },
        { Card.Class.Wrath, Color.red },
        { Card.Class.Fright, Color.magenta + Color.blue * 0.5f },
        { Card.Class.Awe, Color.red + Color.cyan * 0.5f },
        { Card.Class.Contempt, Color.green }
    };

    public Rarity rarity;
    public string description;
    public Card.Class buffClass;
    public Card.Class duoSecondClass;
    public string lore;

    [HideInInspector] public Card.Team team;

    /// <summary>
    /// Called the instant a Buff is gained.
    /// </summary>
    /// <param name="gained"> The buff that was gained </param>
    protected virtual void OnBuffGainedImmediate(Buff gained)
    {
        
    }

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
    /// Called the instant a card is hurt to modify how much damage will be dealt.
    /// </summary>
    /// <param name="hurt"> [The card that received damage, the card that dealt the damage, the final amount dealt] </param>
    protected virtual int CardHurtModifiers(Tuple<Damagable, Card, int> hurt)
    {
        return 0;
    }

    /// <summary>
    /// Called the instant a card is hurt.
    /// </summary>
    /// <param name="hurt"> [The card that received damage, the card that dealt the damage, the final amount dealt] </param>
    protected virtual int OnCardHurtImmediate(Tuple<Damagable, Card, int> hurt)
    {
        return 0;
    }

    /// <summary>
    /// Called the instant a card is healed.
    /// </summary>
    /// <param name="healed"> [The card that got healed, the initial amount to heal] </param>
    protected virtual int OnCardHealImmediate(Tuple<Card, int> healed)
    {
        return 0;
    }

    /// <summary>
    /// Called the instant a hero is healed.
    /// </summary>
    /// <param name="healed"> [The hero that got healed, the initial amount to heal] </param>
    protected virtual int OnHeroHealImmediate(Tuple<Hero, int> healed)
    {
        return 0;
    }

    protected virtual void OnBlock(Hero hero)
    {

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
        transform.Find("BG/Mask/Image").GetComponent<Image>().color = Color.Lerp(classColors[buffClass], Color.white, 0.5f);

        GetComponent<Button>().onClick.AddListener(ShowBuffInfo);
        CallAllImmediate("OnBuffGainedImmediate", this);
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
	/// Called after OnTurnEnd but before OnTurnStart, useful for buffs that shouldn't interact with those methods 
	/// </summary>
    protected virtual void AfterTurnEndBeforeTurnStart(object arg)
    {
        
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
