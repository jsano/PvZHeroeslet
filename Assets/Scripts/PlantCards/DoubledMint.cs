using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubledMint : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		ChangeStats(atk, HP);
		yield return base.OnTurnStart();
	}

}
