using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayflower : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == GameManager.Instance.zombieHero && hurt.Item2 == this) 
		{
            yield return new WaitForSeconds(1);
            yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Corn, Tribe.Squash, Tribe.Bean)));
        }
		yield return base.OnCardHurt(hurt);
	}

}
