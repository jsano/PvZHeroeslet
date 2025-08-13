using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGourd : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        GameManager.Instance.plantHero.StealBlock(-10);
		yield return base.OnThisPlay();
	}

}
