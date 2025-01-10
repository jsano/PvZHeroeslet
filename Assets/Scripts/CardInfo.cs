using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInfo : MonoBehaviour
{

	public void Show(Card source)
	{
		transform.parent.gameObject.SetActive(true);
		Card baseCard = source;
		foreach (Card c in AllCards.Instance.cards)
		{
			if (c.gameObject.name == source.name.Substring(0, source.gameObject.name.IndexOf("(Clone)")))
			{
				baseCard = c;
				break;
			}
		}
		Debug.Log(baseCard.description);
	}

}
