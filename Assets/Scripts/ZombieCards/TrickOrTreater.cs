using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickOrTreater : Card
{

	private bool first = true;

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Trick && played.team == Team.Zombie && first)
		{
			first = false;
            yield return Glow();
            int card = UnityEngine.Random.Range(0, 2) == 0 ? AllCards.NameToID("Healthy Treat") : AllCards.NameToID("Sugary Treat");
			yield return GameManager.Instance.GainHandCard(team, card);
		}
		yield return base.OnCardPlay(played);
	}

    protected override IEnumerator OnTurnEnd()
    {
		first = true;
        yield return base.OnTurnEnd();
    }

}
