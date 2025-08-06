using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tankylosaurus : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
            choices.Clear();
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
                {
                    if (Tile.plantTiles[i, j].planted != null) choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
                }
            choices.Add(GameManager.Instance.plantHero.GetComponent<BoxCollider2D>());
            yield return new WaitForSeconds(1);
            var choice = choices[Random.Range(0, choices.Count)];
            if (choice.GetComponent<Hero>() != null) yield return SyncRandomChoiceAcrossNetwork(-1 + " - " + -1);
            else yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);

            if (int.Parse(GameManager.Instance.shuffledLists[^1][0]) == -1)
            {
                yield return AttackFX(Tile.plantHeroTiles[col]);
                yield return Tile.plantHeroTiles[col].ReceiveDamage(2, this);
            }
            else
            {
                Tile t = Tile.plantTiles[int.Parse(GameManager.Instance.shuffledLists[^1][0]), int.Parse(GameManager.Instance.shuffledLists[^1][1])];
                yield return AttackFX(t.planted);
                yield return t.planted.ReceiveDamage(2, this);
            }
        }
		yield return base.OnCardDraw(team);
	}

}
