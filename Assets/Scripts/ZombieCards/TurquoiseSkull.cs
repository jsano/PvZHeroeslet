using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurquoiseSkull : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		yield return GameManager.Instance.UpdateRemaining(-1, Team.Plant);
		ChangeStats(1, 1);
		yield return base.OnTurnStart();
	}

}
