using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurseGargantuar : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2.tribes.Contains(Tribe.Gargantuar))
		{
			GameManager.Instance.DisableHandCards();
			yield return new WaitForSeconds(1);
			GameManager.Instance.zombieHero.Heal(hurt.Item3);
		}
		yield return base.OnCardHurt(hurt);
	}

}
