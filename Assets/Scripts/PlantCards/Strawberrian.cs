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
				if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) targets.Add(Tile.zombieTiles[0, col + i].planted);
			}
            yield return Glow();
            yield return AttackFXs(targets);
            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(1, this));
		}
		yield return base.OnCardPlay(played);
	}

}
