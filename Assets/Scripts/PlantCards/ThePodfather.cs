using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePodfather : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Pea))
		{
            yield return Glow();
            played.ChangeStats(2, 2);
		}
		yield return base.OnCardPlay(played);
	}

}
