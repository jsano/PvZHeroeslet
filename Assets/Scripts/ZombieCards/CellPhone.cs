using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhone : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

}
