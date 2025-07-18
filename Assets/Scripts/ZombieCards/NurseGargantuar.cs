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
			yield return new WaitForSeconds(1);
			yield return GameManager.Instance.zombieHero.Heal(hurt.Item3);
		}
		yield return base.OnCardHurt(hurt);
	}

}
