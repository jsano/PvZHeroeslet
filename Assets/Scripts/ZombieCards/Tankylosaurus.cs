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
                    if (Tile.GetTeamTiles(GetOpponent(team))[i, j].planted != null) choices.Add(Tile.GetTeamTiles(GetOpponent(team))[i, j].GetComponent<BoxCollider2D>());
                }
            choices.Add(GameManager.Instance.GetTeamHero(GetOpponent(team)).GetComponent<BoxCollider2D>());
            yield return Glow();
            var choice = choices[Random.Range(0, choices.Count)];
            if (choice.GetComponent<Hero>() != null) yield return SyncRandomChoiceAcrossNetwork(-1 + " - " + -1);
            else yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);

            if (int.Parse(GameManager.Instance.GetShuffledList()[0]) == -1)
            {
                yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
                yield return Tile.GetTeamHeroTiles(GetOpponent(team))[col].ReceiveDamage(2, this);
            }
            else
            {
                Tile t = Tile.GetTeamTiles(GetOpponent(team))[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])];
                yield return AttackFX(t.planted);
                yield return t.planted.ReceiveDamage(2, this);
            }
        }
		yield return base.OnCardDraw(team);
	}

}
