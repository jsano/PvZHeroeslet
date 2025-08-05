using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluckyClover : Card
{

	protected override IEnumerator OnThisPlay()
	{        
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.DrawCard(team);
		if (GameManager.Instance.team == team)
		{
			GameManager.Instance.StoreRpc(GameManager.Instance.GetHandCards()[0].orig.cost + "");
		}
		yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);
		ChangeStats(int.Parse(GameManager.Instance.shuffledList[0]), 0);
		yield return base.OnThisPlay();
	}

}
