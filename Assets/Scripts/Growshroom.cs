using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growshroom : Card
{

	private bool selecting = true;
	private List<Tile> choices = new List<Tile> ();
	private Camera cam;

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played == this)
		{
			GameManager.Instance.go.interactable = false;
			GameManager.Instance.DisableHandCards();
			cam = GameObject.Find("Main Camera").GetComponent<Camera>();
			for (int row = 0; row < 2; row++)
			{
				for (int col = 0; col < 5; col++)
				{
					if (Tile.tileObjects[row, col].planted != null && Tile.tileObjects[row, col].planted != this)
					{
						choices.Add(Tile.tileObjects[row, col]);
					}
				}
			}
			yield return new WaitUntil(() => selecting == false);
			GameManager.Instance.go.interactable = true;
			GameManager.Instance.EnablePlayableHandCards();
		}
		yield return null;
	}

	void Update()
	{
		if (selecting)
		{
			if (Input.GetMouseButtonDown(0))
			{
				foreach (Tile tile in choices)
				{
					if (tile.GetComponent<BoxCollider2D>().bounds.Contains((Vector2) cam.ScreenToWorldPoint(Input.mousePosition)))
					{
						GameManager.Instance.RaiseAttackRpc(team, tile.row, tile.col, 2);
						GameManager.Instance.HealRpc(team, tile.row, tile.col, 2, true);
						selecting = false;
						break;
					}
				}
			}
		}
	}

}
