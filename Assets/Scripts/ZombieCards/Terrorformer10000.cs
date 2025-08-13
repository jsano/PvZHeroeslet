using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrorformer10000 : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomTrick(team));
        if (GameManager.Instance.team == Team.Zombie) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (AllCards.Instance.cards[hc.ID].type == Type.Trick) hc.ChangeCost(-1);
        yield return base.OnThisPlay();
    }

}
