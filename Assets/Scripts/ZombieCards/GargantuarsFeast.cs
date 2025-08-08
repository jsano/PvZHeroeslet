using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargantuarsFeast : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        List<int> locations = new();
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].planted == null) locations.Add(col);
        }
        for (int n = locations.Count - 1; n > 0; n--)
        {
            int k = UnityEngine.Random.Range(0, n + 1);
            var temp = locations[n];
            locations[n] = locations[k];
            locations[k] = temp;
        }
        if (locations.Count > 0)
        {
            string s = "";
            for (int i = 0; i < Mathf.Min(3, locations.Count); i++)
            {
                s += locations[i] + " - " + AllCards.RandomFromTribe((Tribe.Gargantuar, Tribe.Gargantuar), true, locations[i] == 4) + " - ";
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
