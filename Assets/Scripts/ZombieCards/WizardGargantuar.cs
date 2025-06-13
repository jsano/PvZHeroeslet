using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardGargantuar : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
                if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted.tribes.Contains(Tribe.Gargantuar))
                    Tile.zombieTiles[row, col].planted.bullseye += 1;
                if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted != this)
				{
					choices.Add(Tile.zombieTiles[row, col].GetComponent<BoxCollider2D>());
				}
			}
		}
        if (choices.Count == 1) yield return OnSelection(choices[0]);
        if (choices.Count >= 2)
        {
            if (GameManager.Instance.team == team) selected = false;
            yield return new WaitUntil(() => GameManager.Instance.selection != null);
            yield return OnSelection(GameManager.Instance.selection);
        }
        yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return base.OnSelection(bc);
		Tile t = bc.GetComponent<Tile>();
		Card toDestroy = t.planted;
        t.Unplant(true);
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.RandomFromCost(Team.Zombie, (toDestroy.cost + 1, toDestroy.cost + 1), true, toDestroy.col == 4)), toDestroy.row, toDestroy.col);
		Destroy(toDestroy.gameObject);
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.tribes.Contains(Tribe.Gargantuar)) played.bullseye += 1;
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died == this)
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted.tribes.Contains(Tribe.Gargantuar))
                        Tile.zombieTiles[row, col].planted.bullseye -= 1;
                }
            }
        yield return base.OnCardDeath(died);
    }

}
