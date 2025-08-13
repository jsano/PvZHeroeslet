using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Card
{
	public int produceAmount;

	protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        yield return GameManager.Instance.UpdateRemaining(produceAmount, team);
		yield return base.OnTurnStart();
	}

}
