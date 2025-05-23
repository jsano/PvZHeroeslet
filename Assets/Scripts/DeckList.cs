using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckList : MonoBehaviour
{

    private int ID = -1;
    public GameObject deckButtonPrefab;

    private TextMeshProUGUI heroName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        heroName = GameObject.Find("HeroName").GetComponent<TextMeshProUGUI>();
        GameObject.Find("Heroes").transform.GetChild(0).GetComponent<DeckListButton>().OnClick();
    }

    public void Show(int newID, bool force=false)
    {
        if (ID == newID && !force) return;
        ID = newID;
        foreach (Transform t in transform) if (!t.gameObject.name.Contains("New")) Destroy(t.gameObject);
        foreach (string name in UserAccounts.allDecks.Keys)
        {
            if (UserAccounts.allDecks[name].heroID == ID)
            {
                DeckButton d = Instantiate(deckButtonPrefab, transform).GetComponent<DeckButton>();
                d.deckName = name;
                d.transform.SetAsFirstSibling();
            }
        }
        heroName.text = AllCards.Instance.heroes[ID].name;
    }

    public void New()
    {
        DeckButton.deleting = false;
        string n = "New " + ID + " " + transform.childCount;
        UserAccounts.allDecks.Add(n, new DeckBuilder.Deck(ID));
        LoadDeck(n);
    }

    public void Remove(string name)
    {
        UserAccounts.allDecks.Remove(name);
        PlayerPrefs.SetString("Decks", JsonConvert.SerializeObject(UserAccounts.allDecks));
        Show(ID, true);
    }

    public void LoadDeck(string name)
    {
        DeckBuilder.deckName = name;
        //Debug.Log(CollectionToString(UserAccounts.allDecks.Values));
        SceneManager.LoadScene("DeckBuilder", LoadSceneMode.Single);
    }

    private string CollectionToString(ICollection myList)
    {
        string result = "";
        foreach (var item in myList)
        {
            if (item is DeckBuilder.Deck) result += CollectionToString(((DeckBuilder.Deck)item).cards) + "  ";
            else result += item.ToString() + "  ";
        }
        return result;
    }

}
