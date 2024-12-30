using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int row;
    public int col;

    /// <summary>
    /// The player tiles, aka tiles on the lower half of the screen
    /// </summary>
    public static Tile[,] tileObjects = new Tile[2, 5];
	/// <summary>
	/// The opponent tiles, aka tiles on the upper half of the screen
	/// </summary>
	public static Tile[,] opponentTiles = new Tile[2, 5];
    // TODO: maybe replace with plantTiles/zombieTiles

    [HideInInspector] public Card planted;

    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.y < 0) tileObjects[row, col] = this;
        else opponentTiles[row, col] = this;
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHighlight(bool on)
    {
        SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, on ? 1 : 0);
    }

}
