using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyThyme : Card
{

	protected override IEnumerator OnCardBonusAttack(Card attacked)
	{
		yield return Glow();
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnCardBonusAttack(attacked);
    }

}
