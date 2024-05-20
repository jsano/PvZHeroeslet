using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public NetworkList<int> plants;
    public NetworkList<int> zombies;

    private int turn;
    private int phase; // 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
    private int remaining;
    public Card.Team team; 

    public HandCard selecting;

    // Start is called before the first frame update
    void Start()
    {
        plants = new NetworkList<int>();
        zombies = new NetworkList<int>();
        turn = 1;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsHost) team = Card.Team.Plant;
        else team = Card.Team.Zombie;

        if (!IsServer) return;
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) plants.Add(-1);
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) zombies.Add(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End()
    {
        phase += 1;
    }

    [Rpc(SendTo.Server)]
    public void PlayCardRpc(int ID, int row, int col, Card.Team team)
    {
        if (team == Card.Team.Plant) plants[row + 2*col] = ID;
        else zombies[row + 2*col] = ID;
        PositionCardRpc(ID, row, col, team);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PositionCardRpc(int ID, int row, int col, Card.Team team)
    {
        Card card = Instantiate(AllCards.Instance.cards[ID]).GetComponent<Card>();
        card.row = row;
        card.col = col;
        if (team != this.team)
        {
            card.transform.position = Tile.opponentTiles[row, col].transform.position;
            Tile.opponentTiles[row, col].planted = card;
        }
        else
        {
            card.transform.position = Tile.tileObjects[row, col].transform.position;
            Tile.tileObjects[row, col].planted = card;
        }
    }

}
