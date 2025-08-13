using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroVera : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
		GameManager.Instance.plantHero.ChangeStats(0, 10);
		yield return GameManager.Instance.plantHero.Heal(10);
		yield return base.OnThisPlay();
	}

}
