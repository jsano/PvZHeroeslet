using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeGatekeeper : Card
{

	protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        StartCoroutine(GameManager.Instance.DrawCard(Team.A));
		StartCoroutine(GameManager.Instance.DrawCard(Team.B));
		yield return new WaitForSeconds(1);
		yield return base.OnTurnStart();
	}

}
