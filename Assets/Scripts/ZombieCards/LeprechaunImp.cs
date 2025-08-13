using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeprechaunImp : Card
{

	protected override IEnumerator OnThisPlay()
	{
		//yield return new WaitForSeconds(1);
		int id = AllCards.NameToID("Pot of Gold");
		GameManager.Instance.ShuffleIntoDeck(team, new() { id });
		yield return base.OnThisPlay();
	}

}
