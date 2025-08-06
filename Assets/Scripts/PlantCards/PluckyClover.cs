using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluckyClover : Card
{

	protected override IEnumerator OnThisPlay()
	{        
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.DrawCard(team);
        yield return SyncRandomChoiceAcrossNetwork(GameManager.Instance.GetHandCards()[0].orig.cost + "", "Plant");
		ChangeStats(int.Parse(GameManager.Instance.shuffledLists[^1][0]), 0);
		yield return base.OnThisPlay();
	}

}
