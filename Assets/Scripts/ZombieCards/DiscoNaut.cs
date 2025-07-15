using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscoNaut : Card
{

    private List<Card> buffed = new();

    protected override IEnumerator OnThisPlay()
    {
        for (int j = 0; j < 5; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted() && Tile.zombieTiles[0, j].planted.atk <= 2)
            {
                Tile.zombieTiles[0, j].planted.bullseye += 1;
                buffed.Add(Tile.zombieTiles[0, j].planted);
            }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == Team.Zombie && played.type == Type.Unit && played.atk <= 2)
        {
            played.bullseye += 1;
            buffed.Add(played);
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardStatsChanged(Tuple<Card, int, int> changed)
    {
        if (changed.Item1.team == Team.Zombie && changed.Item1.atk <= 2 && !buffed.Contains(changed.Item1))
        {
            changed.Item1.bullseye += 1;
            buffed.Add(changed.Item1);
        }
        if (changed.Item1.team == Team.Zombie && changed.Item1.atk > 2 && buffed.Contains(changed.Item1))
        {
            changed.Item1.bullseye -= 1;
            buffed.Remove(changed.Item1);
        }
        yield return base.OnCardStatsChanged(changed);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
        {
            foreach (Card c in buffed) if (c != null) c.bullseye -= 1;
        }
        yield return base.OnCardDeath(died);
    }

}
