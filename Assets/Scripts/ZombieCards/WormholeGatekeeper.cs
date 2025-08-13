using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeGatekeeper : Card
{

	protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        StartCoroutine(GameManager.Instance.DrawCard(Team.Plant));
		StartCoroutine(GameManager.Instance.DrawCard(Team.Zombie));
		yield return new WaitForSeconds(1);
		yield return base.OnTurnStart();
	}

}
