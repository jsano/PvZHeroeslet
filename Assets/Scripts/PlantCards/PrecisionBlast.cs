using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionBlast : Card
{

	protected override IEnumerator OnThisPlay()
	{
        GameManager.Instance.DisableHandCards();
        yield return new WaitForSeconds(1);

        StartCoroutine(GetTargets(2)[0].ReceiveDamage(5, this));

        yield return base.OnThisPlay();
	}

}
