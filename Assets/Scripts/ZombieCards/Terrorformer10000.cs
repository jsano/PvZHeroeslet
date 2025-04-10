using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrorformer10000 : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.GainHandCard(team, AllCards.RandomTrick(team));
        yield return null;
        foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (AllCards.Instance.cards[hc.ID].type == Type.Trick) hc.ChangeCost(-1);
        yield return base.OnThisPlay();
    }

}
