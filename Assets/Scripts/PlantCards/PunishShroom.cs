using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunishShroom : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
        choices.Clear();
        if (died.Item1.tribes.Contains(Tribe.Mushroom))
        {
            for (int j = 0; j < 5; j++)
            {
                if (Tile.zombieTiles[0, j].planted != null) choices.Add(Tile.zombieTiles[0, j].GetComponent<BoxCollider2D>());
            }
            choices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());

            var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
            if (choice.GetComponent<Hero>() != null) yield return SyncRandomChoiceAcrossNetwork(-1 + " - " + -1);
            else yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);

            if (int.Parse(GameManager.Instance.shuffledLists[^1][0]) == -1)
            {
                yield return AttackFX(Tile.zombieHeroTiles[col]);
                yield return Tile.zombieHeroTiles[col].ReceiveDamage(2, this);
            }
            else
            {
                Tile t = Tile.zombieTiles[int.Parse(GameManager.Instance.shuffledLists[^1][0]), int.Parse(GameManager.Instance.shuffledLists[^1][1])];
                yield return AttackFX(t.planted);
                yield return t.planted.ReceiveDamage(2, this);
            }
        }
        yield return base.OnCardDeath(died);
	}

}
