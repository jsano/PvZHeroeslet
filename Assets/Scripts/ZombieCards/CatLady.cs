using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLady : Card
{

	private int added;

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this)
		{
			if (played.tribes.Contains(Tribe.Pet))
			{
                yield return Glow();
                ChangeStats(3, 0);
				added += 3;
			}
		}
		yield return base.OnCardPlay(played);
	}

    protected override IEnumerator OnTurnEnd()
    {
		ChangeStats(-added, 0);
        return base.OnTurnEnd();
    }

}
