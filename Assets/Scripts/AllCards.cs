using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Card;

public class AllCards : MonoBehaviour
{

    private static AllCards instance;
    public static AllCards Instance { get { return instance; } }

    public Card[] cards;
    public Hero[] heroes;
    public Buff[] buffs;
    
    public Sprite attackSprite;
    public Sprite HPSprite;
    public Sprite multiSprite;
    public Sprite gravestoneSprite;
    public Sprite antiheroSprite;
    public Sprite armorSprite;
    public Sprite bullseyeSprite;
    public Sprite deadlySprite;
    public Sprite doubleStrikeSprite;
    public Sprite frenzySprite;
    public Sprite overshootSprite;
    public Sprite strikethroughSprite;
    public Sprite untrickableSprite;
    public Sprite frozenSprite;
    public Sprite invulnerableSprite;
    public Sprite strengthHeartSprite;

    public Sprite sunUI;
    public Sprite brainUI;
    public Sprite plantCardBack;
    public Sprite zombieCardBack;

    public GameObject attackFX;

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public static int RandomFromTribe(ITuple tribe, bool forceUnit=false, bool forceAmphibious=false, Team team = Team.A)
    {
        List<int> possible = new();
        for (int i = 0; i < Instance.cards.Length; i++)
        {
			for (int j = 0; j < tribe.Length; j++)
			{
                if (Instance.cards[i].tribes.Contains((Tribe) tribe[j]) && (!forceUnit || Instance.cards[i].type == Type.Unit) && (!forceAmphibious || Instance.cards[i].amphibious))
                {
                    if ((Tribe)tribe[j] == Tribe.Superpower && team != Instance.cards[i].team) break;
                    possible.Add(i);
                    break;
                }
			}
        }
        return possible[Random.Range(0, possible.Count)];
    }

	public static int RandomFromCost(Team team, ITuple cost, bool forceUnit=false, bool forceAmphibious = false)
	{
		List<int> possible = new();
		for (int i = 0; i < Instance.cards.Length; i++)
		{
			for (int j = 0; j < cost.Length; j++)
			{
				if (Instance.cards[i].team == team && Instance.cards[i].cost == (int)cost[j] && (!forceUnit || Instance.cards[i].type == Type.Unit) && (!forceAmphibious || Instance.cards[i].amphibious))
				{
					possible.Add(i);
					break;
				}
			}
		}
		return possible[Random.Range(0, possible.Count)];
	}

    public static int RandomTribeOfCost(Tribe tribe, int cost, bool forceUnit=false)
    {
        List<int> possible = new();
        for (int i = 0; i < Instance.cards.Length; i++)
        {
            if (Instance.cards[i].tribes.Contains(tribe) && Instance.cards[i].cost == cost && (!forceUnit || Instance.cards[i].type == Type.Unit))
            {
                possible.Add(i);
            }
        }
        return possible[Random.Range(0, possible.Count)];
    }

    public static int RandomTrick(Team team)
	{
        List<int> possible = new();
        for (int i = 0; i < Instance.cards.Length; i++)
        {
            if (Instance.cards[i].team == team && Instance.cards[i].type == Type.Trick)
            {
                possible.Add(i);
            }
        }
        return possible[Random.Range(0, possible.Count)];
    }

	public static int NameToID(string name)
	{
		for (int i = 0; i < Instance.cards.Length; i++)
		{
			if (Instance.cards[i].name == name)
			{
				return i;
			}
		}
        for (int i = 0; i < Instance.buffs.Length; i++)
        {
            if (Instance.buffs[i].name == name)
            {
                return i;
            }
        }
        return -1;
	}

    public static Card InstanceToPrefab(Card instance)
    {
        if (instance.name.IndexOf("(") >= 0)
            foreach (Card c in Instance.cards)
            {
                if (instance.name.Substring(0, instance.name.IndexOf("(")) == c.name)
                {
                    return c;
                }
            }
        return instance;
    }

    public static Buff InstanceToPrefab(Buff instance)
    {
        if (instance.name.IndexOf("(") >= 0)
            foreach (Buff c in Instance.buffs)
            {
                if (instance.name.Substring(0, instance.name.IndexOf("(")) == c.name)
                {
                    return c;
                }
            }
        return instance;
    }

}
