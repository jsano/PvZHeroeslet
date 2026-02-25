using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberrian : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (evolved)
        {
            yield return Glow();
            yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Berry Blast"));
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Berry))
		{
			List<Damagable> targets = new();
			for (int i = -1; i <= 1; i += 2)
			{
                for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(GetOpponent(team))[r, col + i].HasRevealedPlanted()) targets.Add(Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted);
			}
            yield return Glow();
            yield return AttackFXs(targets);
            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(1, this));
		}
		yield return base.OnCardPlay(played);
	}

}
