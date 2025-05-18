using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Card
{

	protected override IEnumerator OnThisPlay()
	{
		ToggleInvulnerability(true);
        yield return base.OnThisPlay();
	}

}
