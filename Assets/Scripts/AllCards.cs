using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AllCards : MonoBehaviour
{

    private static AllCards instance;
    public static AllCards Instance { get { return instance; } }

    public Card[] cards;
    public Hero[] heroes;
    
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
    public Sprite strengthHeart;

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

    public static int RandomFromTribe(ITuple tribe, bool forceUnit=false, bool forceAmphibious=false)
    {
        List<int> possible = new();
        for (int i = 0; i < Instance.cards.Length; i++)
        {
			for (int j = 0; j < tribe.Length; j++)
			{
                if (Instance.cards[i].tribes.Contains((Card.Tribe) tribe[j]) && (!forceUnit || Instance.cards[i].type == Card.Type.Unit) && (!forceAmphibious || Instance.cards[i].amphibious))
                {
                    possible.Add(i);
                    break;
                }
			}
        }
        return possible[Random.Range(0, possible.Count)];
    }

	public static int RandomFromCost(Card.Team team, ITuple cost, bool forceUnit=false, bool forceAmphibious = false)
	{
		List<int> possible = new();
		for (int i = 0; i < Instance.cards.Length; i++)
		{
			for (int j = 0; j < cost.Length; j++)
			{
				if (Instance.cards[i].team == team && Instance.cards[i].cost == (int)cost[j] && (!forceUnit || Instance.cards[i].type == Card.Type.Unit) && (!forceAmphibious || Instance.cards[i].amphibious))
				{
					possible.Add(i);
					break;
				}
			}
		}
		return possible[Random.Range(0, possible.Count)];
	}

	public static int RandomTrick(Card.Team team)
	{
        List<int> possible = new();
        for (int i = 0; i < Instance.cards.Length; i++)
        {
            if (Instance.cards[i].team == team && Instance.cards[i].type == Card.Type.Trick)
            {
                possible.Add(i);
                break;
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
		return -1;
	}

}
