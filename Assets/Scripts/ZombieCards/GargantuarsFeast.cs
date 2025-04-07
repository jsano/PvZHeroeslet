using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargantuarsFeast : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team)
		{
			List<int> locations = new();
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[0, col].planted == null) locations.Add(col);
			}
			for (int i = 0; i < 3 && locations.Count > 0; i++) {
				int cur = Random.Range(0, locations.Count);
				GameManager.Instance.PlayCardRpc(FinalStats.MakeDefaultFS(AllCards.RandomFromTribe((Tribe.Gargantuar, Tribe.Gargantuar), true, locations[cur] == 4)), row, locations[cur], true);
				locations.RemoveAt(cur);
			}
		}
		yield return base.OnThisPlay();
	}

}
