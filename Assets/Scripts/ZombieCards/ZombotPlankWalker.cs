using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotPlankWalker : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team)
        {
            List<int> columns = new();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
            }
            for (int n = columns.Count - 1; n > 0; n--)
            {
                int k = UnityEngine.Random.Range(0, n + 1);
                var temp = columns[n];
                columns[n] = columns[k];
                columns[k] = temp;
            }
            for (int i = 0; i < 2; i++)
		    {
                if (i >= columns.Count) break;
                int c = AllCards.RandomFromTribe((Tribe.Pirate, Tribe.Pirate), true, columns[i] == 4);
                while (c == AllCards.NameToID("Zombot Plank Walker")) c = AllCards.RandomFromTribe((Tribe.Pirate, Tribe.Pirate), true, columns[i] == 4); // BROKEN UNTIL ANOTHER AMPHIBIOUS PIRATE
                GameManager.Instance.PlayCardRpc(new FinalStats(c), 0, columns[i]);
            }
        }

        yield return base.OnThisPlay();
	}

}
