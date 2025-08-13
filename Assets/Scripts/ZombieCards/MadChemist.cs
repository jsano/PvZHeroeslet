using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadChemist : Card
{

	private bool first = true;

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Trick && played.team == Team.Zombie && first)
		{
			first = false;
			yield return Glow();
			int card = AllCards.RandomTrick(team);
			FinalStats fs = new(card);
			fs.cost -= 1;
			yield return GameManager.Instance.GainHandCard(team, card, fs);
		}
		yield return base.OnCardPlay(played);
	}

    protected override IEnumerator OnTurnEnd()
    {
		first = true;
        yield return base.OnTurnEnd();
    }

}
