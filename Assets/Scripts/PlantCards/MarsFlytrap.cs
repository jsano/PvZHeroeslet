using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsFlytrap : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == GameManager.Instance.zombieHero) 
		{
            yield return new WaitForSeconds(1);
			int amount = GameManager.Instance.zombieHero.StealBlock(1);
			GameManager.Instance.plantHero.StealBlock(-amount);
        }
		yield return base.OnCardHurt(hurt);
	}

}
