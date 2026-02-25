using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toadstool : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Damagable target = GetTargets(col)[0];
		if (target.GetComponent<Card>() != null && !target.GetComponent<Card>().gravestone && target.GetComponent<Card>().atk <= 4)
		{
			yield return Glow();
            target.GetComponent<Card>().Destroy();
		}
		yield return base.OnThisPlay();
    }

    protected override IEnumerator OnTurnStart()
    {
        yield return Glow();
        yield return GameManager.Instance.UpdateRemaining(1, team);
        yield return base.OnTurnStart();
    }

}
