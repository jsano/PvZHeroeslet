using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawXCards : Card
{

    public int count;

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.DrawCard(team, count);
        yield return base.OnThisPlay();
    }

}
