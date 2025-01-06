using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCards : MonoBehaviour
{

    private static AllCards instance;
    public static AllCards Instance { get { return instance; } }

    public Card[] cards;
	public Sprite gravestoneSprite;

	void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public static int RandomFromTribe(params Card.Tribe[] tribe)
    {
        List<int> possible = new();
        for (int i = 0; i < Instance.cards.Length; i++)
        {
			foreach (Card.Tribe t in tribe)
			{
                if (Instance.cards[i].tribes.Contains(t))
                {
                    possible.Add(i);
                    break;
                }
			}
        }
        return possible[Random.Range(0, possible.Count)];
    }

}
