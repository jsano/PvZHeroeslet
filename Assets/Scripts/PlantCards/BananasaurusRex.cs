using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananasaurusRex : Card
{

	protected override IEnumerator OnCardDraw()
	{
		yield return new WaitForSeconds(1);
		RaiseAttack(1);
		Heal(1, true);
	}

}
