using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboozle : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return Glow();
			yield return GameManager.Instance.DrawCard(team, 2);
		}
		yield return base.OnThisPlay();
	}

}