using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRooster : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return DamageLane();
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
	{
        choices.Clear();
		if (played.col == col && played.team == Team.Plant && played.type == Type.Unit)
		{
            for (int j = 0; j < 4; j++)
            {
                if (j != col && Tile.zombieTiles[row, j].planted == null)
                {
                    choices.Add(Tile.zombieTiles[row, j].GetComponent<BoxCollider2D>());
                }
            }
            if (choices.Count > 0)
            {
                yield return Glow();
                var choice = choices[Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Move(int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1]));
            }
        }
		yield return base.OnCardPlay(played);
	}

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved == this) yield return DamageLane();
        yield return base.OnCardMoved(moved);
    }

    private IEnumerator DamageLane()
    {
        List<Damagable> targets = new();
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) targets.Add(Tile.plantTiles[i, col].planted);
        yield return Glow();
        yield return AttackFXs(targets);
        foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(1, this, bullseye > 0, deadly > 0));
    }

}
