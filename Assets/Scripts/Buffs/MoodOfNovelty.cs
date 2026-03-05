using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfNovelty : Buff
{

    private int turns = 0;

	protected override IEnumerator OnTurnStart()
    {
        turns += 1;
        if (turns % 2 == 0)
        {
            int count = 0;
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted())
                    {
                        Card c = Tile.GetTeamTiles(team)[i, j].planted;
                        if (c._class == Card.Class.Awe)
                        {
                            c.ChangeStats(c.atk, c.HP);
                            count += 1;
                        }
                    }
            if (count > 0) StartCoroutine(Glow());
        }
        yield return base.OnTurnStart();
    }

}
