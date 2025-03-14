using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandCard;

public class Tile : MonoBehaviour
{

    public int row;
    public int col;

    /// <summary>
    /// The plant tiles, which will be at the top or bottom depending on the user team
    /// </summary>
    public static Tile[,] plantTiles = new Tile[2, 5];
	/// <summary>
	/// The zombie tiles, which will be at the top or bottom depending on the user team
	/// </summary>
	public static Tile[,] zombieTiles = new Tile[2, 5];
    [HideInInspector] public bool isPlantTile;

    [HideInInspector] public Card planted;

    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {        
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignSide()
    {
        if (transform.position.y < 0)
        {
            if (GameManager.Instance.team == Card.Team.Plant)
            {
                plantTiles[row, col] = this;
                isPlantTile = true;
            }
            else
            {
                zombieTiles[row, col] = this;
                isPlantTile = false;
            }
        }
        else
        {
            if (GameManager.Instance.team == Card.Team.Plant)
            {
                zombieTiles[row, col] = this;
                isPlantTile = false;
            }
            else
            {
                plantTiles[row, col] = this;
                isPlantTile = true;
            }
		}
    }

    public void Plant(Card c)
    {
        if (planted != null) plantTiles[1 - row, col].Plant(planted);
        planted = c;
        c.row = row;
        c.col = col;
        c.transform.position = transform.position;
    }

    public static bool CanPlantInCol(int col, Tile[,] tileObjects, bool teamUp, bool amphibious)
    {
        if (col == 4 && !amphibious) return false;
        if (tileObjects[0, col].planted != null && tileObjects[1, col].planted != null) return false;
        if (tileObjects[0, col].planted == null && tileObjects[1, col].planted == null) return true;
        bool hasTeamup = false;
        if (tileObjects[0, col].planted != null && tileObjects[0, col].planted.teamUp) hasTeamup = true;
        if (tileObjects[1, col].planted != null && tileObjects[1, col].planted.teamUp) hasTeamup = true;
        if (hasTeamup || teamUp) return true;
        return false;
    }

    public void ToggleHighlight(bool on)
    {
        SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, on ? 1 : 0);
    }

}
