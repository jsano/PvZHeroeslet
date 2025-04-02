using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trickster : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		yield return Attack();
		yield return base.OnThisPlay();
	}

}
