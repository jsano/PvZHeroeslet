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
    public Sprite gravestoneSprite;

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
