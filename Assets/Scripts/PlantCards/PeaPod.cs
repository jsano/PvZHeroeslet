using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaPod : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		ChangeStats(1, 1);
	}

}
