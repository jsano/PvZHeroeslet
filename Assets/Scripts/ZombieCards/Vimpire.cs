using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vimpire : Card
{

    private Card attacked;

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).team == Team.Plant && hurt.Item2 == this)
		{
            attacked = (Card)hurt.Item1;
		}
		yield return base.OnCardHurt(hurt);
	}

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died == attacked)
        {
            yield return new WaitForSeconds(1);
            ChangeStats(2, 2);
        }
        yield return base.OnCardDeath(died);
    }

}
