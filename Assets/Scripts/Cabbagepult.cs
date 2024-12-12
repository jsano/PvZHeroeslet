using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabbagepult : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played == this)
		{
			if (col == 0)
			{
				yield return new WaitForSeconds(1);
				Heal(1, true);
				RaiseAttack(1);
			}
		}
		yield return null;
	}

}
