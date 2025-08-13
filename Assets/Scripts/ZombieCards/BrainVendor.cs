using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainVendor : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		yield return GameManager.Instance.UpdateRemaining(3, team);
		yield return base.OnThisPlay();
	}

}
