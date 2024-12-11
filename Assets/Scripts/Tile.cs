using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int row;
    public int col;

    public static Tile[,] tileObjects = new Tile[2, 5];
    public static Tile[,] opponentTiles = new Tile[2, 5];

    [HideInInspector] public Card planted;

    private SpriteRenderer SR;
    private BoxCollider2D BC;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.y < 0) tileObjects[row, col] = this;
        else opponentTiles[row, col] = this;
        SR = GetComponent<SpriteRenderer>();
        BC = GetComponent<BoxCollider2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0 && GameManager.Instance.selecting != null && BC.bounds.Contains((Vector2) cam.ScreenToWorldPoint(Input.mousePosition)) && planted == null)
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 1);
        }
        else SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0);
    }

}
