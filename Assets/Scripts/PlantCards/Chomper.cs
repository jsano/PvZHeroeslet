using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Damagable target = GetTargets(col)[0];
		if (target.GetComponent<Card>() != null && !target.GetComponent<Card>().gravestone && target.GetComponent<Card>().atk <= 3)
		{
			yield return Glow();
            target.GetComponent<Card>().Destroy();
		}
		yield return base.OnThisPlay();
    }

}
