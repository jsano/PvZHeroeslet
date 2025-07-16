using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
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
        opponentHandCards = transform.Find("OpponentHandCards");
        turn = 1;
	}

	private bool mulliganed;
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
    /// <summary>
    /// A queue of how many opponent cards have yet to play due to previous animations. The GO button won't be enabled while this is in use
    /// </summary>
    private List<ITuple> opponentPlayedQueue = new();
	private bool isProcessingOpponentQueue;
    /// <summary>
    /// How many seconds behind the plant side is during the combat phase due to zombie trick animations. This needs to be made up in some way
    /// </summary>
    private float plantCombatBehindBy;

	private float timer;
	private bool timerOn;
	private bool timerMOn;
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
    /// How much extra attack the plants have for the rest of the game
    /// </summary>
    [HideInInspector] public int plantPermanentAttackBonus = 0;
    /// <summary>
    /// How much extra HP the plants have for the rest of the game
    /// </summary>
    [HideInInspector] public int plantPermanentHPBonus = 0;
    /// <summary>
    /// How much extra attack the zombies have for the rest of the game
    /// </summary>
    [HideInInspector] public int zombiePermanentAttackBonus = 0;
    /// <summary>
    /// How much extra HP the zombies have for the rest of the game
    /// </summary>
    [HideInInspector] public int zombiePermanentHPBonus = 0;
    /// <summary>
    /// The player's team (Plant/Zombie), derived from their hero's team
    /// </summary>
    public Team team;

	public GameObject mulliganUI;
	public Transform laneHighlight;
	public SpriteRenderer boardHighlight;
	public Image timerImageM;
	public Image timerImage;
    public Button go;
    private LTDescr goTween;
    public GameObject phaseText;
	public Transform phaseIcons;
    private Transform handCards;
    public GameObject handcardPrefab;
	public Transform opponentHandCards { get; private set; }
	public GameObject cardBackPrefab;
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
		{ "OnCardBonusAttack", 8 },
		{ "OnCardDraw", 9 } 
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
                    else if (methodName == "OnCardDeath") stackArg = ((Tuple<Card, Card>)eventStack[i].arg).Item1.GetComponent<Card>().col;
                    else stackArg = ((Damagable)eventStack[i].arg).GetComponent<Card>().col;
					int arg1;
					if (methodName == "OnCardHurt")
					{
						var temp = (Tuple<Damagable, Card, int, int>)arg;
                        arg1 = temp.Item4 != -1 ? temp.Item4 : temp.Item1.GetComponent<Card>().col;
                    }
                    else if (methodName == "OnCardDeath") arg1 = ((Tuple<Card, Card>)arg).Item1.GetComponent<Card>().col;
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
                        else if (methodName == "OnCardDeath") stackTeam = ((Tuple<Card, Card>)eventStack[i].arg).Item1.GetComponent<Card>().team;
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
    public IEnumerator ProcessEvents(bool combatVersion = false)
    {
		if (isProcessing || ENDED) yield break;
		isProcessing = true;
		yield return null;
        DisableHandCards();
		yield return new WaitUntil(() => currentlySpawningCards == 0); // For remaining OnThisPlays
        int ignored = 0;
        while (eventStack.Count > ignored)
        {
			GameEvent currentEvent = eventStack[^(ignored + 1)];
			if (combatVersion)
			{
				if (currentEvent.methodName == "OnCardHurt" || currentEvent.methodName == "OnCardDeath")
				{
					ignored += 1;
					continue;
				}
			}
			eventStack.RemoveAt(eventStack.Count - 1 - ignored);

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

            //yield return new WaitUntil(() => currentlySpawningCards == 0); maybe remove???
            shuffledList = null;
			yield return null;
        }
		isProcessing = false;

		if (!combatVersion)
		{
			// Recursively handle frenzy if applicable
			while (frenzyActivate != null)
			{
				Card temp = frenzyActivate;
				frenzyActivate = null;
				yield return temp.BonusAttack();
				yield return ProcessEvents(combatVersion);
			}
		}

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
            remainingText.transform.parent.GetComponent<Image>().sprite = AllCards.Instance.brainUI;
            opponentRemainingText.transform.parent.GetComponent<Image>().sprite = AllCards.Instance.sunUI;
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

	private IEnumerator Mulligan()
	{
        StartCoroutine(DrawCard(team == Team.Plant ? Team.Zombie : Team.Plant, 4, false));
        StartCoroutine(GainHandCard(team == Team.Plant ? Team.Zombie : Team.Plant, 0, null, false, false));

		Vector2[] pos = new Vector2[] { new(-1, 1), new(1, 1), new(-1, -1), new(1, -1) };
        for (int i = 0; i < 4; i++)
        {
            GameObject c = Instantiate(handcardPrefab, handCards);
            c.GetComponent<HandCard>().ID = deck[0];
            c.transform.position = pos[i];
            c.transform.localScale = new Vector3(1.1f, 1.1f, 1);
            c.GetComponent<SpriteRenderer>().sortingLayerName = "Error";
            c.GetComponent<Canvas>().sortingLayerName = "Error";
            deck.RemoveAt(0);
        }
		timerMOn = true;
		timer = 15;
		yield return new WaitUntil(() => mulliganed == true);

		mulliganUI.SetActive(false);
		UpdateHandCardPositions();
		bool done = false;
		for (int i = 0; i < 4; i++)
		{
			GameObject c = handCards.GetChild(i).gameObject;
			Vector3 oldPos = c.transform.position;
			c.transform.position = pos[i];
			c.transform.localScale = new Vector3(1.1f, 1.1f, 1);
			var lt = LeanTween.move(c, oldPos, 0.5f).setEaseOutQuint().setDelay(0.5f).setOnComplete(() => done = true);
			LeanTween.scale(c, handcardPrefab.transform.localScale, 0.5f).setEaseOutQuint().setDelay(0.5f);
            c.GetComponent<SpriteRenderer>().sortingLayerName = "HandCard";
            c.GetComponent<Canvas>().sortingLayerName = "HandCard";
        }
		yield return new WaitUntil(() => done == true);

        yield return GainHandCard(team, UserAccounts.allDecks[UserAccounts.GameStats.DeckName].superpowerOrder[superpowerIndex], null, false);
        yield return ProcessEvents();
        UpdateRemaining(0, Team.Plant);
        UpdateRemaining(0, Team.Zombie);
        EndRpc();
	}

	public void ReplaceMulliganCard(Transform t)
	{
        AudioManager.Instance.PlaySFX("Draw Card");
        int index = t.GetSiblingIndex();
		Transform hc = handCards.GetChild(index);
		deck.Insert(UnityEngine.Random.Range(4, deck.Count), hc.GetComponent<HandCard>().ID);
        GameObject c = Instantiate(handcardPrefab, handCards);
        c.GetComponent<HandCard>().ID = deck[0];
        c.transform.position = hc.position;
        c.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        c.transform.SetSiblingIndex(index);
        c.GetComponent<SpriteRenderer>().sortingLayerName = "Error";
        c.GetComponent<Canvas>().sortingLayerName = "Error";
        deck.RemoveAt(0);
		Destroy(hc.gameObject);
    }

	public void FinishMulligan()
	{
		mulliganed = true;
	}

	void Update()
	{
		if (timerOn)
		{
			timer -= Time.deltaTime;
			timerImage.fillAmount = 0.18f + 0.64f * (waitingOnBlock ? timer / 10 : timer / 30);
			if (timer <= 0)
			{
				timerOn = false;
				timer = 30;
				if (waitingOnBlock) HoldTrickRpc(team);
				else EndRpc();
			}
		}
		if (timerMOn)
		{
            timer -= Time.deltaTime;
            timerImageM.fillAmount = timer / 15;
            if (timer <= 0)
            {
                timerMOn = false;
                FinishMulligan();
            }
        }
	}

    /// <summary>
    /// Draw a card for the player with given team, and triggers the GameEvent
    /// </summary>
    public IEnumerator DrawCard(Team t, int count = 1, bool animation = true)
	{
		for (int i = 0; i < count; i++)
		{
            if (team == t && handCards.childCount < 10) deck.RemoveAt(0);
			yield return GainHandCard(t, deck[0], null, false, animation);
        }
    }

    /// <summary>
    /// Gain a HandCard of the card with given ID for the player with given team, and triggers the GameEvent
    /// </summary>
	/// <param name="fs">If provided, uses these stats for the HandCard, otherwise uses the default</param>
	/// <param name="animation">Whether to include the drawing animation or just appear</param>
    public IEnumerator GainHandCard(Team t, int id, FinalStats fs = null, bool conjured = true, bool animation = true)
	{
		GameObject c = null;
		if (team == t)
		{
			if (handCards.childCount >= 10) yield break;
			if (AllCards.Instance.cards[id].specialHandCard != null) c = Instantiate(AllCards.Instance.cards[id].specialHandCard, handCards);
            else c = Instantiate(handcardPrefab, handCards);
			c.SetActive(false);
			UpdateHandCardPositions();
			c.GetComponent<HandCard>().ID = id;
			if (fs != null) c.GetComponent<HandCard>().OverrideFS(fs);
			c.GetComponent<HandCard>().conjured = conjured;
            c.SetActive(true);
		} 
		else
		{
			if (opponentHandCards.childCount >= 10) yield break;
			int current = opponentHandCards.childCount;
			c = Instantiate(cardBackPrefab, opponentHandCards);
			c.transform.SetSiblingIndex(current);
            c.transform.localPosition = new Vector2(-2.5f + current * 0.5f, 0);
			c.GetComponent<SpriteRenderer>().sortingOrder = current;
            if (t == Team.Zombie) c.GetComponent<SpriteRenderer>().sprite = AllCards.Instance.zombieCardBack;
        }
        TriggerEvent("OnCardDraw", t);
		if (animation)
		{
			AudioManager.Instance.PlaySFX("Draw Card");
			Vector3 oldScale = c.transform.localScale;
			Vector3 oldPos = c.transform.position;
			c.transform.position = new Vector2(0, team == t ? -0.5f : 0.5f);
			c.transform.localScale = new Vector3(1.1f, 1.1f, 1);
			bool done = false;
			var lt = LeanTween.move(c, oldPos, 0.5f).setEaseOutQuint().setDelay(0.5f).setOnComplete(() => done = true);
			LeanTween.scale(c, oldScale, 0.5f).setEaseOutQuint().setDelay(0.5f);
			yield return new WaitUntil(() => done == true);
		}
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
    /// Shuffles a list of card IDs into the player's deck
    /// </summary>
    public void ShuffleIntoDeck(Team t, List<int> toAdd)
	{
		if (team == t) foreach (int i in toAdd)
		{
			deck.Insert(UnityEngine.Random.Range(0, deck.Count + 1), i);
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
		yield return new WaitUntil(() => opponentPlayedQueue.Count == 0);

        string[] pnames = new string[] { "", "Zombies\nPlay", "Plants\nPlay", "Zombie\nTricks", "FIGHT!" };

		// Only start the next turn when both players are ready
		if (phase == 0 || phase == 4)
		{
			nextTurnReady += 1;
			if (nextTurnReady < 2) yield break;
			nextTurnReady = 0;
		}

        AudioManager.Instance.PlaySFX("Go");
        phase += 1;

        phaseText.GetComponent<TextMeshProUGUI>().text = pnames[phase];
		for (int i = 1; i <= 4; i++) phaseIcons.GetChild(i).GetComponent<Image>().color = Color.gray;
		phaseIcons.GetChild(phase).GetComponent<Image>().color = Color.white;
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
                    DisableHandCards();
                }
				if (c != null) yield return c.OnZombieTricks();
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
        AudioManager.Instance.PlaySFX("Combat");
        StartCoroutine(AudioManager.Instance.ToggleBattleMusic(true));
		if (plantCombatBehindBy > 0) plantCombatBehindBy -= 1;
        else yield return new WaitForSeconds(1);

		laneHighlight.gameObject.SetActive(true);
		// Cards attack left to right
        for (int col = 0; col < 5; col++)
		{
			laneHighlight.position = new Vector3(Tile.plantTiles[0, col].transform.position.x, 0, 0);

			if (Tile.terrainTiles[col].planted != null) yield return Tile.terrainTiles[col].planted.BeforeCombat();
			yield return ProcessEvents();

			if (Tile.zombieTiles[0, col].planted != null)
			{
                yield return Tile.zombieTiles[0, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (Tile.zombieTiles[0, col].planted != null) yield return Tile.zombieTiles[0, col].planted.Attack();
			}

            if (ENDED) yield break;

			if (Tile.plantTiles[1, col].planted != null)
			{
                yield return Tile.plantTiles[1, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (Tile.plantTiles[1, col].planted != null) yield return Tile.plantTiles[1, col].planted.Attack();
			}
			if (Tile.plantTiles[0, col].planted != null)
			{
                yield return Tile.plantTiles[0, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (Tile.plantTiles[0, col].planted != null) yield return Tile.plantTiles[0, col].planted.Attack();
			}

			yield return ProcessEvents();

            if (ENDED) yield break;

            // Handle doublestrike if applicable
            if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.doubleStrike > 0) yield return Tile.zombieTiles[0, col].planted.BonusAttack();

			if (Tile.plantTiles[1, col].planted != null && Tile.plantTiles[1, col].planted.doubleStrike > 0) yield return Tile.plantTiles[1, col].planted.BonusAttack();
			if (Tile.plantTiles[0, col].planted != null && Tile.plantTiles[0, col].planted.doubleStrike > 0) yield return Tile.plantTiles[0, col].planted.BonusAttack();

            if (ENDED) yield break;

            yield return ProcessEvents();
			yield return new WaitForSeconds(0.1f);
        }
        laneHighlight.gameObject.SetActive(false);

        TriggerEvent("OnTurnEnd", null);
        yield return ProcessEvents();

        if (plantCombatBehindBy > 0) plantCombatBehindBy -= 1;
        else yield return new WaitForSeconds(1);

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

		bool wait = false;
		// If at least 1 side drew, this is how long it should theoretically take. TODO: fix?
		if (handCards.childCount < 10 || opponentHandCards.childCount < 10) wait = true;
		StartCoroutine(DrawCard(Team.Zombie));
        StartCoroutine(DrawCard(Team.Plant));
		if (wait) yield return new WaitForSeconds(1);

		yield return ProcessEvents();
		EndRpc();
		plantCombatBehindBy = 0;
    }

    /// <summary>
    /// Sends a unit to be played through the network under the given FinalStats, row, and column. Uses the card's team to decide which side to plant it on
    /// </summary>
    /// <param name="fs">The played card's stats</param>
    /// <param name="fromHandCard">If true, adds to the network spawning queue.
	/// Otherwise, assumes it was called by another card and doesn't deduct from this player's remaining gold</param>
    [Rpc(SendTo.ClientsAndHost)]
    public void PlayCardRpc(FinalStats fs, int row, int col, bool fromHandCard=false)
    {
		if (!fromHandCard) fs.cost = 0;
		if (AllCards.Instance.cards[fs.ID].team == team) PlayCardHelper(fs, row, col);
		else
		{
			opponentPlayedQueue.Add((fs, row, col, false));
			StartCoroutine(ProcessOpponentPlayedQueue());
		}
    }

	private void PlayCardHelper(FinalStats fs, int row, int col)
	{
		Card card = AllCards.Instance.cards[fs.ID];
		if (card.team != team)
		{
			Destroy(opponentHandCards.GetChild(opponentHandCards.childCount - 1).gameObject);
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
		if (AllCards.Instance.cards[fs.ID].team == team) PlayTrickHelper(fs, row, col, isPlantTarget);
		else
		{
			opponentPlayedQueue.Add((fs, row, col, isPlantTarget));
			StartCoroutine(ProcessOpponentPlayedQueue());
		}
    }

	private IEnumerator OpponentTrickAnimation(FinalStats fs, int row, int col, bool isPlantTarget)
	{
		if (phase == 3) plantCombatBehindBy += 2f;
		GameObject hc = Instantiate(handcardPrefab, opponentHandCards.position, Quaternion.identity);
		var hc1 = hc.GetComponent<HandCard>();
		hc1.ID = fs.ID;
		yield return null;
		hc1.ShowInfo();
        Vector3 oldScale = hc.transform.localScale;
        hc.transform.localScale = Vector3.one * 0.5f;

		Vector2 dest;
        if (!isPlantTarget)
        {
			if (row == -1 && col == -1) dest = zombieHero.transform.position;
			else if (row == 2) dest = Tile.terrainTiles[col].transform.position;
			else dest = Tile.zombieTiles[row, col].transform.position;
        }
        else
        {
            if (row == -1 && col == -1) dest = plantHero.transform.position;
            else if (row == 2) dest = Tile.terrainTiles[col].transform.position;
            else dest = Tile.plantTiles[row, col].transform.position;
        }
        bool done = false;
        LeanTween.move(hc, new Vector2(0, 0), 0.5f).setEaseOutQuad().setOnComplete(() => done = true);
        LeanTween.scale(hc, oldScale, 0.5f).setEaseOutQuad();
        yield return new WaitUntil(() => done == true);
		yield return new WaitForSeconds(1.25f);
		done = false;
        LeanTween.move(hc, dest, 0.25f).setOnComplete(() => done = true);
        LeanTween.scale(hc, Vector2.one * 0.25f, 0.25f);
        yield return new WaitUntil(() => done == true);
		Destroy(hc);
		PlayTrickHelper(fs, row, col, isPlantTarget);
    }

	private void PlayTrickHelper(FinalStats fs, int row, int col, bool isPlantTarget)
	{
		Card card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
		card.row = row;
		card.col = col;
		card.sourceFS = fs;
		
		if (row == 2) // Terrain
        {
            card.transform.position = Tile.terrainTiles[col].transform.position;
            Tile.terrainTiles[col].Plant(card);
        }
		else
		{
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
		}

        if (card.team != team) Destroy(opponentHandCards.GetChild(opponentHandCards.childCount - 1).gameObject);
		UpdateRemaining(-fs.cost, card.team);
	}

	private IEnumerator ProcessOpponentPlayedQueue()
	{
		if (isProcessingOpponentQueue) yield break;
		while (opponentPlayedQueue.Count > 0)
		{
			isProcessingOpponentQueue = true;
			var cur = opponentPlayedQueue[0];
			if (AllCards.Instance.cards[((FinalStats)cur[0]).ID].type == Card.Type.Unit) PlayCardHelper((FinalStats)cur[0], (int)cur[1], (int)cur[2]);
			else yield return OpponentTrickAnimation((FinalStats)cur[0], (int)cur[1], (int)cur[2], (bool)cur[3]);
			yield return new WaitUntil(() => isProcessingOpponentQueue == false);
			opponentPlayedQueue.RemoveAt(0);
        }
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
		if (team == t) handCards.GetChild(handCards.childCount - 1).GetComponent<HandCard>().ChangeCost(1);
        var to = opponentHandCards.TransformPoint(-2.5f + (opponentHandCards.childCount - 1) * 0.5f, 0, 0);
		LeanTween.move(opponentHandCards.GetChild(opponentHandCards.childCount - 1).gameObject, to, 0.5f).setEaseOutQuint();
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
        if (tteam == Team.Plant) StartCoroutine(Tile.plantTiles[row, col].planted.BonusAttack());
        else StartCoroutine(Tile.zombieTiles[row, col].planted.BonusAttack());
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
		List<Card> toDo = new(); // Need it all at the beginning or else cards that move to the right call multiple times
		for (int i = 0; i < 5; i++)
		{
			if (Tile.terrainTiles[i].planted != null) toDo.Add(Tile.terrainTiles[i].planted);

			if (Tile.zombieTiles[0, i].HasRevealedPlanted()) toDo.Add(Tile.zombieTiles[0, i].planted);
			
			if (Tile.plantTiles[1, i].planted != null) toDo.Add(Tile.plantTiles[1, i].planted);
			if (Tile.plantTiles[0, i].planted != null) toDo.Add(Tile.plantTiles[0, i].planted);
		}
		foreach (Card c in toDo) yield return c.StartCoroutine(methodName, arg);
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
                    if ((AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type != Card.Type.Unit || allowZombieCards || Tile.IsOnField("Teleportation")) &&
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

        isProcessingOpponentQueue = false;
    }

	/// <summary>
	/// Adds to the given team's gold count by the given change. Gold counts can't go below 0. Updates UI
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
    /// Called when a block GameEvent is being processed. Makes a 0-cost superpower HandCard, update superpower index, and waits for the given hero to make a decision
    /// </summary>
    public IEnumerator HandleHeroBlocks(Hero h)
	{
		h.ResetBlock();
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

            timerImage.gameObject.SetActive(true);
            timerOn = true;
			timer = 10;
		}
		else
		{
            int current = opponentHandCards.childCount;
            GameObject c = Instantiate(cardBackPrefab, opponentHandCards);
            c.transform.SetSiblingIndex(current);
			c.transform.localPosition = new Vector2(0, -3);
            c.GetComponent<SpriteRenderer>().sortingOrder = current;
            if (h.team == Team.Zombie) c.GetComponent<SpriteRenderer>().sprite = AllCards.Instance.zombieCardBack;
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
        AudioManager.Instance.PlaySFX("Dead");
        ENDED = true;
		
		winner.text = (won == Team.Plant ? "PLANTS" : "ZOMBIES") + " WIN";
		if (SessionManager.Instance.ActiveSession.Properties["Ranked"].Value == "True")
		{
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
		}
		else
		{
			score.text = "--";
			change.text = "";
		}
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
		ret.Reverse();
		return ret;
	}

	public void ClearSelection()
	{
		selection = null;
	}

}
