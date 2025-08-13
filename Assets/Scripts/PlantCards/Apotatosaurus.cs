using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apotatosaurus : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Root, Tribe.Root)));
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardDraw(Team team)
    {
        if (team == this.team)
        {
            yield return new WaitForSeconds(1);
            ChangeStats(1, 1);
        }
        yield return base.OnCardDraw(team);
    }

}
