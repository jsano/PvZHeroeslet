using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Card
{
	public int produceAmount;

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.UpdateRemaining(produceAmount, team);
		yield return base.OnTurnStart();
	}

}
