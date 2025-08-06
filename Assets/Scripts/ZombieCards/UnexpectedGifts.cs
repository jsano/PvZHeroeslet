using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnexpectedGifts : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.DrawCard(team, 3);
        yield return GameManager.Instance.DrawCard(Team.Plant, 1);
        yield return base.OnThisPlay();
    }

}
