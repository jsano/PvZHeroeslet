using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{

    public class Deck
    {
        public int heroID;
        public Dictionary<int, int> cards = new();

        public Deck(int hero)
        {
            heroID = hero;
        }
    }

    [HideInInspector]
    public string deckName;
    private Deck deck;
    public Transform deckCards;
    public Transform allDeckCards;
    public GameObject deckCardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = UserAccounts.allDecks[deckName];
        for (int i = 0; i < AllCards.Instance.cards.Length; i++)
        {
            Card c = AllCards.Instance.cards[i];
            if (AllCards.Instance.heroes[deck.heroID].classes[0] == c._class || AllCards.Instance.heroes[deck.heroID].classes[1] == c._class)
            {
                DeckCard d = Instantiate(deckCardPrefab, allDeckCards).GetComponent<DeckCard>();
                d.ID = i;
            }
        }
    }

    public void Add(int id)
    {
        deck.cards.Add(id, deck.cards.TryGetValue(id, out int count) ? count + 1 : 1);

    }

    public void Remove(int id)
    {
        deck.cards.Add(id, deck.cards.TryGetValue(id, out int count) ? count - 1 : 0);

    }

    public void Confirm()
    {
        PlayerPrefs.SetString("Decks", JsonConvert.SerializeObject(UserAccounts.allDecks));
        SceneManager.LoadScene("Decks", LoadSceneMode.Single);
    }

}
