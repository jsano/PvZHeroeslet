using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

		plants = new NetworkList<int>();
		zombies = new NetworkList<int>();
        
        handCards = transform.Find("HandCards");
        turn = 1;
	}

    public NetworkList<int> plants; // for when the game becomes 1 authoritative server with 2 clients
    public NetworkList<int> zombies;

    private int turn;
    private int phase; // 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
    private int remaining;
    public Card.Team team;

    public Button go;
    public GameObject phaseText;
    public HandCard selecting;
    private Transform handCards;
    public GameObject handcardPrefab;

    [HideInInspector] public Hero player;
    [HideInInspector] public Hero opponent;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer) return;
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) plants.Add(-1);
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) zombies.Add(-1);
	}

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.OnClientConnectedCallback += P2Joined;
	}

    private void P2Joined(ulong data)
    {
        if (data == NetworkManager.ServerClientId) return;
        Debug.Log("Client " + data + " connected");
		if (IsHost)
		{
			team = Card.Team.Plant;
			player = GameObject.Find("Green Shadow").GetComponent<Hero>(); //temp
			opponent = GameObject.Find("Super Brainz").GetComponent<Hero>();

		}
		else
		{
			team = Card.Team.Zombie;
			opponent = GameObject.Find("Green Shadow").GetComponent<Hero>(); //temp
			player = GameObject.Find("Super Brainz").GetComponent<Hero>();
		}
        player.transform.position = new Vector2(0, -3);
		opponent.transform.position = new Vector2(0, 3.5f);

		for (int i = 0; i < 2; i++)
		{
			GameObject c = Instantiate(handcardPrefab, handCards);
			c.transform.localPosition = new Vector2(i * 1.5f, 0);
			c.GetComponent<HandCard>().ID = (team == Card.Team.Zombie ? AllCards.Instance.cards.Length / 2 + i : i);
		}
		if (IsServer) return;
        EndRpc();
    }

    // Update is called once per frame
    void Update()
    {
		
	}

    [Rpc(SendTo.ClientsAndHost)]
    public void EndRpc()
    {
        string[] pnames = new string[] { "", "Zombies\nPlay", "Plants\nPlay", "Zombie\nTricks", "FIGHT!" };
        phase += 1;
        phaseText.GetComponent<TextMeshProUGUI>().text = pnames[phase];
        LeanTween.scale(phaseText, Vector3.one, 0.5f).setEaseOutBack();
        LeanTween.scale(phaseText, Vector3.zero, 0.5f).setEaseInBack().setDelay(1.5f);

        if (team == Card.Team.Plant)
        {
            if (phase == 2)
            {
                go.interactable = true;
                foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = true;
            }
            else
            {
                go.interactable = false;
                foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
            }
        }
        else
        {
            if (phase == 1 || phase == 3)
            {
                go.interactable = true;
                foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = true;
            }
            else
            {
                go.interactable = false;
                foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
            }
        }

        if (phase == 4) StartCoroutine(Combat());
    }

    private IEnumerator Combat()
    {
        for (int col = 0; col < 5; col++)
        {
            for (int row = 0; row < 2; row++)
            {
                if (team == Card.Team.Zombie)
                {
                    if (Tile.tileObjects[row, col].planted != null) yield return Tile.tileObjects[row, col].planted.Attack();                    
                    if (Tile.opponentTiles[row, col].planted != null) yield return Tile.opponentTiles[row, col].planted.Attack();
				}
                else
                {
                    if (Tile.opponentTiles[row, col].planted != null) yield return Tile.opponentTiles[row, col].planted.Attack();                    
                    if (Tile.tileObjects[row, col].planted != null) yield return Tile.tileObjects[row, col].planted.Attack();
				}
            }
            for (int col1 = 0; col1 < 5; col1++) for (int row1 = 0; row1 < 2; row1++)
                {
                    if (team == Card.Team.Zombie)
                    {
                        if (Tile.tileObjects[row1, col1].planted != null) yield return Tile.tileObjects[row1, col1].planted.DieIf0();
                        if (Tile.opponentTiles[row1, col1].planted != null) yield return Tile.opponentTiles[row1, col1].planted.DieIf0();
                    }
                    else
                    {
                        if (Tile.opponentTiles[row1, col1].planted != null) yield return Tile.opponentTiles[row1, col1].planted.DieIf0();
                        if (Tile.tileObjects[row1, col1].planted != null) yield return Tile.tileObjects[row1, col1].planted.DieIf0();
					}
                }
        }

        turn += 1;
        remaining = turn;
        phase = 0;
        //draw card
        if (IsServer) EndRpc();
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
