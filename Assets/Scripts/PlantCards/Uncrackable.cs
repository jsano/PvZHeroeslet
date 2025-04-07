using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uncrackable : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        GameManager.Instance.plantHero.ToggleInvulnerability(true);
        GameManager.Instance.DrawCard(team);

        yield return base.OnThisPlay();
	}

}
