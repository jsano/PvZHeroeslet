using System;
using System.Collections;
using System.Collections.Generic;
using SerializableCallback;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static Card;

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

    private int turn = 1;
    private int phase; // 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
    private int remaining = 1;
    public Team team;

    public Button go;
    private LTDescr goTween;
    public GameObject phaseText;
    private Transform handCards;
    public GameObject handcardPrefab;
    public TextMeshProUGUI remainingText;

    [HideInInspector] public Hero plantHero;
    [HideInInspector] public Hero zombieHero;
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
		plantHero = GameObject.Find("Green Shadow").GetComponent<Hero>(); //temp
		zombieHero = GameObject.Find("Super Brainz").GetComponent<Hero>();
        if (IsHost)
		{
			team = Team.Plant;
			plantHero.transform.position = new Vector2(0, -3);
		    zombieHero.transform.position = new Vector2(0, 3.5f);
            zombieHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
            zombieHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);
		}
		else
		{
			team = Team.Zombie;
			zombieHero.transform.position = new Vector2(0, -3);
			plantHero.transform.position = new Vector2(0, 3.5f);
			plantHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
			plantHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);
		}

		foreach (Transform t in GameObject.Find("Tiles").transform)
		{
            t.GetComponent<Tile>().AssignSide();
		}

		int num = AllCards.Instance.cards.Length / 2;
		for (int i = 0; i < num; i++)
		{
			GameObject c = Instantiate(handcardPrefab, handCards);
			c.SetActive(false);
			c.transform.localPosition = new Vector2(-(num-1)/2 + i, 0);
			c.GetComponent<HandCard>().ID = (team == Team.Zombie ? num + i : i);
            c.SetActive(true);
		}
		if (IsServer) return;
        StartCoroutine(Wait1Frame());
    }

    private IEnumerator Wait1Frame()
    {
        yield return null;
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
        if (goTween != null && LeanTween.isTweening(goTween.id)) LeanTween.cancel(goTween.id);
        phaseText.transform.localScale = Vector3.zero;
        goTween = LeanTween.scale(phaseText, Vector3.one, 0.5f).setEaseOutBack().setOnComplete(() => LeanTween.scale(phaseText, Vector3.zero, 0.5f).setEaseInBack().setDelay(1));

        DisableHandCards();
        if (team == Team.Plant)
        {
            if (phase == 2) go.interactable = true;
            else go.interactable = false;
        }
        else
        {
            if (phase == 1 || phase == 3) go.interactable = true;
            else go.interactable = false;
        }
        EnablePlayableHandCards();

        if (phase == 4) StartCoroutine(Combat());
    }

    private IEnumerator Combat()
    {
        yield return new WaitForSeconds(1);

        for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted != null) yield return Tile.zombieTiles[0, col].planted.Attack();

			if (Tile.plantTiles[1, col].planted != null) yield return Tile.plantTiles[1, col].planted.Attack();
			if (Tile.plantTiles[0, col].planted != null) yield return Tile.plantTiles[0, col].planted.Attack();

			for (int col1 = 0; col1 < 5; col1++)
			{
				if (Tile.zombieTiles[0, col1].planted != null) yield return Tile.zombieTiles[0, col1].planted.DieIf0(false);

				if (Tile.plantTiles[1, col1].planted != null) yield return Tile.plantTiles[1, col1].planted.DieIf0(false);
				if (Tile.plantTiles[0, col1].planted != null) yield return Tile.plantTiles[0, col1].planted.DieIf0(false);
			}

			if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.doubleStrike) yield return Tile.zombieTiles[0, col].planted.Attack();

			if (Tile.plantTiles[1, col].planted != null && Tile.plantTiles[1, col].planted.doubleStrike) yield return Tile.plantTiles[1, col].planted.Attack();
			if (Tile.plantTiles[0, col].planted != null && Tile.plantTiles[0, col].planted.doubleStrike) yield return Tile.plantTiles[0, col].planted.Attack();
		}

        turn += 1;
        remaining = turn;
        UpdateBrains(0);
        phase = 0;
        //draw card
        yield return CallLeftToRight("OnTurnStart", null);
		if (IsServer) EndRpc();
    }

    [Rpc(SendTo.Server)]
    public void PlayCardRpc(HandCard.FinalStats fs, int row, int col)
    {
        //if (team == Team.Plant) plants[row + 2*col] = ID;
        //else zombies[row + 2*col] = ID;
        PositionCardRpc(fs, row, col);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PositionCardRpc(HandCard.FinalStats fs, int row, int col)
    {
        Card card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
        card.row = row;
        card.col = col;
        card.atk = fs.atk;
        card.HP = fs.hp;
        //abilities...
        if (card.team == Team.Zombie)
        {
            Tile to = Tile.zombieTiles[row, col];
			card.transform.position = to.transform.position;
            if (to.planted != null)
            {
                Tile.zombieTiles[1 - row, col].planted = to.planted;
                to.planted.row = 1 - row;
                to.planted.transform.position = Tile.zombieTiles[1 - row, col].transform.position;
            }
            to.planted = card;
        }
        else
        {
			Tile to = Tile.plantTiles[row, col];
			card.transform.position = to.transform.position;
			if (to.planted != null)
			{
				Tile.plantTiles[1 - row, col].planted = to.planted;
				to.planted.row = 1 - row;
				to.planted.transform.position = Tile.plantTiles[1 - row, col].transform.position;
			}
			to.planted = card;
		}
    }

	[Rpc(SendTo.Server)]
	public void PlayTrickRpc(HandCard.FinalStats fs, int row, int col, bool isPlantTarget)
	{
		//if (team == Team.Plant) plants[row + 2*col] = ID;
		//else zombies[row + 2*col] = ID;
		PositionTrickRpc(fs, row, col, isPlantTarget);
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void PositionTrickRpc(HandCard.FinalStats fs, int row, int col, bool isPlantTarget)
	{
		Card card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
		card.row = row;
		card.col = col;
		card.atk = fs.atk;
		card.HP = fs.hp;
		if (!isPlantTarget)
		{
            if (row == -1 && col == -1) card.transform.position = zombieHero.transform.position;
            else
            {
                Tile to = Tile.zombieTiles[row, col];
			    card.transform.position = to.transform.position;
            }
		}
		else
		{
            if (row == -1 && col == -1) card.transform.position = plantHero.transform.position;
            else
            {
				Tile to = Tile.plantTiles[row, col];
			    card.transform.position = to.transform.position;
            }
		}
	}

	public static IEnumerator CallLeftToRight(string methodName, Damagable arg)
	{
		for (int i = 0; i < 5; i++)
		{
			if (Tile.zombieTiles[0, i].planted != null) yield return Tile.zombieTiles[0, i].planted.StartCoroutine(methodName, arg);

			if (Tile.plantTiles[1, i].planted != null) yield return Tile.plantTiles[1, i].planted.StartCoroutine(methodName, arg);
			if (Tile.plantTiles[0, i].planted != null) yield return Tile.plantTiles[0, i].planted.StartCoroutine(methodName, arg);
		}
        yield return null;
	}

    public void DisableHandCards()
    {
        foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
    }

    public void EnablePlayableHandCards()
    {
		if (team == Team.Plant)
		{
            if (phase == 2)
            {
                foreach (Transform t in handCards) if (t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = true;
            }
            else foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
		}
		else
		{
            if (phase == 1)
            {
                foreach (Transform t in handCards)
                {
                    if (AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type == Card.Type.Unit)
                    {
						if (t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = true;
                    }
                    else t.GetComponent<HandCard>().interactable = false;
                }
            }
            else if (phase == 3)
            {
				foreach (Transform t in handCards)
				{
                    if (AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type == Card.Type.Unit)
                    {
						if (t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = false;
                    }
                    else t.GetComponent<HandCard>().interactable = true;
				}
			}
		}
	}

    public void UpdateBrains(int change)
    {
        remaining += change;
        remainingText.text = remaining + "";
        DisableHandCards();
        EnablePlayableHandCards();
    }

	[Rpc(SendTo.ClientsAndHost)]
	public void RaiseAttackRpc(Team tteam, int row, int col, int amount)
	{
        if (tteam == Team.Plant) Tile.plantTiles[row, col].planted.RaiseAttack(amount);
        else Tile.zombieTiles[row, col].planted.RaiseAttack(amount);
	}

	[Rpc(SendTo.ClientsAndHost)]
	public void HealRpc(Team tteam, int row, int col, int amount, bool raiseCap)
	{
        if (row == -1 && col == -1)
        {
            if (tteam == Team.Plant) plantHero.Heal(amount, raiseCap);
            else zombieHero.Heal(amount, raiseCap);
        }
        else 
        {
            if (tteam == Team.Plant) Tile.plantTiles[row, col].planted.Heal(amount, raiseCap);
			else Tile.zombieTiles[row, col].planted.Heal(amount, raiseCap);
        }
	}

}
