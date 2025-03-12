using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawXCards : Card
{

    public int count;

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.DrawCard(count);
        yield return base.OnThisPlay();
    }

}
