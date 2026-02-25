using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilBuddy : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        yield return GameManager.Instance.GetTeamHero(team).Heal(2);
		yield return base.OnThisPlay();
	}

}
