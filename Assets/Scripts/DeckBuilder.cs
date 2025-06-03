using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI counter;
    public Transform deckCards;
    public Transform superpowersUI;
    public Transform allDeckCards;
    public GameObject deckCardPrefab;
    public GameObject superpowerCardPrefab;

    public Transform[] positions;
    private List<Transform> superpowerCards = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = UserAccounts.allDecks[deckName];

        transform.Find("Title").GetComponent<TMP_InputField>().text = deckName;

        foreach (int id in deck.cards.Keys)
        {
            for (int i = 0; i < deck.cards[id]; i++)
            {
                DeckCard d = Instantiate(deckCardPrefab, deckCards).GetComponent<DeckCard>();
                d.ID = id;
                d.hideButtons = true;
            }
        }

        for (int i = 0; i < deck.superpowerOrder.Count; i++)
        {
            SuperpowerDeckCard d = Instantiate(superpowerCardPrefab, superpowersUI).GetComponent<SuperpowerDeckCard>();
            d.ID = deck.superpowerOrder[i];
            d.transform.localPosition = positions[i].localPosition;
            superpowerCards.Add(d.transform);
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
        SortAllCards("cost");

        counter.text = deckCards.childCount + "/40";
    }

    public void OnDeckNameChange(string s)
    {
        if (UserAccounts.allDecks.ContainsKey(s))
        {
            UserAccounts.Instance.ShowError("Deck name already exists");
            transform.Find("Title").GetComponent<TMP_InputField>().text = deckName;
            return;
        }
        Deck d = UserAccounts.allDecks[deckName];
        UserAccounts.allDecks.Remove(deckName);
        UserAccounts.allDecks[s] = d;
        deckName = s;
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

        counter.text = deckCards.childCount + "/40";
        if (deckCards.childCount >= 40)
        {
            foreach (Transform t in allDeckCards) t.GetComponent<DeckCard>().add.interactable = false;
        }
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
                t.SetParent(null);
                Destroy(t.gameObject);
                break;
            }
        }

        counter.text = deckCards.childCount + "/40";
        if (deckCards.childCount < 40)
        {
            foreach (Transform t in allDeckCards)
            {
                DeckCard dc = t.GetComponent<DeckCard>();
                if (!deck.cards.ContainsKey(dc.ID) || deck.cards[dc.ID] < 4) dc.add.interactable = true;
            }
        } 
    }

    public void UpdateSuperpowerOrder(Transform t, int id)
    {
        deck.superpowerOrder.Remove(id);
        for (int i = 0; i < positions.Length - 1; i++)
        {
            if (Mathf.Abs(t.localPosition.x - positions[i].localPosition.x) < Mathf.Abs(t.localPosition.x - positions[i+1].localPosition.x))
            {
                deck.superpowerOrder.Insert(i, id);
                break;
            }
        }
        for (int i = 0; i < deck.superpowerOrder.Count; i++)
        {
            foreach (Transform t1 in superpowerCards) if (t1.GetComponent<SuperpowerDeckCard>().ID == deck.superpowerOrder[i])
                {
                    t1.localPosition = positions[i].localPosition;
                    break;
                }
            
        }

        string result = "";
        foreach (var item in deck.superpowerOrder)
        {
            result += item.ToString() + "  ";
        }
    }

    public async void Confirm()
    {
        await UserAccounts.Instance.SaveData();
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

    public void SortAllCards(string key)
    {
        List<DeckCard> lst = new();
        foreach (Transform t in allDeckCards) lst.Add(t.GetComponent<DeckCard>());
        var field = AllCards.Instance.cards[0].GetType().GetField(key);
        lst.Sort((a, b) => ((int)field.GetValue(AllCards.Instance.cards[a.ID])).CompareTo((int)field.GetValue(AllCards.Instance.cards[b.ID])));
        for (var i = lst.Count - 1; i >= 0; i--)
        {
            lst[i].transform.SetSiblingIndex(0);
        }
    }

    public void Search(string s)
    {
        s = s.ToLower();
        foreach (Transform t in allDeckCards)
        {
            Card c = AllCards.Instance.cards[t.GetComponent<DeckCard>().ID];
            t.gameObject.SetActive(false);
            if (c.name.ToLower().Contains(s)) t.gameObject.SetActive(true);
            foreach (Card.Tribe tribe in c.tribes) if (Enum.GetName(typeof(Card.Tribe), tribe).ToLower().Contains(s)) t.gameObject.SetActive(true);
            if (c.description.ToLower().Contains(s)) t.gameObject.SetActive(true);
            var field = c.GetType().GetField(s);
            if (field != null && (field.GetType() == typeof(int) && (int)field.GetValue(c) > 0 || c.GetType() == typeof(bool) && (bool)field.GetValue(c))) t.gameObject.SetActive(true);
        }
    }

}
