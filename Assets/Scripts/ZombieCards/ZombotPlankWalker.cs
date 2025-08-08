using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotPlankWalker : Card
{

	protected override IEnumerator OnThisPlay()
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
        if (columns.Count > 0)
        {
            yield return new WaitForSeconds(1);
            string s = "";
            for (int i = 0; i < Mathf.Min(2, columns.Count); i++)
            {
                int c = AllCards.RandomFromTribe((Tribe.Pirate, Tribe.Pirate), true, columns[i] == 4);
                while (c == AllCards.NameToID("Zombot Plank Walker")) c = AllCards.RandomFromTribe((Tribe.Pirate, Tribe.Pirate), true, columns[i] == 4);
                s += columns[i] + " - " + c + " - ";
            }
            yield return SyncRandomChoiceAcrossNetwork(s);
            for (int i = 0; i < GameManager.Instance.GetShuffledList().Count - 1; i += 2)
            {
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[i + 1])]);
                Tile.zombieTiles[0, int.Parse(GameManager.Instance.GetShuffledList()[i])].Plant(c);
            }
        }
        yield return base.OnThisPlay();
	}

}
