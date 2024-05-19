using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int turn;
    private int phase; // 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
    private int remaining;

    public static HandCard selecting;

    // Start is called before the first frame update
    void Start()
    {
        turn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End()
    {
        phase += 1;
    }

}
