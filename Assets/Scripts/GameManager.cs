using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
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

	private bool ENDED;
	/// <summary>
	/// Index for which superpower to draw next
	/// </summary>
	private int superpowerIndex = 0;
	/// <summary>
	/// List representation of the current deck's cards, so it can be easily shuffled/drawn from
	/// </summary>
	private List<int> deck = new();
    public int turn { get; private set; }
	/// <summary>
	/// A global count of how many players are ready for the next turn. Only start next turn when this = 2
	/// </summary>
	private int nextTurnReady;

	private float timer;
	private bool timerOn;
	/// <summary>
	/// 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
	/// </summary>
    private int phase;
	/// <summary>
	/// How much gold the player has left this turn
	/// </summary>
    private int remaining = 1;
	/// <summary>
	/// The highest gold the player had this turn
	/// </summary>
    public int remainingTop { get; private set; }
    /// <summary>
    /// How much extra gold the player has for each turn (ex. from Sunburn)
    /// </summary>
    [HideInInspector] public int permanentBonus = 0;
    /// <summary>
    /// How much gold the opponent has left this turn
    /// </summary>
    private int opponentRemaining = 1;
	/// <summary>
	/// The highest gold the opponent had this turn
	/// </summary>
    public int opponentRemainingTop { get; private set; }
    /// <summary>
    /// How much extra gold the opponent has for each turn (ex. from Cryo-brain)
    /// </summary>
    [HideInInspector] public int opponentPermanentBonus = 0;
    /// <summary>
    /// The player's team (Plant/Zombie), derived from their hero's team
    /// </summary>
    public Team team;

	public Image timerImage;
    public Button go;
    private LTDescr goTween;
    public GameObject phaseText;
    private Transform handCards;
    public GameObject handcardPrefab;
    public TextMeshProUGUI remainingText;
	public TextMeshProUGUI opponentRemainingText;
	public GameObject endScreen;
	public TextMeshProUGUI winner;
	public TextMeshProUGUI score;
	public TextMeshProUGUI change;

    /// <summary>
    /// Reference to plant hero script
    /// </summary>
    [HideInInspector] public Hero plantHero;
    /// <summary>
    /// Reference to zombie hero script
    /// </summary>
    [HideInInspector] public Hero zombieHero;
    /// <summary>
    /// Halt game flow if a player blocked. When set to true, game flow immediately resumes
    /// </summary>
    [HideInInspector] public bool waitingOnBlock = false;
    /// <summary>
    /// If a player is selecting a choice (ex. from moving a card), set this to using <c>SelectingChosenRpc</c> so both players can access the selection
    /// </summary>
    public BoxCollider2D selection { get; private set; }
    /// <summary>
    /// Cards will increment this on the same frame of instantiation. Game flow should only continue once this is 0 to handle recusive spawns (ex. from Cornucopia)
    /// </summary>
    [HideInInspector] public int currentlySpawningCards = 0;
    /// <summary>
    /// Reference to a card with frenzy that just attacked. <c>null</c> otherwise
    /// </summary>
    [HideInInspector] public Card frenzyActivate;
    /// <summary>
    /// Set this to true if there's a special circumstance allowing zombies to be played in tricks phase (ex. from Teleport)
    /// </summary>
    [HideInInspector] public bool allowZombieCards = false;
    /// <summary>
    /// Reference to any data that should be shared across the network that can't be achieved normally (ex. Mixed-up Gravedigger)
    /// </summary>
    public List<string> shuffledList { get; private set; }

    /// <summary>
    /// Events with higher priority should be processed first. Those not on the list have no defined ordering
    /// </summary>
    private Dictionary<string, int> priority = new() { 
		{ "OnCardPlay", 0 }, 
		{ "OnBlock", 1 }, 
		{ "OnCardMoved", 2 }, 
		{ "OnCardDeath", 3 }, 
		{ "OnCardFreeze", 4 }, 
		{ "OnCardHurt", 5 }, 
		{ "OnHeroHeal" , 6 }, 
		{ "OnCardHeal" , 7 }, 
		{ "OnCardDraw", 8 } 
	};
    
	/// <summary>
    /// A game event, caused by any card effect or block, with information about the method, arguments, and frame number it was called
    /// </summary>
	public class GameEvent
	{
		public string methodName;
		public object arg;
		public int time;

		public GameEvent(string _methodName, object _arg)
		{
			methodName = _methodName;
			arg = _arg;
			time = Time.frameCount;
		}
	}

    /// <summary>
	/// Events are processed in a stack structure.
	/// Higher priority events should be processed first.
	/// For the same events, the one that happened later should be processed first.
	/// For the same events at the same moment in time, process them left to right.
	/// </summary>
	private List<GameEvent> eventStack = new();
	private bool isProcessing;

    /// <summary>
    /// Load a game event defined by the method name and arguments into the appropriate location in the event stack. It won't be processed until <c>ProcessEvents</c> is called
    /// </summary>
    public void TriggerEvent(string methodName, object arg)
    {
		try
		{
			int i;
			for (i = 0; i < eventStack.Count; i++)
			{
				if (methodName == eventStack[i].methodName && eventStack[i].time < Time.frameCount) continue;
				if (!priority.ContainsKey(methodName)) break;
				if (!priority.ContainsKey(eventStack[i].methodName)) continue;
				if (priority[eventStack[i].methodName] < priority[methodName]) continue;
				if (priority[eventStack[i].methodName] == priority[methodName] && methodName != "OnCardDraw")
				{
					int stackArg;
					if (methodName == "OnCardHurt")
					{
						var temp = (Tuple<Damagable, Card, int, int>)eventStack[i].arg;
						stackArg = temp.Item4 != -1 ? temp.Item4 : temp.Item1.GetComponent<Card>().col;
					}
					else stackArg = ((Damagable)eventStack[i].arg).GetComponent<Card>().col;
					int arg1;
					if (methodName == "OnCardHurt")
					{
						var temp = (Tuple<Damagable, Card, int, int>)arg;
                        arg1 = temp.Item4 != -1 ? temp.Item4 : temp.Item1.GetComponent<Card>().col;
                    }
					else arg1 = ((Damagable)arg).GetComponent<Card>().col;

					if (stackArg > arg1) continue;
					if (stackArg == arg1)
					{
                        Team stackTeam;
						if (methodName == "OnCardHurt")
						{
							var temp = ((Tuple<Damagable, Card, int, int>)eventStack[i].arg).Item1;
							if (temp.GetComponent<Card>() != null) stackTeam = temp.GetComponent<Card>().team;
							else stackTeam = temp.GetComponent<Hero>().team;
                        }
						else stackTeam = ((Damagable)eventStack[i].arg).GetComponent<Card>().team;
                        if (stackTeam == Team.Plant) continue;
					}
				}
				break;
			}
			eventStack.Insert(i, new GameEvent(methodName, arg));
		}
		catch (Exception) { Debug.Log("ERROR " + " " + methodName + " " + arg);}
	}

    /// <summary>
    /// Process all GameEvents in the event stack, one at a time, until it is empty. Player moves will be disabled until it's finished. Multiple ongoing calls will be ignored
    /// </summary>
    public IEnumerator ProcessEvents()
    {
		if (isProcessing || ENDED) yield break;
		isProcessing = true;
		yield return null;
        DisableHandCards();
		
        while (eventStack.Count > 0)
        {
            GameEvent currentEvent = eventStack[^1];
			eventStack.RemoveAt(eventStack.Count - 1);

			if (currentEvent.methodName == "OnBlock")
			{
				yield return HandleHeroBlocks((Hero)currentEvent.arg);
				continue;
			}

            string col = "";
			try { col = ((Card)currentEvent.arg).GetComponent<Card>().col + "";}
			catch (Exception) { }
            Debug.Log(currentEvent.time + " " + currentEvent.methodName + " from " + currentEvent.arg + " at column " + col + " -- Remaining: " + eventStack.Count);
            yield return CallLeftToRight(currentEvent.methodName, currentEvent.arg);

			if (ENDED) yield break;

			yield return new WaitUntil(() => currentlySpawningCards == 0);
			yield return null;
        }
		isProcessing = false;
        EnablePlayableHandCards();
	}

    // Start is called before the first frame update
    void Start()
    {
		var choices = new List<string> { "PitC", "BiiB", "RC", "BS" };
		AudioManager.Instance.PlayMusic(choices[UnityEngine.Random.Range(0, choices.Count)]);
		endScreen.SetActive(false);
		// Setup board structure depending on the player's team
		plantHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.PlantHero]).GetComponent<Hero>();
		zombieHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.ZombieHero]).GetComponent<Hero>();
        if (AllCards.Instance.heroes[UserAccounts.allDecks[UserAccounts.GameStats.DeckName].heroID].team == Team.Plant)
		{
			team = Team.Plant;
			plantHero.transform.position = new Vector2(0, -3.25f);
		    zombieHero.transform.position = new Vector2(0, 3.5f);
            zombieHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
            zombieHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);
		}
		else
		{
			team = Team.Zombie;
			zombieHero.transform.position = new Vector2(0, -3.25f);
			plantHero.transform.position = new Vector2(0, 3.5f);
			plantHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
			plantHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);
        }

		foreach (Transform t in GameObject.Find("Tiles").transform)
		{
            t.GetComponent<Tile>().AssignSide();
		}

		// Load the deck given the deck name, and shuffle
		foreach (int card in UserAccounts.allDecks[UserAccounts.GameStats.DeckName].cards.Keys)
		{
			for (int count = 0; count < UserAccounts.allDecks[UserAccounts.GameStats.DeckName].cards[card]; count++) deck.Add(card);
        }
        for (int n = deck.Count - 1; n > 0; n--)
        {
            int k = UnityEngine.Random.Range(0, n + 1);
            int temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }
		turn = 1;

		StartCoroutine(Mulligan());
    }

	/// <summary>
	/// Draw 4 cards. TODO: Implement the actual mulligan
	/// </summary>
	private IEnumerator Mulligan()
	{
		DrawCard(team, 4);
		GainHandCard(team, UserAccounts.allDecks[UserAccounts.GameStats.DeckName].superpowerOrder[superpowerIndex]);
        yield return ProcessEvents();
		EndRpc();
	}

    void Update()
    {
        if (timerOn)
		{
			timer -= Time.deltaTime;
			timerImage.fillAmount = timer / 30;
            if (timer <= 0)
			{
				timerOn = false;
				timer = 30;
				EndRpc();
			}
		}
    }

    /// <summary>
    /// Draw a card for the player with given team, and triggers the GameEvent
    /// </summary>
    public void DrawCard(Team t, int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
            if (team == t)
			{
				GainHandCard(t, deck[0]);
				deck.RemoveAt(0);
			}
			else TriggerEvent("OnCardDraw", t);
        }
    }

    /// <summary>
    /// Gain a HandCard of the card with given ID for the player with given team, and triggers the GameEvent
    /// </summary>
	/// <param name="fs">If provided, uses these stats for the HandCard, otherwise uses the default</param>
    public void GainHandCard(Team t, int id, FinalStats fs = null)
	{
		if (handCards.childCount >= 10) return;
		if (team == t)
		{
			GameObject c;
			if (AllCards.Instance.cards[id].specialHandCard != null) c = Instantiate(AllCards.Instance.cards[id].specialHandCard, handCards);
            else c = Instantiate(handcardPrefab, handCards);
			c.SetActive(false);
			UpdateHandCardPositions();
			c.GetComponent<HandCard>().ID = id;
			if (fs != null) c.GetComponent<HandCard>().OverrideFS(fs);
			c.SetActive(true);
		}
		TriggerEvent("OnCardDraw", t);
    }

	/// <summary>
	/// Organizes the HandCard layout to be in the 5x2 grid
	/// </summary>
	public void UpdateHandCardPositions()
	{
		int rows = (int)Mathf.Ceil(handCards.childCount / 5f);
		float ypos = 0.4f * (rows-1);
		int index = 0;
		for (int r = 0; r < rows; r++)
		{
			int numThisRow = Mathf.Min(5, handCards.childCount - index);
			for (int i = 0; i < numThisRow; i++, index++)
			{
				handCards.GetChild(index).transform.localPosition = new Vector2(1.2f * (-(numThisRow - 1) / 2f + i), ypos);
			}
			ypos -= 0.8f;
		}
	}

    /// <summary>
    /// Signals to the network that it is ready for the next phase. Transitions to the next phase if possible
    /// </summary>
    [Rpc(SendTo.ClientsAndHost)]
    public void EndRpc()
    {
        StartCoroutine(EndRpcHelper());
    }

    private IEnumerator EndRpcHelper()
    {
        string[] pnames = new string[] { "", "Zombies\nPlay", "Plants\nPlay", "Zombie\nTricks", "FIGHT!" };

		// Only start the next turn when both players are ready
		if (phase == 0 || phase == 4)
		{
			nextTurnReady += 1;
			if (nextTurnReady < 2) yield break;
			nextTurnReady = 0;
		}

        phase += 1;

        phaseText.GetComponent<TextMeshProUGUI>().text = pnames[phase];
        if (goTween != null && LeanTween.isTweening(goTween.id)) LeanTween.cancel(goTween.id);
        phaseText.transform.localScale = Vector3.zero;
        goTween = LeanTween.scale(phaseText, Vector3.one, 0.5f).setEaseOutBack().setOnComplete(() => LeanTween.scale(phaseText, Vector3.zero, 0.5f).setEaseInBack().setDelay(1));

		// Start of zombie tricks: Reveal gravestones
        DisableHandCards();
        if (phase == 3)
        {
			for (int col = 0; col < 5; col++)
			{
				Card c = Tile.zombieTiles[0, col].planted;
				if (c != null && c.gravestone)
				{
					// Update zombie brain UI only for the plant side
					if (c.team != team) UpdateRemaining(-c.playedCost, Team.Zombie);
					yield return c.Reveal();
				}
			}
		}

		timer = 30;
        if (phase == 4) StartCoroutine(Combat());
		else EnablePlayableHandCards();
    }

	/// <summary>
	/// Begin combat phase, then updates game state afterwards
	/// </summary>
	/// <returns></returns>
    private IEnumerator Combat()
    {
		StartCoroutine(AudioManager.Instance.ToggleBattleMusic(true));
        yield return new WaitForSeconds(1);

		// Cards attack left to right
        for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted != null) yield return Tile.zombieTiles[0, col].planted.Attack();

            if (ENDED) yield break;

            if (Tile.plantTiles[1, col].planted != null) yield return Tile.plantTiles[1, col].planted.Attack();
			if (Tile.plantTiles[0, col].planted != null) yield return Tile.plantTiles[0, col].planted.Attack();

			yield return ProcessEvents();

            if (ENDED) yield break;

            // Recursively handle frenzy if applicable
            while (frenzyActivate != null)
			{
				Card temp = frenzyActivate;
				frenzyActivate = null;
				yield return temp.Attack();
                yield return ProcessEvents();
			}

            if (ENDED) yield break;

            // Handle doublestrike if applicable
            if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.doubleStrike) yield return Tile.zombieTiles[0, col].planted.Attack();

			if (Tile.plantTiles[1, col].planted != null && Tile.plantTiles[1, col].planted.doubleStrike) yield return Tile.plantTiles[1, col].planted.Attack();
			if (Tile.plantTiles[0, col].planted != null && Tile.plantTiles[0, col].planted.doubleStrike) yield return Tile.plantTiles[0, col].planted.Attack();

            if (ENDED) yield break;

            yield return ProcessEvents();
        }

        TriggerEvent("OnTurnEnd", null);
        yield return ProcessEvents();

		// Any invulnerable objects lose invulnerability at end of turn
		plantHero.ToggleInvulnerability(false);
		zombieHero.ToggleInvulnerability(false);
		for (int col = 0; col < 5; col++)
		{
			for (int row = 0; row < 2; row++)
			{
                if (Tile.plantTiles[row, col].HasRevealedPlanted()) Tile.plantTiles[row, col].planted.ToggleInvulnerability(false);
                if (Tile.zombieTiles[row, col].HasRevealedPlanted()) Tile.zombieTiles[row, col].planted.ToggleInvulnerability(false);
			}
		}

        // Setup for next turn
        StartCoroutine(AudioManager.Instance.ToggleBattleMusic(false));
        turn += 1;
        remaining = turn + permanentBonus;
		opponentRemaining = turn + opponentPermanentBonus;
		UpdateRemaining(0, Team.Plant);
		UpdateRemaining(0, Team.Zombie);
		phase = 0;
		allowZombieCards = false;
		
		TriggerEvent("OnTurnStart", null);
        yield return ProcessEvents();

        DrawCard(Team.Zombie);
        DrawCard(Team.Plant);

		yield return ProcessEvents();
		EndRpc();
    }

    /// <summary>
    /// Sends a unit to be played through the network under the given FinalStats, row, and column. Uses the card's team to decide which side to plant it on
    /// </summary>
    /// <param name="fs">The played card's stats</param>
    /// <param name="free">If true, doesn't deduct from this player's remaining gold</param>
    [Rpc(SendTo.ClientsAndHost)]
    public void PlayCardRpc(FinalStats fs, int row, int col, bool free=false)
    {
		if (free) fs.cost = 0;
		Card card = AllCards.Instance.cards[fs.ID];
        if (card.team != team)
        {
			// From the opponent's perspective, only deduct the gold UI if it's not a gravestone
			if (!card.gravestone) UpdateRemaining(-fs.cost, card.team);
        }
        else
        {
            // From the player's perspective, always deduct the gold UI
            UpdateRemaining(-fs.cost, team);
        }
		
		card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
		card.sourceFS = fs;

        if (card.team == Team.Zombie) Tile.zombieTiles[row, col].Plant(card);
        else Tile.plantTiles[row, col].Plant(card);

        // Disable after 1 card play by default
        allowZombieCards = false;
    }

    /// <summary>
    /// Sends a trick to be played through the network under the given FinalStats, row, and column. If targeting a hero, set row/column to -1
    /// </summary>
    /// <param name="fs">The played card's stats</param>
    /// <param name="isPlantTarget">Whether the given row/column represents the plant or zombie side of the board</param>
    [Rpc(SendTo.ClientsAndHost)]
	public void PlayTrickRpc(FinalStats fs, int row, int col, bool isPlantTarget)
	{
		Card card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
		card.row = row;
		card.col = col;
		card.sourceFS = fs;
		if (!isPlantTarget)
		{
            if (row == -1 && col == -1) card.transform.position = zombieHero.transform.position;
            else card.transform.position = Tile.zombieTiles[row, col].transform.position;
		}
		else
		{
            if (row == -1 && col == -1) card.transform.position = plantHero.transform.position;
            else card.transform.position = Tile.plantTiles[row, col].transform.position;
		}

		UpdateRemaining(-fs.cost, card.team);
    }

	/// <summary>
	/// Signals to the network that the given team's player who blocked chose to keep the superpower. Triggers a card draw GameEvent and allows the game flow to continue
	/// </summary>
	/// <param name="t"></param>
	[Rpc(SendTo.ClientsAndHost)]
	public void HoldTrickRpc(Team t)
	{
        waitingOnBlock = false;
		TriggerEvent("OnCardDraw", t);
	}

    /// <summary>
    /// Signals to the network to move the unit with the given row/column and team to this new row/column. Triggers a card moved GameEvent
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
    /// <param name="row">Old row</param>
    /// <param name="col">Old column</param>
    /// <param name="nrow">New row</param>
    /// <param name="ncol">New column</param>
    [Rpc(SendTo.ClientsAndHost)]
	public void MoveRpc(Team tteam, int row, int col, int nrow, int ncol)
	{
		Card c;
		if (tteam == Team.Plant)
		{
			c = Tile.plantTiles[row, col].planted;
			Tile.plantTiles[row, col].Unplant();
			Tile.plantTiles[nrow, ncol].Plant(c);
		}
		else
		{
			c = Tile.zombieTiles[row, col].planted;
			Tile.zombieTiles[row, col].Unplant();
			Tile.zombieTiles[nrow, ncol].Plant(c);
		}
		Debug.Log("Moving " + c + " from Row " + row + " Col " + col + " to Row " + nrow + " Col " + ncol);
		TriggerEvent("OnCardMoved", c);
    }

    /// <summary>
    /// Signals to the network to freeze the unit with the given row/column and team. Triggers a card freeze GameEvent
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
    [Rpc(SendTo.ClientsAndHost)]
    public void FreezeRpc(Team tteam, int row, int col)
    {
        if (tteam == Team.Plant) Tile.plantTiles[row, col].planted.Freeze();
        else Tile.zombieTiles[row, col].planted.Freeze();
    }

    /// <summary>
    /// Signals to the network to make the unit with the given row/column and team to do a bonus attack
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
    [Rpc(SendTo.ClientsAndHost)]
    public void BonusAttackRpc(Team tteam, int row, int col)
    {
        if (tteam == Team.Plant) StartCoroutine(Tile.plantTiles[row, col].planted.Attack());
        else StartCoroutine(Tile.zombieTiles[row, col].planted.Attack());
    }

    /// <summary>
    /// Signals to the network that the selecting player selected this tile as their selection choice. If targeting a hero, set row/column to -1
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
    [Rpc(SendTo.ClientsAndHost)]
    public void SelectingChosenRpc(Team tteam, int row, int col)
    {
        if (tteam == Team.Plant) 
		{
			if (row == -1 && col == -1) selection = plantHero.GetComponent<BoxCollider2D>();
			else selection = Tile.plantTiles[row, col].GetComponent<BoxCollider2D>();
        }
		else
		{
            if (row == -1 && col == -1) selection = zombieHero.GetComponent<BoxCollider2D>();
            else selection = Tile.zombieTiles[row, col].GetComponent<BoxCollider2D>();
        }
    }

    /// <summary>
	/// Calls the GameEvent coroutine with the given name and arguments for each card and HandCard that currently exists, from left to right, column 1 to 0
	/// </summary>
	/// <param name="methodName"></param>
	/// <param name="arg"></param>
	/// <returns></returns>
    private IEnumerator CallLeftToRight(string methodName, object arg)
	{
		for (int i = 0; i < 5; i++)
		{
			if (Tile.zombieTiles[0, i].planted != null) yield return Tile.zombieTiles[0, i].planted.StartCoroutine(methodName, arg);

			if (Tile.plantTiles[1, i].planted != null) yield return Tile.plantTiles[1, i].planted.StartCoroutine(methodName, arg);
			if (Tile.plantTiles[0, i].planted != null) yield return Tile.plantTiles[0, i].planted.StartCoroutine(methodName, arg);
		}
		foreach (Transform h in handCards) h.GetComponent<HandCard>().StartCoroutine(methodName, arg);
        yield return null;
	}

	/// <summary>
	/// Disables all HandCards and the end turn button
	/// </summary>
	public void DisableHandCards()
    {
		timerOn = false;
        timerImage.gameObject.SetActive(false);
        UpdateHandCardPositions();
        foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
		go.interactable = false;
	}

    /// <summary>
    /// Enables only the specific HandCards and the end turn button with respect to the game's rules of the current phase
    /// </summary>
    public void EnablePlayableHandCards()
    {
		if (team == Team.Plant)
		{
            if (phase == 2)
            {
				foreach (Transform t in handCards)
				{
					if (t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = true;
					else t.GetComponent<HandCard>().interactable = false;
                }
            }
            else foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
		}
		else
		{
            if (phase == 1)
            {
                foreach (Transform t in handCards)
                {
                    if (AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type == Card.Type.Unit && t.GetComponent<HandCard>().GetCost() <= remaining)
						t.GetComponent<HandCard>().interactable = true;
                    else t.GetComponent<HandCard>().interactable = false;
                }
            }
            else if (phase == 3)
            {
				foreach (Transform t in handCards)
				{
                    if ((AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type == Card.Type.Trick || allowZombieCards) &&
						t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = true;
                    else t.GetComponent<HandCard>().interactable = false;
				}
            }
            else foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
        }

		if (team == Team.Plant)
		{
			if (phase == 2)
			{
				go.interactable = true;
                timerImage.gameObject.SetActive(true);
                timerOn = true;
			}
			else go.interactable = false;
		}
		else
		{
			if (phase == 1 || phase == 3)
			{
				go.interactable = true;
                timerImage.gameObject.SetActive(true);
                timerOn = true;
            }
			else go.interactable = false;
		}
	}

	/// <summary>
	/// Adds to the current team's gold count by the given change. Gold counts can't go below 0. Updates UI
	/// </summary>
    public void UpdateRemaining(int change, Team team)
    {
		if (team == this.team)
		{
			remaining += change;
			remaining = Mathf.Max(remaining, 0);
			remainingText.text = remaining + "";
			remainingTop = Mathf.Max(remaining, remainingTop);
		}
		else
		{
			opponentRemaining += change;
            opponentRemaining = Mathf.Max(opponentRemaining, 0);
            opponentRemainingText.text = opponentRemaining + "";
			opponentRemainingTop = Mathf.Max(opponentRemaining, opponentRemainingTop);
		}
    }

    /// <summary>
    /// Signals to the network to raise the attack of the unit with the given row/column and team by this amount
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
    [Rpc(SendTo.ClientsAndHost)]
	public void RaiseAttackRpc(Team tteam, int row, int col, int amount)
	{
        if (tteam == Team.Plant) Tile.plantTiles[row, col].planted.RaiseAttack(amount);
        else Tile.zombieTiles[row, col].planted.RaiseAttack(amount);
    }

    /// <summary>
    /// Signals to the network to raise the HP of the unit with the given row/column and team
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
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

    /// <summary>
    /// Called when a block GameEvent is being processed. Makes a 0-cost superpower HandCard, update superpower index, and waits for the given hero to make a decision
    /// </summary>
    public IEnumerator HandleHeroBlocks(Hero h)
	{
		waitingOnBlock = true;
		if (team == h.team)
		{
			superpowerIndex += 1;
			GameObject c = Instantiate(handcardPrefab, handCards);
			c.SetActive(false);
			c.transform.localPosition = new Vector2(0, 3);
			HandCard hc = c.GetComponent<HandCard>();
			hc.ID = UserAccounts.allDecks[UserAccounts.GameStats.DeckName].superpowerOrder[superpowerIndex];
            hc.interactable = true;
			FinalStats fs = new FinalStats(hc.ID);
			fs.cost = 0;
			hc.OverrideFS(fs);
			c.SetActive(true);
		}
		yield return new WaitUntil(() => waitingOnBlock == false);
	}

    /// <summary>
    /// Signals to the network to store data for any future use. Must be provided in a " - " separated string (since that's the only way to serialize it...)
    /// </summary>
    [Rpc(SendTo.ClientsAndHost)]
    public void StoreRpc(string list)
    {
		string[] list1 = list.Split(" - ");
		shuffledList = new(list1);
    }

    /// <summary>
    /// The given winner has won the game. Update player's score on the leaderboard or add if this is new
    /// </summary>
    public async void GameEnded(Team won)
    {
		ENDED = true;
		
		winner.text = (won == Team.Plant ? "PLANTS" : "ZOMBIES") + " WIN";
		int oldScore = 0;
		int newScore = 0;
		try
		{
			var existingScore = await LeaderboardsService.Instance.GetPlayerScoreAsync("devplayers");
			oldScore = (int)existingScore.Score;
			if (team == won)
			{
				// If this is the winning team, raise score by 25
				var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync("devplayers", oldScore + 25);
				newScore = (int)scoreResponse.Score;
			}
			else
			{
				// If this is the losing team, lower score by 25 unless they are in Wood tier
				if (existingScore.Tier == "Wood") newScore = oldScore;
				else
				{
					var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync("devplayers", oldScore - 15);
					newScore = (int)scoreResponse.Score;
				}
            }
		}
		catch (LeaderboardsException e)
		{
			// This player isn't on the leaderboard, so if they are the winner, add an entry of 25
			if (team == won && e.Reason == LeaderboardsExceptionReason.EntryNotFound)
			{
                var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync("devplayers", 25);
                newScore = (int)scoreResponse.Score;
            }
		}
		score.text = newScore + "";
		change.text = "(" + (newScore - oldScore > 0 ? "+" : "") + (newScore - oldScore) + ")";
		if (newScore - oldScore > 0) change.color = Color.green;
        else if (newScore == oldScore) change.color = Color.gray;
        else change.color = Color.red;
		endScreen.SetActive(true);

		UserAccounts.Instance.UpdateCachedScore();
    }

    /// <summary>
    /// Retrieves a list of all existing HandCards for the current player
    /// </summary>
    public List<HandCard> GetHandCards()
	{
		List<HandCard> ret = new();
		foreach (Transform t in handCards) ret.Add(t.GetComponent<HandCard>());
		return ret;
	}

	public void ClearSelection()
	{
		selection = null;
	}

}
