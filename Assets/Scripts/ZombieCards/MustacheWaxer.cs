using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MustacheWaxer : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this)
		{
			if (played.tribes.Contains(Tribe.Mustache))
			{
                yield return new WaitForSeconds(1);
                ChangeStats(0, 2);
				GameManager.Instance.UpdateRemaining(1, team);
			}
		}
		yield return base.OnCardPlay(played);
	}

}
