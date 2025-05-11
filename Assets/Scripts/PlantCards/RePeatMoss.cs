using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePeatMoss : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Trick && played.team == Team.Plant) yield return Attack();
		yield return base.OnCardPlay(played);
	}

}
