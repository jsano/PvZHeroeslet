using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotSharktronicSub : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).team == Team.Plant)
		{
			yield return new WaitForSeconds(1);
			((Card)hurt.Item1).Destroy();
		}
		yield return base.OnCardHurt(hurt);
	}

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Team.Plant)
        {
            yield return new WaitForSeconds(1);
            ChangeStats(1, 0);
        }
        yield return base.OnCardDeath(died);
    }

}
