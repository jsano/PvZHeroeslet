using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOfTheGrapes : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == GameManager.Instance.GetTeamHero(GetOpponent(team)) && hurt.Item2.col == col && hurt.Item2.team == team && hurt.Item2.type == Type.Unit) 
		{
            yield return Glow();
            yield return GameManager.Instance.DrawCard(team);
        }
		yield return base.OnCardHurt(hurt);
	}

}
