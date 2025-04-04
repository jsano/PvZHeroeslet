using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotSharktronicSub : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).team == Team.Plant)
		{
			GameManager.Instance.DisableHandCards();
			yield return new WaitForSeconds(1);
			((Card)hurt.Item1).Destroy();
		}
		yield return base.OnCardHurt(hurt);
	}

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died.team == Team.Plant)
        {
            GameManager.Instance.DisableHandCards();
            yield return new WaitForSeconds(1);
            RaiseAttack(1);
        }
        yield return base.OnCardDeath(died);
    }

}
