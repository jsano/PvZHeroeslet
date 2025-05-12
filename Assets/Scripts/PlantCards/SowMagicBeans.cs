using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowMagicBeans : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		int id = AllCards.NameToID("Magic Beanstalk");
		GameManager.Instance.ShuffleIntoDeck(team, new() { id, id, id, id, id });
		yield return base.OnThisPlay();
	}

}
