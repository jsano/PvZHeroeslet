using System;
using System.Collections;
using System.Collections.Generic;
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
        
        handCards = transform.Find("HandCards");
        turn = 1;
	}

    private int turn = 1;
    private int phase; // 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
    private int remaining = 1;
    private int opponentRemaining = 1;
    public Team team;

    public Button go;
    private LTDescr goTween;
    public GameObject phaseText;
    private Transform handCards;
    public GameObject handcardPrefab;
    public TextMeshProUGUI remainingText;
	public TextMeshProUGUI opponentRemainingText;

	[HideInInspector] public Hero plantHero;
    [HideInInspector] public Hero zombieHero;
	[HideInInspector] public bool waitingOnBlock = false;
	
    // Start is called before the first frame update
    void Start()
    {        
		plantHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.PlantHero]).GetComponent<Hero>();
		zombieHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.ZombieHero]).GetComponent<Hero>();
        if (IsHost)
		{
			team = Team.Plant;
			plantHero.transform.position = new Vector2(0, -3);
		    zombieHero.transform.position = new Vector2(0, 3.5f);
            zombieHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
            zombieHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);

			UserAccounts.GameStats.Deck = new List<int>(new int[] {
                AllCards.NameToID("Wall-nut"),
                AllCards.NameToID("Pineclone"),
                AllCards.NameToID("Wall-nut Bowling"),
                AllCards.NameToID("Sunflower"),
                AllCards.NameToID("Poppin' Poppies"), 
				5,5 }); //temp
		}
		else
		{
			team = Team.Zombie;
			zombieHero.transform.position = new Vector2(0, -3);
			plantHero.transform.position = new Vector2(0, 3.5f);
			plantHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
			plantHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);

            UserAccounts.GameStats.Deck = new List<int>(new int[] {
                AllCards.NameToID("Disco"),
                AllCards.NameToID("Smoke Bomb"),
                AllCards.NameToID("Pied Piper"),
                AllCards.NameToID("Lurch for Lunch"), 
				50, 
				44,44 });
        }

		foreach (Transform t in GameObject.Find("Tiles").transform)
		{
            t.GetComponent<Tile>().AssignSide();
		}

		var deck = UserAccounts.GameStats.Deck;
        for (int n = deck.Count - 1; n > 0; n--)
        {
            int k = UnityEngine.Random.Range(0, n + 1);
            int temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }

		StartCoroutine(Mulligan());
    }

	private IEnumerator Mulligan()
	{
		yield return DrawCard(4);
		if (IsServer) yield break;
        yield return null;
		EndRpc();
	}

	public IEnumerator DrawCard(int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			yield return GainHandCard(UserAccounts.GameStats.Deck[0]);
			UserAccounts.GameStats.Deck.RemoveAt(0);
		}
    }

    public IEnumerator GainHandCard(int id)
	{
		GameObject c = Instantiate(handcardPrefab, handCards);
        c.SetActive(false);
		for (int i = 0; i < handCards.childCount; i++)
		{
			handCards.GetChild(i).transform.localPosition = new Vector2(1.2f * (-(handCards.childCount - 1) / 2f + i), 0);
		}
        c.GetComponent<HandCard>().ID = id;
        c.SetActive(true);
		yield return CallLeftToRight("OnCardDraw", null);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void EndRpc()
    {
        StartCoroutine(EndRpcHelper());
    }

    private IEnumerator EndRpcHelper()
    {
        string[] pnames = new string[] { "", "Zombies\nPlay", "Plants\nPlay", "Zombie\nTricks", "FIGHT!" };
        phase += 1;

        phaseText.GetComponent<TextMeshProUGUI>().text = pnames[phase];
        if (goTween != null && LeanTween.isTweening(goTween.id)) LeanTween.cancel(goTween.id);
        phaseText.transform.localScale = Vector3.zero;
        goTween = LeanTween.scale(phaseText, Vector3.one, 0.5f).setEaseOutBack().setOnComplete(() => LeanTween.scale(phaseText, Vector3.zero, 0.5f).setEaseInBack().setDelay(1));

        DisableHandCards();
        if (phase == 3)
        {
			for (int col = 0; col < 5; col++)
			{
				Card c = Tile.zombieTiles[0, col].planted;
				if (c != null && c.gravestone)
				{
					if (c.team != team) UpdateRemaining(-c.playedCost, Team.Zombie);
					yield return c.Reveal();
				}
			}
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
            
            yield return CheckDeaths();

            yield return HandleHeroBlocks();

			if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.doubleStrike) yield return Tile.zombieTiles[0, col].planted.Attack();

			if (Tile.plantTiles[1, col].planted != null && Tile.plantTiles[1, col].planted.doubleStrike) yield return Tile.plantTiles[1, col].planted.Attack();
			if (Tile.plantTiles[0, col].planted != null && Tile.plantTiles[0, col].planted.doubleStrike) yield return Tile.plantTiles[0, col].planted.Attack();
		}

        turn += 1;
        remaining = turn;
		opponentRemaining = turn;
		UpdateRemaining(0, Team.Plant);
		UpdateRemaining(0, Team.Zombie);
		phase = 0;

        yield return DrawCard();

        yield return CallLeftToRight("OnTurnStart", null);
		if (IsServer) EndRpc();
    }

    // For whatever reason CallLeftToRight("DieIfZero") breaks???
    public IEnumerator CheckDeaths()
    {
		for (int col1 = 0; col1 < 5; col1++)
		{
			if (Tile.zombieTiles[0, col1].planted != null) yield return Tile.zombieTiles[0, col1].planted.DieIfZero();
			if (Tile.plantTiles[1, col1].planted != null) yield return Tile.plantTiles[1, col1].planted.DieIfZero();
			if (Tile.plantTiles[0, col1].planted != null) yield return Tile.plantTiles[0, col1].planted.DieIfZero();
		}
	}

    [Rpc(SendTo.Server)]
    public void PlayCardRpc(HandCard.FinalStats fs, int row, int col, bool free=false)
    {
        //if (team == Team.Plant) plants[row + 2*col] = ID;
        //else zombies[row + 2*col] = ID;
        PositionCardRpc(fs, row, col, free);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PositionCardRpc(HandCard.FinalStats fs, int row, int col, bool free=false)
    {
		Card card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
        card.atk = fs.atk;
        card.HP = fs.hp;
        //abilities...
        if (card.team == Team.Zombie)
        {
			Tile.zombieTiles[row, col].Plant(card);
        }
        else
        {
			Tile to = Tile.plantTiles[row, col];
			if (to.planted != null)
			{
				Tile.plantTiles[1 - row, col].Plant(to.planted);
			}
			to.Plant(card);
		}

        if (card.team != team)
        {
			if (!free)
			{
				if (!card.gravestone)
				{
					UpdateRemaining(-fs.cost, card.team);
				} else card.playedCost = fs.cost;
			}
        }
        else
        {
            if (!free) UpdateRemaining(-fs.cost, team);
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

		UpdateRemaining(-fs.cost, card.team);
	}

	[Rpc(SendTo.ClientsAndHost)]
	public void HoldTrickRpc()
	{
        waitingOnBlock = false;
	}

	[Rpc(SendTo.ClientsAndHost)]
	public void MoveRpc(Team tteam, int row, int col, int nrow, int ncol)
	{
		Card c;
		if (tteam == Team.Plant)
		{
			c = Tile.plantTiles[row, col].planted;
			Tile.plantTiles[row, col].planted = null;
			Tile.plantTiles[nrow, ncol].Plant(c);
		}
		else
		{
			c = Tile.zombieTiles[row, col].planted;
			Tile.zombieTiles[row, col].planted = null;
			Tile.zombieTiles[nrow, ncol].Plant(c);
		}
		if (tteam != team) StartCoroutine(CallLeftToRight("OnCardMoved", c)); //THE MOVING TEAM SHOULD YIELD BREAK THEIR OWN CALL (TODO: maybe rework???)
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

	public static IEnumerator CallLeftToRight(string methodName)
	{
		for (int i = 0; i < 5; i++)
		{
			if (Tile.zombieTiles[0, i].planted != null) yield return Tile.zombieTiles[0, i].planted.StartCoroutine(methodName);

			if (Tile.plantTiles[1, i].planted != null) yield return Tile.plantTiles[1, i].planted.StartCoroutine(methodName);
			if (Tile.plantTiles[0, i].planted != null) yield return Tile.plantTiles[0, i].planted.StartCoroutine(methodName);
		}
		yield return null;
	}

	public void DisableHandCards()
    {
        foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
		go.interactable = false;
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
                    if (AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type == Card.Type.Trick)
                    {
						if (t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = true;
                    }
                    else t.GetComponent<HandCard>().interactable = false;
				}
			}
		}

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
	}

    public void UpdateRemaining(int change, Team team)
    {
		if (team == this.team)
		{
			remaining += change;
			remainingText.text = remaining + "";
			DisableHandCards();
			EnablePlayableHandCards();
		}
		else
		{
			opponentRemaining += change;
			opponentRemainingText.text = opponentRemaining + "";
		}
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

	public IEnumerator HandleHeroBlocks()
	{
		if (zombieHero.blocked)
		{
			zombieHero.blocked = false;
			waitingOnBlock = true;
			if (team == Team.Zombie)
			{
				GameObject c = Instantiate(handcardPrefab, handCards);
				c.SetActive(false);
				c.transform.localPosition = new Vector2(0, 3);
				c.GetComponent<HandCard>().ID = 10; //temp
				c.GetComponent<HandCard>().interactable = true;
				c.SetActive(true);
			}
			yield return new WaitUntil(() => waitingOnBlock == false);
		}
		yield return CheckDeaths();
		if (plantHero.blocked)
		{
			plantHero.blocked = false;
			waitingOnBlock = true;
			if (team == Team.Plant)
			{
				GameObject c = Instantiate(handcardPrefab, handCards);
				c.SetActive(false);
				c.transform.localPosition = new Vector2(0, 3);
				c.GetComponent<HandCard>().ID = 5; //temp
				c.GetComponent<HandCard>().interactable = true;
				c.SetActive(true);
			}
			yield return new WaitUntil(() => waitingOnBlock == false);
		}
		yield return CheckDeaths();
	}

}
