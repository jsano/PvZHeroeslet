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
    /// <summary>
    /// True if this tile belongs to the plant side, false if it's the zombie side
    /// </summary>
    [HideInInspector] public bool isPlantTile;

    /// <summary>
    /// The reference to the card currently placed in this tile. Add/remove this using <c>Plant()</c> and <c>Unplant()</c>
    /// </summary>
    public Card planted { get; private set; }

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

    /// <summary>
    /// Places a card onto this tile and sets the card's position and row/column values.
    /// This assumes the card can legally be placed here. If there is already a card here, it will replant that card onto the opposite row. Updates any anti-hero
    /// </summary>
    public void Plant(Card c)
    {
        if (planted != null) plantTiles[1 - row, col].Plant(planted);
        planted = c;
        c.row = row;
        c.col = col;
        c.transform.position = transform.position;

        for (int i = 0; i < 2; i++)
        {
            if (plantTiles[i, col].HasRevealedPlanted()) plantTiles[i, col].planted.UpdateAntihero();
            if (zombieTiles[i, col].HasRevealedPlanted()) zombieTiles[i, col].planted.UpdateAntihero();
        }
    }

    /// <summary>
    /// Nullifies this tile's reference to its planted card. The card won't be destroyed nor have its values updated so it should be done separately
    /// </summary>
    /// <param name="silent">If true, this doesn't update anti-hero (only useful for visual consistency)</param>
    public void Unplant(bool silent = false)
    {
        planted = null;

        if (!silent)
            for (int i = 0; i < 2; i++)
            {
                if (plantTiles[i, col].HasRevealedPlanted()) plantTiles[i, col].planted.UpdateAntihero();
                if (zombieTiles[i, col].HasRevealedPlanted()) zombieTiles[i, col].planted.UpdateAntihero();
            }
    }

    /// <summary>
    /// Returns whether or not there is at least 1 tile in the given column that a card with the given attributes will be able to be planted in
    /// </summary>
    /// <param name="tileObjects">The tile array to check in (plant or zombie)</param>
    /// <param name="teamUp">Whether this card has Team-up</param>
    /// <param name="amphibious">Whether this card has Amphibious</param>
    public static bool CanPlantInCol(int col, Tile[,] tileObjects, bool teamUp, bool amphibious)
    {
        // If the card isn't amphibious and this is the water lane, can't plant here
        if (col == 4 && !amphibious) return false;
        // If both rows are full or empty, instant answer
        if (tileObjects[0, col].planted != null && tileObjects[1, col].planted != null) return false;
        if (tileObjects[0, col].planted == null && tileObjects[1, col].planted == null) return true;
        // If either card has team-up, can plant here
        bool hasTeamup = false;
        if (tileObjects[0, col].planted != null && tileObjects[0, col].planted.teamUp) hasTeamup = true;
        if (tileObjects[1, col].planted != null && tileObjects[1, col].planted.teamUp) hasTeamup = true;
        if (hasTeamup || teamUp) return true;
        return false;
    }

    /// <summary>
    /// Whether this tile has a planted card that isn't hiding in a gravestone
    /// </summary>
    public bool HasRevealedPlanted()
    {
        return planted != null && !planted.gravestone;
    }

    public void ToggleHighlight(bool on)
    {
        SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, on ? 1 : 0);
    }

}
