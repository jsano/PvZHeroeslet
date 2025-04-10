using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightspeedSeed : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 2; i++) GameManager.Instance.GainHandCard(team, AllCards.RandomTrick(team));
        yield return base.OnThisPlay();
    }

}
