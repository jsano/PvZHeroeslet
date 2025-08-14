using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutronImp : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Terrain)
		{
			yield return Glow();
			yield return BonusAttack();
		}
		yield return base.OnCardPlay(played);
	}

}
