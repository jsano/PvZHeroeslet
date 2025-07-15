using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitPea : Card
{

	public override IEnumerator BeforeCombat()
	{
		yield return AttackFX(GameManager.Instance.plantHero);
		StartCoroutine(GameManager.Instance.plantHero.ReceiveDamage(1, this));
		yield return base.BeforeCombat();
	}

}
