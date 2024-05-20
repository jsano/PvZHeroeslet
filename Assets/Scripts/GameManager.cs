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

    private int turn;
    private int phase; // 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
    private int remaining;
    public Card.Team team; 

    public HandCard selecting;

    // Start is called before the first frame update
    void Start()
    {
        turn = 1;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsHost) team = Card.Team.Plant;
        else team = Card.Team.Zombie;
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
    public void PlayCardRpc(int ID, int row, int col)
    {
        Card card = Instantiate(AllCards.Instance.cards[ID]).GetComponent<Card>();
        card.row = row;
        card.col = col;
        card.GetComponent<NetworkObject>().Spawn();
    }

}
