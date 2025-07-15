using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntergalacticWarlord : Card
{

    protected override IEnumerator OnThisPlay()
    {
        GameManager.Instance.zombiePermanentAttackBonus += 1;
        GameManager.Instance.zombiePermanentHPBonus += 1;
        for (int j = 0; j < 5; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted()) Tile.zombieTiles[0, j].planted.ChangeStats(1, 1);
        yield return base.OnThisPlay();
    }

}
