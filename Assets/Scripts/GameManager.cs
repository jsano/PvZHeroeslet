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
    public HandCard selecting;
    private Transform handCards;
    public GameObject handcardPrefab;
    public TextMeshProUGUI remainingText;

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
			team = Team.Plant;
			player = GameObject.Find("Green Shadow").GetComponent<Hero>(); //temp
			opponent = GameObject.Find("Super Brainz").GetComponent<Hero>();

		}
		else
		{
			team = Team.Zombie;
			opponent = GameObject.Find("Green Shadow").GetComponent<Hero>(); //temp
			player = GameObject.Find("Super Brainz").GetComponent<Hero>();
		}
        player.transform.position = new Vector2(0, -3);
		opponent.transform.position = new Vector2(0, 3.5f);
        opponent.GetComponent<SpriteRenderer>().sortingOrder = -1;
        opponent.transform.Find("HeroUI").position *= new Vector2(-1, 1);

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

		Tile[,] first = Tile.tileObjects;
		Tile[,] second = Tile.opponentTiles;
		if (team == Team.Plant)
		{
			first = Tile.opponentTiles;
			second = Tile.tileObjects;
		}
        for (int col = 0; col < 5; col++)
		{
			if (first[1, col].planted != null) yield return first[1, col].planted.Attack();
			if (first[0, col].planted != null) yield return first[0, col].planted.Attack();

			if (second[1, col].planted != null) yield return second[1, col].planted.Attack();
			if (second[0, col].planted != null) yield return second[0, col].planted.Attack();

			for (int col1 = 0; col1 < 5; col1++)
			{
				if (first[1, col1].planted != null) yield return first[1, col1].planted.DieIf0();
				if (first[0, col1].planted != null) yield return first[0, col1].planted.DieIf0();

				if (second[1, col1].planted != null) yield return second[1, col1].planted.DieIf0();
				if (second[0, col1].planted != null) yield return second[0, col1].planted.DieIf0();
			}

			if (first[1, col].planted != null && first[1, col].planted.doubleStrike) yield return first[1, col].planted.Attack();
			if (first[0, col].planted != null && first[0, col].planted.doubleStrike) yield return first[0, col].planted.Attack();

			if (second[1, col].planted != null && second[1, col].planted.doubleStrike) yield return second[1, col].planted.Attack();
			if (second[0, col].planted != null && second[0, col].planted.doubleStrike) yield return second[0, col].planted.Attack();
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
    public void PlayCardRpc(HandCard.mods mods, int row, int col, Team team)
    {
        //if (team == Team.Plant) plants[row + 2*col] = ID;
        //else zombies[row + 2*col] = ID;
        PositionCardRpc(mods, row, col, team);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PositionCardRpc(HandCard.mods mods, int row, int col, Team team)
    {
        Card card = Instantiate(AllCards.Instance.cards[mods.ID]).GetComponent<Card>();
        card.row = row;
        card.col = col;
        card.atk = mods.atk;
        card.HP = mods.hp;
        //abilities...
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

	public static IEnumerator CallLeftToRight(string methodName, Damagable arg)
	{
		Tile[,] first = Tile.tileObjects;
		Tile[,] second = Tile.opponentTiles;
		if (Instance.team == Team.Plant)
		{
			first = Tile.opponentTiles;
			second = Tile.tileObjects;
		}

		for (int i = 0; i < 5; i++)
		{
			if (first[1, i].planted != null) yield return first[1, i].planted.StartCoroutine(methodName, arg);
			if (first[0, i].planted != null) yield return first[0, i].planted.StartCoroutine(methodName, arg);

			if (second[1, i].planted != null) yield return second[1, i].planted.StartCoroutine(methodName, arg);
			if (second[0, i].planted != null) yield return second[0, i].planted.StartCoroutine(methodName, arg);
		}
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
        if (tteam == team) Tile.tileObjects[row, col].planted.RaiseAttack(amount);
        else Tile.opponentTiles[row, col].planted.RaiseAttack(amount);
	}

	[Rpc(SendTo.ClientsAndHost)]
	public void HealRpc(Team tteam, int row, int col, int amount, bool raiseCap)
	{
        if (tteam == team)
        {
            if (row == -1 && col == -1) player.Heal(amount, raiseCap);
            else Tile.tileObjects[row, col].planted.Heal(amount, raiseCap);
        }
        else
        {
			if (row == -1 && col == -1) opponent.Heal(amount, raiseCap);
			else Tile.opponentTiles[row, col].planted.Heal(amount, raiseCap);
        }
	}

}
