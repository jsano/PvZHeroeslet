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
        playerBuffs = transform.Find("UI").Find("PlayerBuffs");
        opponentBuffs = transform.Find("UI").Find("OpponentBuffs");
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
	private bool timerBOn;
	[HideInInspector] public int turnTimerMax = 30;
    [HideInInspector] public int mulliganTimerMax = 15;
    [HideInInspector] public int blockTimerMax = 10;
    [HideInInspector] public int buffTimerMax = 15;

    /// <summary>
    /// How many buff rerolls the player has
    /// </summary>
    [HideInInspector] public int rerolls = 1;
    /// <summary>
    /// The current randomized selection of buffs to offer each player. Only one player should fill these in each time so the second attempt should be ignored
    /// </summary>
    private List<int> buffChoices = new();
    /// <summary>
    /// What buffs were finalized for each player. Index 0 is player, index 1 is opponent
    /// </summary>
    private int[] chosenBuff = new int[2];
	private List<int> availableBuffDatabase = new();

	/// <summary>
	/// 0 = prep, 1 = zombie, 2 = plant, 3 = zombie trick, 4 = fight
	/// </summary>
    public int phase { get; private set; }
	/// <summary>
	/// How much gold the player has left this turn
	/// </summary>
    public float remaining { get; private set; }
	/// <summary>
	/// The highest gold the player had this turn
	/// </summary>
    public float remainingTop { get; private set; }
    
    /// <summary>
    /// How much gold the opponent has left this turn
    /// </summary>
    public float opponentRemaining { get; private set; }
	/// <summary>
	/// The highest gold the opponent had this turn
	/// </summary>
    public float opponentRemainingTop { get; private set; }
	/// <summary>
    /// How much extra gold the player has for each turn (ex. from Sunburn)
    /// </summary>
    [HideInInspector] public int playerPermanentBonus = 0;
    /// <summary>
    /// How much extra gold the opponent has for each turn (ex. from Cryo-brain)
    /// </summary>
    [HideInInspector] public int opponentPermanentBonus = 0;
    /// <summary>
    /// How much extra attack the player cards have for the rest of the game
    /// </summary>
    [HideInInspector] public int playerPermanentAttackBonus = 0;
    /// <summary>
    /// How much extra HP the player cards have for the rest of the game
    /// </summary>
    [HideInInspector] public int playerPermanentHPBonus = 0;
    /// <summary>
    /// How much extra attack the opponent cards have for the rest of the game
    /// </summary>
    [HideInInspector] public int opponentPermanentAttackBonus = 0;
    /// <summary>
    /// How much extra HP the opponent cards have for the rest of the game
    /// </summary>
    [HideInInspector] public int opponentPermanentHPBonus = 0;
    /// <summary>
    /// How much less gold the player units cost for the rest of the game
    /// </summary>
    [HideInInspector] public float playerUnitPermanentDiscount = 0;
    /// <summary>
    /// How much less gold the player tricks cost for the rest of the game
    /// </summary>
    [HideInInspector] public float playerTrickPermanentDiscount = 0;
    /// <summary>
    /// For literally just Clique Peas only
    /// </summary>
    [HideInInspector] public int playerCliquePeas = 0;
    /// <summary>
    /// For literally just Clique Peas only
    /// </summary>
    [HideInInspector] public int opponentCliquePeas = 0;
    /// <summary>
    /// The player's team (A/B)
    /// </summary>
    public Team team;

	public GameObject mulliganUI;
	public Transform laneHighlight;
	public SpriteRenderer boardHighlight;
	public Image timerImageM;
	public Image timerImage;
	public Image timerImageB;
	public GameObject buffSelectionUI;
	public Transform buffList;
	public GameObject buffListing;
	public TextMeshProUGUI rerollText;
	public Button lockInButton;
	public Button rerollButton;
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
	public GameObject remainingAnim;
	public GameObject opponentRemainingAnim;
	public GameObject endScreen;
	public TextMeshProUGUI winner;
	public TextMeshProUGUI score;
	public TextMeshProUGUI change;
	public GameObject waiting;

    /// <summary>
    /// Reference to plant hero script
    /// </summary>
    [HideInInspector] public Hero playerHero;
    /// <summary>
    /// Reference to zombie hero script
    /// </summary>
    [HideInInspector] public Hero opponentHero;
    /// <summary>
    /// Halt game flow if a player blocked. When set to null, game flow immediately resumes
    /// </summary>
    [HideInInspector] public Hero waitingOnBlock = null;
	private bool blockChoiceMade = false;
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
    public List<List<string>> shuffledLists { get; private set; }
    [HideInInspector] public int shuffledListsNextExpectedCount = 1;
    /// <summary>
    /// For literally just Sun Strike only
    /// </summary>
    public List<Card> removeStrikethrough = new();

	public Transform playerBuffs { get; private set; }
    public Transform opponentBuffs { get; private set; }

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
		{ "OnCardStatsChanged", 8 },
		{ "OnCardBonusAttack", 9 },
		{ "OnCardDraw", 10 } 
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
                        if (stackTeam == GetOpponent(WentFirst())) continue;
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
    public IEnumerator ProcessEvents(bool combatVersion = false, bool ignoreSpawning = false)
    {
		if (isProcessing || ENDED) yield break;
		isProcessing = true;
		yield return null;
        DisableHandCards();
		if (!ignoreSpawning) yield return new WaitUntil(() => currentlySpawningCards == 0); // For remaining OnThisPlays
        int ignored = 0;
        while (eventStack.Count > ignored)
        {
			GameEvent currentEvent = eventStack[^(ignored + 1)];
			if (combatVersion)
			{
				if (currentEvent.methodName == "OnCardHurt" || currentEvent.methodName == "OnCardDeath" || currentEvent.methodName == "OnBlock")
				{
					ignored += 1;
					continue;
				}
			}
			eventStack.RemoveAt(eventStack.Count - 1 - ignored);

			if (currentEvent.methodName == "OnBlock")
			{
				Buff.CallAllImmediate("OnBlock", (Hero)currentEvent.arg);
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

        if (!ignoreSpawning) EnablePlayableHandCards();
	}

    // Start is called before the first frame update
    void Start()
    {
		var choices = new List<string> { "PitC", "BiiB", "RC", "BS" };
		AudioManager.Instance.PlayMusic(choices[UnityEngine.Random.Range(0, choices.Count)]);
		endScreen.SetActive(false);
		// Setup board structure depending on the player's team
		team = UserAccounts.GameStats.team;
        if (team == Team.B)
		{
			playerHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.BHero]).GetComponent<Hero>();
			opponentHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.AHero]).GetComponent<Hero>();
        }
		else
		{
            playerHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.AHero]).GetComponent<Hero>();
            opponentHero = Instantiate(AllCards.Instance.heroes[UserAccounts.GameStats.BHero]).GetComponent<Hero>();
        }
		playerHero.transform.position = new Vector2(0, -3.25f);
        opponentHero.transform.position = new Vector2(0, 3.7f);
        opponentHero.GetComponent<SpriteRenderer>().sortingOrder = -1;
        opponentHero.transform.Find("HeroUI").position *= new Vector2(-1, 1);
		playerHero.team = team;
		opponentHero.team = GetOpponent(team);
		remaining = 1;
		opponentRemaining = 1;

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

		shuffledLists = new();

		if (UserAccounts.GameStats.Buffs != null) foreach (int i in UserAccounts.GameStats.Buffs) availableBuffDatabase.Add(i);
        else for (int i = 0; i < AllCards.Instance.buffs.Length; i++) availableBuffDatabase.Add(i);

		StartCoroutine(Mulligan());
    }

	private IEnumerator Mulligan()
	{
        StartCoroutine(DrawCard(GetOpponent(team), 4, false));
        StartCoroutine(GainHandCard(GetOpponent(team), 0, null, false, false));

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
		timer = mulliganTimerMax;
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

		yield return OfferBuffs();

        yield return GainHandCard(team, UserAccounts.allDecks[UserAccounts.GameStats.DeckName].superpowerOrder[superpowerIndex], null, false);
        yield return ProcessEvents();
        StartCoroutine(UpdateRemaining(0, Team.A, false));
        yield return UpdateRemaining(0, Team.B, false);
        EndRpc(team);
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
        AudioManager.Instance.PlaySFX("Go");
        timerMOn = false;
		mulliganed = true;
	}

	void Update()
	{
		if (timerOn)
		{
			timer -= Time.deltaTime;
			timerImage.fillAmount = 0.18f + 0.64f * (waitingOnBlock ? timer / blockTimerMax : timer / turnTimerMax);
			if (timer <= 0)
			{
				timerOn = false;
				timer = turnTimerMax;
				if (waitingOnBlock) HoldTrickRpc(team);
				else EndRpc(team, true);
			}
		}
		else
		{
			if (waitingOnBlock == null) if (phase == 3 && team != WentFirst() || phase == 2 && team == WentFirst() || phase == 1 && team != WentFirst()) {
				opponentHero.ToggleThinking(true);
            }
		}
		if (timerMOn)
		{
			timer -= Time.deltaTime;
			timerImageM.fillAmount = timer / mulliganTimerMax;
			if (timer <= 0)
			{
				timerMOn = false;
				FinishMulligan();
			}
		}
        if (timerBOn)
        {
            timer -= Time.deltaTime;
            timerImageB.fillAmount = timer / buffTimerMax;
            if (timer <= 0)
            {
                timerBOn = false;
				BuffSelection.current = buffChoices[0];
				LockIn();
            }
        }
        if (opponentPlayedQueue.Count == 0 && phase == 3 || waitingOnBlock && !blockChoiceMade)
		{
			plantCombatBehindBy = Math.Max(plantCombatBehindBy - Time.deltaTime, 0);
		}
	}

    /// <summary>
    /// Draw a card for the player with given team, and triggers the GameEvent
    /// </summary>
    public IEnumerator DrawCard(Team t, int count = 1, bool animation = true)
	{
		if (Buff.PlayerHasBuff("Altered Expectations", t))
		{
			if (phase == 0) count = 0;
			else count += 1;
		}
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
			if (handCards.childCount >= 10 || Buff.PlayerHasBuff("Suffocating Limits", GetOpponent(t)) && handCards.childCount >= 8) yield break;
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
			if (opponentHandCards.childCount >= 10 || Buff.PlayerHasBuff("Suffocating Limits", GetOpponent(t)) && opponentHandCards.childCount >= 8) yield break;
			int current = opponentHandCards.childCount;
			c = Instantiate(cardBackPrefab, opponentHandCards);
			c.transform.SetSiblingIndex(current);
            c.transform.localPosition = new Vector2(-2.5f + current * 0.5f, 0);
			c.GetComponent<SpriteRenderer>().sortingOrder = current;
            if (t == Team.B) c.GetComponent<SpriteRenderer>().sprite = AllCards.Instance.zombieCardBack;
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
				if (LeanTween.isTweening(handCards.GetChild(index).gameObject)) continue;
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

	public void EndButton()
	{
		EndRpc(team);
	}

    /// <summary>
    /// Signals to the network that it is ready for the next phase. Transitions to the next phase if possible
    /// </summary>
    [Rpc(SendTo.ClientsAndHost)]
    public void EndRpc(Team sourceTeam, bool timeOut = false)
    {
		if (timeOut) if (Buff.PlayerHasBuff("Thin Patience", GetOpponent(sourceTeam))) StartCoroutine(GetTeamHero(sourceTeam).ReceiveDamage(5, null, true));
        StartCoroutine(EndRpcHelper(sourceTeam));
    }

    private IEnumerator EndRpcHelper(Team sourceTeam)
    {
		yield return new WaitUntil(() => opponentPlayedQueue.Count == 0 && isProcessing == false);

		string[] pnames = new string[] { "", "Initiative\nPlay", "Reactive\nPlay", "Initiative\nTricks", "FIGHT!" };

		// Only start the next turn when both players are ready
		if (phase == 0 || phase == 4)
		{
			nextTurnReady += 1;
			if (nextTurnReady < 2)
			{
				if (team == sourceTeam)
				{
					yield return new WaitForSeconds(0.5f);
					if (phase == 0) waiting.SetActive(true);
				}
				yield break;
			}
			nextTurnReady = 0;
		}
		waiting.SetActive(false);

        AudioManager.Instance.PlaySFX("Go");
        playerHero.ToggleThinking(false);
        opponentHero.ToggleThinking(false);
        phase += 1;

        phaseText.GetComponent<TextMeshProUGUI>().text = pnames[phase];
		for (int i = 1; i <= 4; i++) phaseIcons.GetChild(i).GetComponent<Image>().color = Color.gray;
		phaseIcons.GetChild(phase).GetComponent<Image>().color = Color.white;
        if (goTween != null && LeanTween.isTweening(goTween.id)) LeanTween.cancel(goTween.id);
        phaseText.transform.localScale = Vector3.zero;
        goTween = LeanTween.scale(phaseText, Vector3.one, 0.5f).setEaseOutBack().setOnComplete(() => LeanTween.scale(phaseText, Vector3.zero, 0.5f).setEaseInBack().setDelay(1));

		// Start of initiative tricks: Reveal their gravestones
        DisableHandCards();
        if (phase == 3)
        {
			for (int row = 0; row < Tile.ROWS; row++) for (int col = 0; col < Tile.COLUMNS; col++)
			{
				Card c = Tile.GetTeamTiles(WentFirst())[row, col].planted;
				if (c != null && c.gravestone)
				{
					yield return c.Reveal();
                    DisableHandCards();
                }
				if (c != null) yield return c.OnZombieTricks();
			}
		}

		// Start of the reactive player's next turn: Reveal their gravestones
        if (phase == 1)
        {
            for (int row = 0; row < Tile.ROWS; row++) for (int col = 0; col < Tile.COLUMNS; col++)
            {
                Card c = Tile.GetTeamTiles(WentFirst())[row, col].planted;
                if (c != null && c.gravestone)
                {
                    yield return c.Reveal();
                    DisableHandCards();
                }
            }
        }

        timer = turnTimerMax;
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
        for (int col = 0; col < Tile.COLUMNS; col++)
		{
			laneHighlight.position = new Vector3(Tile.playerTiles[0, col].transform.position.x, 0, 0);

			if (Tile.terrainTiles[col].planted != null) yield return Tile.terrainTiles[col].planted.BeforeCombat();
			yield return ProcessEvents();

			Tile[,] initiate = Tile.GetTeamTiles(WentFirst());
			Tile[,] retaliate = Tile.GetTeamTiles(GetOpponent(WentFirst()));
			int savedHP1 = -1;
			int savedHP2 = -1;
			if (retaliate[1, col].planted != null) savedHP2 = retaliate[1, col].planted.HP;
            if (retaliate[0, col].planted != null) savedHP1 = retaliate[0, col].planted.HP;

            if (initiate[1, col].planted != null)
            {
                yield return initiate[1, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (initiate[1, col].planted != null) yield return initiate[1, col].planted.Attack();
            }
            if (initiate[0, col].planted != null)
			{
                yield return initiate[0, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (initiate[0, col].planted != null) yield return initiate[0, col].planted.Attack();
			}

            if (ENDED) yield break;

			if (retaliate[1, col].planted != null)
			{
                yield return retaliate[1, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (retaliate[1, col].planted != null) yield return retaliate[1, col].planted.Attack(savedHP2);
			}
			if (retaliate[0, col].planted != null)
			{
                yield return retaliate[0, col].planted.BeforeCombat();
                yield return ProcessEvents(true);
                if (retaliate[0, col].planted != null) yield return retaliate[0, col].planted.Attack(savedHP1);
			}

			yield return ProcessEvents();

            if (ENDED) yield break;

            if (Tile.terrainTiles[col].planted != null) yield return Tile.terrainTiles[col].planted.AfterCombat();
            yield return ProcessEvents();

            if (initiate[1, col].planted != null)
            {
                yield return initiate[1, col].planted.AfterCombat();
                yield return ProcessEvents(true);
            }
            if (initiate[0, col].planted != null)
            {
                yield return initiate[0, col].planted.AfterCombat();
                yield return ProcessEvents(true);
            }

            if (ENDED) yield break;

            if (retaliate[1, col].planted != null)
            {
                yield return retaliate[1, col].planted.AfterCombat();
                yield return ProcessEvents(true);
            }
            if (retaliate[0, col].planted != null)
            {
                yield return retaliate[0, col].planted.AfterCombat();
                yield return ProcessEvents(true);
            }

            if (ENDED) yield break;

            // Handle doublestrike if applicable
            if (initiate[1, col].planted != null && initiate[1, col].planted.doubleStrike > 0) yield return initiate[1, col].planted.BonusAttack();
            if (initiate[0, col].planted != null && initiate[0, col].planted.doubleStrike > 0) yield return initiate[0, col].planted.BonusAttack();

			if (retaliate[1, col].planted != null && retaliate[1, col].planted.doubleStrike > 0) yield return retaliate[1, col].planted.BonusAttack();
			if (retaliate[0, col].planted != null && retaliate[0, col].planted.doubleStrike > 0) yield return retaliate[0, col].planted.BonusAttack();

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
        playerHero.ToggleInvulnerability(false);
		opponentHero.ToggleInvulnerability(false);
		for (int col = 0; col < 5; col++)
		{
			for (int row = 0; row < 2; row++)
			{
                if (Tile.playerTiles[row, col].HasRevealedPlanted()) Tile.playerTiles[row, col].planted.ToggleInvulnerability(false);
                if (Tile.opponentTiles[row, col].HasRevealedPlanted()) Tile.opponentTiles[row, col].planted.ToggleInvulnerability(false);
			}
		}
		foreach (Card c in removeStrikethrough) c.strikethrough -= 1;
		removeStrikethrough.Clear();

		if (turn % 2 == 0) yield return OfferBuffs();

		Buff.CallAllImmediate("AfterTurnEndBeforeTurnStart", null);

        // Setup for next turn
        StartCoroutine(AudioManager.Instance.ToggleBattleMusic(false));
        turn += 1;
        if (!Buff.PlayerHasBuff("Nervous Laughter", team)) remaining = 0;
        if (!Buff.PlayerHasBuff("Nervous Laughter", GetOpponent(team))) opponentRemaining = 0;
		yield return UpdateRemaining(0, Team.A, false);
		yield return UpdateRemaining(0, Team.B, false);
        StartCoroutine(UpdateRemaining(turn + playerPermanentBonus, team, false));
		yield return UpdateRemaining(turn + opponentPermanentBonus, GetOpponent(team), false);
		phase = 0;
		allowZombieCards = false;
		
		TriggerEvent("OnTurnStart", null);
        yield return ProcessEvents();

		bool wait = false;
		// If at least 1 side drew, this is how long it should theoretically take. TODO: fix?
		if (handCards.childCount < 10 || opponentHandCards.childCount < 10) wait = true;
		StartCoroutine(DrawCard(Team.B));
        StartCoroutine(DrawCard(Team.A));
		if (wait) yield return new WaitForSeconds(1);

		yield return ProcessEvents();
		EndRpc(team);
		plantCombatBehindBy = 0;
		shuffledLists.Clear();
		shuffledListsNextExpectedCount = 1;
    }

	private IEnumerator OfferBuffs(bool notFirst = false)
	{
        foreach (Transform child in buffList) Destroy(child.gameObject);
		// TODO: duos
		List<int> temp = new();
		for (int i = 0; i < 3 && availableBuffDatabase.Count > temp.Count; i++)
		{
			int cur;
			do
			{
				cur = availableBuffDatabase[UnityEngine.Random.Range(0, availableBuffDatabase.Count)];
			}
			while (temp.Contains(cur));
			temp.Add(cur);
		}
		while (temp.Count < 4) temp.Add(-1);
        OfferBuffsRpc(IsHost, temp[0], temp[1], temp[2], temp[3]);
		waiting.SetActive(true);
		yield return new WaitUntil(() => buffChoices.Count > 0 && buffChoices[^1] == -69);
        waiting.SetActive(false);
        chosenBuff = new int[] { -1, -1 };
		if (buffChoices.Count == 1) yield break;//lockInButton.interactable = true; // No options
		buffSelectionUI.SetActive(true);
		rerollText.text = rerolls + " remaining";
		rerollButton.interactable = rerolls > 0;
        foreach (int b in buffChoices)
		{
			if (b < 0) break;
			GameObject g = Instantiate(buffListing, buffList);
			g.GetComponent<BuffSelection>().ID = b;
		}
		timerBOn = true;
		timer = buffTimerMax;

		if (notFirst) yield break;
		yield return new WaitUntil(() => chosenBuff[0] != -1 && chosenBuff[1] != -1);
        buffSelectionUI.SetActive(false);

        Buff b1 = Instantiate(AllCards.Instance.buffs[chosenBuff[0]], playerBuffs);
        b1.team = team;
        Buff b2 = Instantiate(AllCards.Instance.buffs[chosenBuff[1]], opponentBuffs);
        b2.team = GetOpponent(team);
        availableBuffDatabase.Remove(chosenBuff[0]);
        availableBuffDatabase.Remove(chosenBuff[1]);
        buffChoices.Clear();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void OfferBuffsRpc(bool isHost, int a, int b, int c, int d)
	{
		if (isHost)
		{
            if (a != -1) buffChoices.Add(a);
			if (a != -1) buffChoices.Add(b);
			if (a != -1) buffChoices.Add(c);
			if (a != -1) buffChoices.Add(d);
			buffChoices.Add(-69);
		}
    }

	public void RerollBuffs()
	{
		rerolls -= 1;
		rerollButton.interactable = rerolls > 0;
		RerollBuffsRpc();
    }

	[Rpc(SendTo.ClientsAndHost)]
	public void RerollBuffsRpc()
	{
		buffChoices.Clear();
		lockInButton.interactable = false;
        StartCoroutine(OfferBuffs(true));
	}

	public void LockIn()
	{
        AudioManager.Instance.PlaySFX("Go");
        lockInButton.interactable = false;
		timerBOn = false;
		LockInRpc(IsHost, BuffSelection.current);
	}

    [Rpc(SendTo.ClientsAndHost)]
    public void LockInRpc(bool host, int chosen)
	{
		int index = IsHost == host ? 0 : 1;
		chosenBuff[index] = chosen;
	}

    /// <summary>
    /// Sends a unit to be played through the network under the given FinalStats, row, and column. Uses the card's team to decide which side to plant it on
    /// </summary>
    /// <param name="fs">The played card's stats</param>
    /// <param name="playingTeam">Which player is playing this card and sent this RPC </param>
    [Rpc(SendTo.ClientsAndHost)]
    public void PlayCardRpc(FinalStats fs, int row, int col, Team playingTeam)
    {
		if (playingTeam == team) PlayCardHelper(fs, row, col, playingTeam);
        else if (waitingOnBlock) PlayCardHelper(fs, row, col, playingTeam);
        else
		{
			opponentPlayedQueue.Add((fs, row, col, false));
			StartCoroutine(ProcessOpponentPlayedQueue());
		}
    }

	private void PlayCardHelper(FinalStats fs, int row, int col, Team playingTeam)
	{
		Card card = AllCards.Instance.cards[fs.ID];
		if (playingTeam != team)
		{
			Destroy(opponentHandCards.GetChild(opponentHandCards.childCount - 1).gameObject);
		}
		
		card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
		card.sourceFS = fs;
		card.team = playingTeam;

		Tile.GetTeamTiles(card.team)[row, col].Plant(card);

		// Disable after 1 card play by default
		allowZombieCards = false;
	}

    /// <summary>
    /// Sends a trick to be played through the network under the given FinalStats, row, and column. If targeting a hero, set row/column to -1
    /// </summary>
    /// <param name="fs">The played card's stats</param>
    /// <param name="isTargetingTeamA">Whether the given row/column represents Team A's side of the board</param>
	/// <param name="playingTeam">Which player is playing this card and sent this RPC </param>
    [Rpc(SendTo.ClientsAndHost)]
	public void PlayTrickRpc(FinalStats fs, int row, int col, bool isTargetingTeamA, Team playingTeam)
	{
		if (playingTeam == team) PlayTrickHelper(fs, row, col, isTargetingTeamA, playingTeam);
		else if (waitingOnBlock)
		{
			blockChoiceMade = true;
			StartCoroutine(OpponentTrickAnimation(fs, row, col, isTargetingTeamA, playingTeam));
		}
		else
		{
			opponentPlayedQueue.Add((fs, row, col, isTargetingTeamA));
			StartCoroutine(ProcessOpponentPlayedQueue());
		}
    }

	private IEnumerator OpponentTrickAnimation(FinalStats fs, int row, int col, bool isTargetingTeamA, Team playingTeam)
	{
		if (phase >= 3) plantCombatBehindBy += 2f;
		GameObject hc = Instantiate(handcardPrefab, opponentHandCards.position, Quaternion.identity);
		var hc1 = hc.GetComponent<HandCard>();
		hc1.ID = fs.ID;
		hc1.OverrideFS(fs);
		yield return null;
		hc1.ShowInfo();
        Vector3 oldScale = hc.transform.localScale;
        hc.transform.localScale = Vector3.one * 0.5f;

		Vector2 dest;
        if (isTargetingTeamA)
        {
			if (row == -1 && col == -1) dest = team == Team.A ? playerHero.transform.position : opponentHero.transform.position;
			else if (row == 2) dest = Tile.terrainTiles[col].transform.position;
			else dest = Tile.GetTeamTiles(Team.A)[row, col].transform.position;
        }
        else
        {
            if (row == -1 && col == -1) dest = team == Team.B ? playerHero.transform.position : opponentHero.transform.position;
            else if (row == 2) dest = Tile.terrainTiles[col].transform.position;
            else dest = Tile.GetTeamTiles(Team.B)[row, col].transform.position;
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
		PlayTrickHelper(fs, row, col, isTargetingTeamA, playingTeam);
    }

	private void PlayTrickHelper(FinalStats fs, int row, int col, bool isTargetingTeamA, Team playingTeam)
	{
		Card card = Instantiate(AllCards.Instance.cards[fs.ID]).GetComponent<Card>();
		card.row = row;
		card.col = col;
		card.sourceFS = fs;
		
		if (row == 2) // Terrain
        {
            card.transform.position = Tile.terrainTiles[col].transform.position;
            if (card.type == Card.Type.Terrain) Tile.terrainTiles[col].Plant(card);
        }
		else
		{
			if (isTargetingTeamA)
			{
				if (row == -1 && col == -1) card.transform.position = team == Team.A ? playerHero.transform.position : opponentHero.transform.position;
                else card.transform.position = Tile.GetTeamTiles(Team.A)[row, col].transform.position;
			}
			else
			{
				if (row == -1 && col == -1) card.transform.position = team == Team.B ? playerHero.transform.position : opponentHero.transform.position;
                else card.transform.position = Tile.GetTeamTiles(Team.B)[row, col].transform.position;
			}
		}
        card.team = playingTeam;
        if (card.team != team) Destroy(opponentHandCards.GetChild(opponentHandCards.childCount - 1).gameObject);
    }

	private IEnumerator ProcessOpponentPlayedQueue()
	{
		if (isProcessingOpponentQueue) yield break;
		while (opponentPlayedQueue.Count > 0)
		{
			isProcessingOpponentQueue = true;
			var cur = opponentPlayedQueue[0];
			if (AllCards.Instance.cards[((FinalStats)cur[0]).ID].type == Card.Type.Unit) PlayCardHelper((FinalStats)cur[0], (int)cur[1], (int)cur[2], GetOpponent(team));
			else yield return OpponentTrickAnimation((FinalStats)cur[0], (int)cur[1], (int)cur[2], (bool)cur[3], GetOpponent(team));
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
		TriggerEvent("OnCardDraw", t);
		if (team == t) handCards.GetChild(handCards.childCount - 1).GetComponent<HandCard>().ChangeCost(1);
        var to = opponentHandCards.TransformPoint(-2.5f + (opponentHandCards.childCount - 1) * 0.5f, 0, 0);
		LeanTween.move(opponentHandCards.GetChild(opponentHandCards.childCount - 1).gameObject, to, 0.5f).setEaseOutQuint();
		waitingOnBlock = null;
	}

    /// <summary>
    /// Signals to the network that the selecting player selected this tile as their selection choice. If targeting a hero, set row/column to -1
    /// </summary>
    /// <param name="tteam">Whether the given row/column represents the plant or zombie side of the board</param>
    [Rpc(SendTo.ClientsAndHost)]
    public void SelectingChosenRpc(Team tteam, int row, int col)
    {
        if (tteam == team) 
		{
			if (row == -1 && col == -1) selection = playerHero.GetComponent<BoxCollider2D>();
			else if (row == 2) selection = Tile.terrainTiles[col].GetComponent<BoxCollider2D>();
            else selection = Tile.playerTiles[row, col].GetComponent<BoxCollider2D>();
        }
		else
		{
            if (row == -1 && col == -1) selection = opponentHero.GetComponent<BoxCollider2D>();
            else if (row == 2) selection = Tile.terrainTiles[col].GetComponent<BoxCollider2D>();
            else selection = Tile.opponentTiles[row, col].GetComponent<BoxCollider2D>();
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
        Tile[,] initiate = Tile.GetTeamTiles(WentFirst());
        Tile[,] retaliate = Tile.GetTeamTiles(GetOpponent(WentFirst()));
        for (int i = 0; i < Tile.COLUMNS; i++)
		{
			if (Tile.terrainTiles[i].planted != null) toDo.Add(Tile.terrainTiles[i].planted);

            if (initiate[1, i].HasRevealedPlanted()) toDo.Add(initiate[1, i].planted);
            if (initiate[0, i].HasRevealedPlanted()) toDo.Add(initiate[0, i].planted);
			
			if (retaliate[1, i].HasRevealedPlanted()) toDo.Add(retaliate[1, i].planted);
			if (retaliate[0, i].HasRevealedPlanted()) toDo.Add(retaliate[0, i].planted);
		}
		foreach (Card c in toDo) if (c != null) yield return c.StartCoroutine(methodName, arg);
		foreach (Transform h in handCards) h.GetComponent<HandCard>().StartCoroutine(methodName, arg);
		Transform firstBuffs = team == WentFirst() ? playerBuffs : opponentBuffs;
        Transform secondBuffs = team == WentFirst() ? opponentBuffs : playerBuffs;
		for (int i = 0; i < Math.Max(playerBuffs.childCount, opponentBuffs.childCount); i++)
		{
			if (i < firstBuffs.childCount) yield return firstBuffs.GetChild(i).GetComponent<Buff>().StartCoroutine(methodName, arg);
            if (i < secondBuffs.childCount) yield return secondBuffs.GetChild(i).GetComponent<Buff>().StartCoroutine(methodName, arg);
        }
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
		if (team != WentFirst())
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
                    if ((AllCards.Instance.cards[t.GetComponent<HandCard>().ID].type != Card.Type.Unit || allowZombieCards || Tile.IsOnField("Teleportation", team) && WentFirst() == team) &&
						t.GetComponent<HandCard>().GetCost() <= remaining) t.GetComponent<HandCard>().interactable = true;
                    else t.GetComponent<HandCard>().interactable = false;
				}
            }
            else foreach (Transform t in handCards) t.GetComponent<HandCard>().interactable = false;
        }

		if (team != WentFirst())
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
	/// <param name="generated"> Whether this was generated by a card or merely systemwide </param>
	public IEnumerator UpdateRemaining(float change, Team team, bool generated = true)
	{
		if (change > 0 && generated && Buff.PlayerHasBuff("Money Hungry", team)) change += 0.5f;
		GameObject c = team == this.team ? remainingAnim : opponentRemainingAnim;
		Vector3 orig = ((RectTransform)c.transform).anchoredPosition;
		c.GetComponentInChildren<TextMeshProUGUI>().text = change + "";
		if (change > 0)
		{
			if (team == Team.A) AudioManager.Instance.PlaySFX("Sun");
			else AudioManager.Instance.PlaySFX("Brain");
			bool done = false;
			c.GetComponent<Image>().color = Color.white;
			var t = c.GetComponentInChildren<TextMeshProUGUI>();
			t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
			((RectTransform)c.transform).anchoredPosition = team == this.team ? new(-100, -50f) : new(100, 50f);
			var lt = LeanTween.moveX((RectTransform)c.transform, team == this.team ? 400 : -400, 0.5f).setEaseOutQuint().setOnComplete(() =>
			{
				var dest = team == this.team ? ((RectTransform)remainingText.transform.parent).anchoredPosition : ((RectTransform)opponentRemainingText.transform.parent).anchoredPosition;
				LeanTween.move((RectTransform)c.transform, dest, 0.5f).setDelay(0.25f).setEaseOutQuad().setOnComplete(() => done = true);
			});
			yield return new WaitUntil(() => done == true);
		}
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
		if (change < 0)
		{
			LeanTween.moveY((RectTransform)c.transform, ((RectTransform)c.transform).anchoredPosition.y + 75, 0.5f).setEaseOutQuad().setOnComplete(() =>
			{
                ((RectTransform)c.transform).anchoredPosition = orig;
			});
			var i = c.GetComponent<Image>();
			var t = c.GetComponentInChildren<TextMeshProUGUI>();
			while (i.color.a > 0)
			{
				i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - Time.deltaTime * 2);
				t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime * 2);
				yield return null;
			}
			i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
			t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
		}
	}

    /// <summary>
    /// Called when a block GameEvent is being processed. Makes a 0-cost superpower HandCard, update superpower index, and waits for the given hero to make a decision
    /// </summary>
    public IEnumerator HandleHeroBlocks(Hero h)
	{
        playerHero.ToggleThinking(false);
        opponentHero.ToggleThinking(false);
        h.ToggleThinking(true);
        h.ResetBlock();
		waitingOnBlock = h;
		blockChoiceMade = false;
		if (team == h.team)
		{
			superpowerIndex += 1;
			GameObject c = Instantiate(handcardPrefab, handCards);
			c.SetActive(false);
			c.transform.localPosition = new Vector3(0, 4, -2);
			HandCard hc = c.GetComponent<HandCard>();
			hc.ID = UserAccounts.allDecks[UserAccounts.GameStats.DeckName].superpowerOrder[superpowerIndex];
            hc.interactable = true;
			FinalStats fs = new(hc.ID);
			fs.cost = 0;
			hc.OverrideFS(fs);
			c.SetActive(true);

            timerImage.gameObject.SetActive(true);
            timerOn = true;
			timer = blockTimerMax;
		}
		else
		{
            int current = opponentHandCards.childCount;
            GameObject c = Instantiate(cardBackPrefab, opponentHandCards);
            c.transform.SetSiblingIndex(current);
			c.transform.localPosition = new Vector2(0, -3);
            c.GetComponent<SpriteRenderer>().sortingOrder = current;
            if (h.team == Team.B) c.GetComponent<SpriteRenderer>().sprite = AllCards.Instance.zombieCardBack;
        }
		yield return new WaitUntil(() => waitingOnBlock == null);
        playerHero.ToggleThinking(false);
        opponentHero.ToggleThinking(false);
    }

    /// <summary>
    /// Signals to the network to store data for any future use. Must be provided in a " - " separated string (since that's the only way to serialize it...)
    /// </summary>
    [Rpc(SendTo.ClientsAndHost)]
    public void StoreRpc(string list)
    {
		string[] list1 = list.Split(" - ");
		shuffledLists.Add(new(list1));
    }

	public List<string> GetShuffledList()
	{
		return shuffledLists[shuffledListsNextExpectedCount - 2];
    }

    /// <summary>
    /// The given winner has won the game. Update player's score on the leaderboard or add if this is new
    /// </summary>
    public async void GameEnded(Team won)
    {
        AudioManager.Instance.PlaySFX("Dead");
        ENDED = true;
		
		winner.text = "YOU" + (won == team ? "WIN" : "LOSE");
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

	/// <summary>
	/// On turn 1, Team A goes first. Then on turn 2, Team B goes first. And so on
	/// </summary>
	/// <returns></returns>
	public Team WentFirst()
	{
		return turn % 2 == 0 ? Team.B : Team.A;
	}

    /// <summary>
    /// Gets the hero that corresponds to the given team
    /// </summary>
    public Hero GetTeamHero(Card.Team team)
    {
        if (team == this.team) return playerHero;
        return opponentHero;
    }

}
