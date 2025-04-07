using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.UpdateRemaining(1, team);
		yield return base.OnTurnStart();
	}

}
