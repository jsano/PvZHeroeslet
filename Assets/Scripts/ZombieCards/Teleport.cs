using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Card
{

    public int count;

    protected override IEnumerator OnThisPlay()
    {
        GameManager.Instance.allowZombieCards = true;
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.DrawCard(team);
        yield return base.OnThisPlay();
    }

}
