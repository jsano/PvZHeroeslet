using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingImp : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		yield return new WaitForSeconds(1);
        yield return ReceiveDamage(1, this);
        yield return base.OnTurnEnd();
    }

}
