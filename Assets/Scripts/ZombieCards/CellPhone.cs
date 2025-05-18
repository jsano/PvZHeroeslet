using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhone : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

}
