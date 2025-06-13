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
        if (GameManager.Instance.team == team)
        {
            List<int> columns = new();
            for (int i = 0; i < 5; i++)
            {
                if (Tile.CanPlantInCol(i, targets, toPlay.teamUp, toPlay.amphibious) && i != col) columns.Add(i);
            }
            for (int n = columns.Count - 1; n > 0; n--)
            {
                int k = UnityEngine.Random.Range(0, n + 1);
                var temp = columns[n];
                columns[n] = columns[k];
                columns[k] = temp;
            }
            GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.NameToID(toPlay.name)), 0, col);
            if (columns.Count > 0) GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.NameToID(toPlay.name)), 0, columns[0]);
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
