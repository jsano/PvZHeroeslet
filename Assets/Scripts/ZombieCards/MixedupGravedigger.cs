using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedupGravedigger: Card
{

	private bool activated = false;

	protected override IEnumerator OnThisPlay()
	{
		if (activated) yield return base.OnThisPlay();
		else
		{
			activated = true;
			List<int> columns = new();
			List<Card> zombies = new();
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[0, col].planted != null)
				{
					columns.Add(col);
					zombies.Add(Tile.zombieTiles[0, col].planted);
                }
			}
			for (int n = columns.Count - 1; n > 0; n--)
			{
				int k = UnityEngine.Random.Range(0, n + 1);
				var temp = columns[n];
				columns[n] = columns[k];
				columns[k] = temp;
			}
			string s = columns[0] + "";
			for (int i = 1; i < columns.Count; i++) s += " - " + columns[i];
			yield return new WaitForSeconds(1);
			yield return SyncRandomChoiceAcrossNetwork(s);
			for (int i = 0; i < zombies.Count; i++)
			{
				Tile.zombieTiles[0, int.Parse(GameManager.Instance.shuffledLists[^1][i])].Unplant();
				Tile.zombieTiles[0, int.Parse(GameManager.Instance.shuffledLists[^1][i])].Plant(zombies[i]);
				zombies[i].Hide();
			}
			GameManager.Instance.currentlySpawningCards -= 1;
            GameManager.Instance.EnablePlayableHandCards();
        }
	}

}
