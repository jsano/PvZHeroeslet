using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonbean : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this)
		{
            yield return Glow();
            int id = AllCards.NameToID("Magic Beanstalk");
            GameManager.Instance.ShuffleIntoDeck(team, new() { id, id });
        }
		yield return base.OnCardHurt(hurt);
	}

}
