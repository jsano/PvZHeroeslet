using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroVera : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
		GameManager.Instance.GetTeamHero(team).ChangeStats(0, 10);
		yield return GameManager.Instance.GetTeamHero(team).Heal(10);
		yield return base.OnThisPlay();
	}

}
