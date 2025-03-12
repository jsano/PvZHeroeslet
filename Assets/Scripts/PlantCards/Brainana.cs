using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brainana : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		yield return new WaitForSeconds(1);
		GameManager.Instance.UpdateRemaining(-100, Team.Zombie);
		yield return base.OnThisPlay();
	}

}
