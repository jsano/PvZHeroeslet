using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitPea : Card
{

	public override IEnumerator BeforeCombat()
	{
		yield return AttackFX(GameManager.Instance.GetTeamHero(team));
		StartCoroutine(GameManager.Instance.GetTeamHero(team).ReceiveDamage(1, this));
		yield return base.BeforeCombat();
	}

}
