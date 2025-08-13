using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargantuarMime : Card
{

	protected override IEnumerator OnCardBonusAttack(Card attacked)
	{
		if (!attacked.tribes.Contains(Tribe.Mime))
		{
            yield return Glow();
            yield return BonusAttack();
			yield return base.OnCardBonusAttack(attacked);
		}
    }

}
