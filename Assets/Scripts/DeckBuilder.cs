using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{

    public class Deck
    {
        public int heroID;
        public List<int> superpowerOrder = new();
        public Dictionary<int, int> cards = new();

        public Deck(int hero)
        {
            heroID = hero;
            foreach (Card c in AllCards.Instance.heroes[hero].superpowers) superpowerOrder.Add(AllCards.NameToID(c.name));
        }
    }

    public static string deckName;
    private Deck deck;
    public Transform deckCards;
    public Transform allDeckCards;
    public GameObject deckCardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = UserAccounts.allDecks[deckName];

        foreach (int id in deck.cards.Keys)
        {
            for (int i = 0; i < deck.cards[id]; i++)
            {
                DeckCard d = Instantiate(deckCardPrefab, deckCards).GetComponent<DeckCard>();
                d.ID = id;
                d.hideButtons = true;
            }
        }

        for (int i = 0; i < AllCards.Instance.cards.Length; i++)
        {
            Card c = AllCards.Instance.cards[i];
            if (!c.token && !c.tribes.Contains(Card.Tribe.Superpower) && (AllCards.Instance.heroes[deck.heroID].classes[0] == c._class || AllCards.Instance.heroes[deck.heroID].classes[1] == c._class))
            {
                DeckCard d = Instantiate(deckCardPrefab, allDeckCards).GetComponent<DeckCard>();
                d.ID = i;
            }
        }
    }

    public void Add(int id)
    {
        if (deck.cards.TryGetValue(id, out int count))
        {
            deck.cards[id] = count + 1;
            if (count + 1 >= 4) GetDeckCard(id).add.interactable = false;
        }
        else deck.cards[id] = 1;
        DeckCard d = Instantiate(deckCardPrefab, deckCards).GetComponent<DeckCard>();
        d.ID = id;
        d.hideButtons = true;
    }

    public void Remove(int id)
    {
        GetDeckCard(id).add.interactable = true;
        if (deck.cards.TryGetValue(id, out int count)) deck.cards[id] = Mathf.Max(0, count - 1);
        else
        {
            deck.cards[id] = 0;
            return;
        }
        foreach (Transform t in deckCards)
        {
            if (t.GetComponent<DeckCard>().ID == id)
            {
                Destroy(t.gameObject);
                break;
            }
        }
    }

    public void Confirm()
    {
        PlayerPrefs.SetString("Decks", JsonConvert.SerializeObject(UserAccounts.allDecks));
        SceneManager.LoadScene("Decks", LoadSceneMode.Single);
    }

    private DeckCard GetDeckCard(int id)
    {
        foreach (Transform t in allDeckCards)
        {
            DeckCard dc = t.GetComponent<DeckCard>();
            if (dc.ID == id) return dc;
        }
        return null;
    }

}
