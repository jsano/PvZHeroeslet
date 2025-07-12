using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaLauncher : Card
{

    protected override IEnumerator OnTurnStart()
    {
        yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Banana Bomb"));
        yield return base.OnTurnStart();
    }

}
