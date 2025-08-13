using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brainana : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		yield return GameManager.Instance.UpdateRemaining(-100, Team.Zombie);
		yield return base.OnThisPlay();
	}

}
