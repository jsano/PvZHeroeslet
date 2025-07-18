using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Tile : Damagable
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
    /// The plant hero tiles, which will be at the top or bottom depending on the user team
    /// </summary>
    public static Tile[] plantHeroTiles = new Tile[5];
    /// <summary>
    /// The zombie hero tiles, which will be at the top or bottom depending on the user team
    /// </summary>
    public static Tile[] zombieHeroTiles = new Tile[5];
    /// <summary>
    /// True if this tile belongs to the plant side, false if it's the zombie side
    /// </summary>
    [HideInInspector] public bool isPlantTile;
    /// <summary>
    /// The terrain tiles. Only index 1-3 is used, others don't matter, but still need to have them for consistency
    /// </summary>
    public static Tile[] terrainTiles = new Tile[5];

    /// <summary>
    /// The reference to the card currently placed in this tile. Add/remove this using <c>Plant()</c> and <c>Unplant()</c>
    /// </summary>
    public Card planted { get; private set; }

    private SpriteRenderer SR;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {        
        SR = GetComponent<SpriteRenderer>();
        target = transform.Find("Target").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignSide()
    {
        if (row == 2)
        {
            terrainTiles[col] = this;
            return;
        }
        if (transform.position.y < 0)
        {
            if (GameManager.Instance.team == Card.Team.Plant)
            {
                if (row == -1) plantHeroTiles[col] = this;
                else plantTiles[row, col] = this;
                isPlantTile = true;
            }
            else
            {
                if (row == -1) zombieHeroTiles[col] = this;
                else zombieTiles[row, col] = this;
                isPlantTile = false;
            }
        }
        else
        {
            if (GameManager.Instance.team == Card.Team.Plant)
            {
                if (row == -1) zombieHeroTiles[col] = this;
                else zombieTiles[row, col] = this;
                isPlantTile = false;
            }
            else
            {
                if (row == -1) plantHeroTiles[col] = this;
                else plantTiles[row, col] = this;
                isPlantTile = true;
            }
		}
    }

    /// <summary>
    /// Places a card onto this tile and sets the card's position and row/column values.
    /// This assumes the card can legally be placed here. If there is already a card here, it will replant that card onto the opposite row.
    /// If there is a terrain here and <c>c</c> is also a terrain, destroys the old terrain. Updates any anti-hero
    /// </summary>
    public void Plant(Card c)
    {
        if (planted != null)
        {
            if (row == 2)
            {
                planted.Destroy();
                terrainTiles[0].planted = planted; // Can't do Plant() since it changes col
            }
            bool move = true;
            if (c.evolution != Card.Tribe.Animal)
            {
                move = false;
                if (!planted.fusion) Destroy(planted.gameObject);
                c.evolved = true;
            }
            if (planted.fusion)
            {
                move = false;
                planted.transform.Find("ATK").gameObject.SetActive(false);
                planted.transform.Find("HP").gameObject.SetActive(false);
                planted.GetComponent<SpriteRenderer>().sortingOrder = c.GetComponent<SpriteRenderer>().sortingOrder - 1;
                c.fusionBase = planted;
                if (planted.amphibious) c.amphibious = true;
            }
            if (move) plantTiles[1 - row, col].Plant(planted);
        }
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

    public static Card IsOnField(string name)
    {
        for (int col = 0; col < 5; col++)
        {
            for (int row = 0; row < 2; row++)
            {
                if (plantTiles[row, col].planted != null && plantTiles[row, col].planted.name.Contains(name))
                {
                    return plantTiles[row, col].planted;
                }
                if (zombieTiles[row, col].HasRevealedPlanted() && zombieTiles[row, col].planted.name.Contains(name))
                {
                    return zombieTiles[row, col].planted;
                }
            }
            if (terrainTiles[col].planted != null && terrainTiles[col].planted.name.Contains(name))
            {
                return terrainTiles[col].planted;
            }
        }
        return null;
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

    public void ToggleTarget(bool on)
    {
        target.SetActive(on);
    }

    /// <summary>
    /// Use instead of <c>Hero.ReceiveDamage</c> when the column where the hero got attacked is important for event trigger ordering (ex. cards that attack here and next door)
    /// </summary>
    /// <param name="heroCol">Never pass in directly (this tile object will pass in its own column). For cards that hit multi-lane, the hero could be hit alongside other units.
    /// This is what the hero's "column" should be registered as to maintain proper order</param>
    public override IEnumerator ReceiveDamage(int dmg, Card source, bool bullseye = false, bool deadly = false, bool freeze = false, int heroCol = -1)
    {
        if (isPlantTile) yield return GameManager.Instance.plantHero.ReceiveDamage(dmg, source, bullseye, false, false, col);
        else yield return GameManager.Instance.zombieHero.ReceiveDamage(dmg, source, bullseye, false, false, col);
    }

    /// <summary>
    /// This should never be called
    /// </summary>
    public override IEnumerator Heal(int amount)
    {
        yield break;
    }

    /// <summary>
    /// This should never be called
    /// </summary>
    public override void ChangeStats(int atkAmount, int hpAmount)
    {

    }

    /// <summary>
    /// This should never be called
    /// </summary>
    public override bool isDamaged()
    {
        return false;
    }

    /// <summary>
    /// This should never be called
    /// </summary>
    public override void ToggleInvulnerability(bool active)
    {
        
    }
}
