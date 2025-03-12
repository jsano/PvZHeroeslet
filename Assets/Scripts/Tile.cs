using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        planted = c;
        c.row = row;
        c.col = col;
        c.transform.position = transform.position;
    }

    public void ToggleHighlight(bool on)
    {
        SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, on ? 1 : 0);
    }

}
