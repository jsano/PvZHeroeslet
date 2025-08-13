using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarchLord : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played != this && played.tribes.Contains(Tribe.Root))
        {
            yield return Glow();
            ChangeStats(1, 1);
            played.ChangeStats(1, 1);
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Root, Tribe.Root)));
		yield return base.OnTurnStart();
	}

}
