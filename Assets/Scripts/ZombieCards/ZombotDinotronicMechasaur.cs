using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotDinotronicMechasaur : Card
{

	protected override IEnumerator OnCardDraw(Team team)
    {
        if (team == this.team)
        {
            List<int> columns = new();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
            }
            if (columns.Count > 0)
            {
                yield return Glow();
                yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + " - " + AllCards.RandomFromTribe((Tribe.History, Tribe.History), true));
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[1])]).GetComponent<Card>();
                Tile.zombieTiles[0, int.Parse(GameManager.Instance.GetShuffledList()[0])].Plant(c);
            }
        }
        yield return base.OnCardDraw(team);
	}

}
