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
			List<Tile> tiles = new();
			List<Card> zombies = new();
            for (int r = 0; r < Tile.ROWS; r++) for (int col = 0; col < 5; col++)
			{
				if (Tile.GetTeamTiles(team)[r, col].planted != null)
				{
					tiles.Add(Tile.GetTeamTiles(team)[r, col]);
					zombies.Add(Tile.GetTeamTiles(team)[r, col].planted);
                }
			}
			for (int n = tiles.Count - 1; n > 0; n--)
			{
				int k = UnityEngine.Random.Range(0, n + 1);
				var temp = tiles[n];
				tiles[n] = tiles[k];
				tiles[k] = temp;
			}
			string s = tiles[0].row + " - " + tiles[0].col;
			for (int i = 1; i < tiles.Count; i++) s += " - " + tiles[i].row + " - " + tiles[i].col;
			yield return Glow();
			yield return SyncRandomChoiceAcrossNetwork(s);
			for (int i = 0; i < zombies.Count; i++)
			{
				Tile.GetTeamTiles(team)[int.Parse(GameManager.Instance.GetShuffledList()[i*2]), int.Parse(GameManager.Instance.GetShuffledList()[i * 2 + 1])].Unplant();
				Tile.GetTeamTiles(team)[int.Parse(GameManager.Instance.GetShuffledList()[i]), int.Parse(GameManager.Instance.GetShuffledList()[i * 2 + 1])].Plant(zombies[i]);
				zombies[i].Hide();
			}
			GameManager.Instance.currentlySpawningCards -= 1;
            GameManager.Instance.EnablePlayableHandCards();
        }
	}

}
