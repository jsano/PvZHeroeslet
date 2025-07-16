using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdOfParadise : Card
{

	protected override IEnumerator OnTurnStart()
	{
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Superpower, Tribe.Superpower)));
        yield return base.OnTurnStart();
	}

}
