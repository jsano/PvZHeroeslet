using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseCutter : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1 == GameManager.Instance.plantHero) 
		{
            yield return Glow();
            int id = AllCards.RandomFromTribe((Tribe.Gourmet, Tribe.Gourmet));
            FinalStats fs = new(id);
            fs.cost -= 1;
            yield return GameManager.Instance.GainHandCard(team, id, fs);
        }
		yield return base.OnCardHurt(hurt);
	}

}
