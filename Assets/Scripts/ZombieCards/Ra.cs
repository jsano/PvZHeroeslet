using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ra : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.UpdateRemaining(-2, Team.Plant);
		yield return base.OnThisPlay();
	}

}
