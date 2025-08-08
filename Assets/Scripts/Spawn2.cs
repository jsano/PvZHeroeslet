using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn2 : Card
{

    public Card toPlay;

	protected override IEnumerator OnThisPlay()
	{	    
        yield return new WaitForSeconds(1);
        var targets = team == Team.Zombie ? Tile.zombieTiles : Tile.plantTiles;
        List<int> columns = new();
        for (int i = 0; i < 5; i++)
        {
            if (Tile.CanPlantInCol(i, targets, toPlay.teamUp, toPlay.amphibious) && i != col) columns.Add(i);
        }
        Card c = Instantiate(toPlay);
        targets[0, col].Plant(c);
        if (columns.Count > 0)
        {
            yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + "");
            c = Instantiate(toPlay);
            targets[0, int.Parse(GameManager.Instance.GetShuffledList()[0])].Plant(c);
        }
        yield return base.OnThisPlay();
	}

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        Tile t = bc.GetComponent<Tile>();
        if (t == null) return false;
        var targets = team == Team.Zombie ? Tile.zombieTiles : Tile.plantTiles;
        if (Tile.CanPlantInCol(t.col, targets, toPlay.teamUp, toPlay.amphibious)) return true;
        return false;
    }

}
