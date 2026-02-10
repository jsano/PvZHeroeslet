using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsFlytrap : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == GameManager.Instance.GetTeamHero(GetOpponent(team))) 
		{
            yield return Glow();
			int amount = GameManager.Instance.GetTeamHero(GetOpponent(team)).StealBlock(1);
			GameManager.Instance.GetTeamHero(team).StealBlock(-amount);
        }
		yield return base.OnCardHurt(hurt);
	}

}
