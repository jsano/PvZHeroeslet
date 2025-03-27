using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckList : MonoBehaviour
{

    public int ID;
    public GameObject deckButtonPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    public void New()
    {
        string n = "New " + ID + " " + transform.childCount;
        UserAccounts.allDecks.Add(n, new DeckBuilder.Deck(ID));
        LoadDeck(n);
    }

    public void Remove(string name)
    {
        UserAccounts.allDecks.Remove(name);
        Start();
    }

    public void LoadDeck(string name)
    {
        DeckBuilder.deckName = name;
        Debug.Log(CollectionToString(UserAccounts.allDecks.Keys));
        Debug.Log(CollectionToString(UserAccounts.allDecks.Values));
        SceneManager.LoadScene("DeckBuilder", LoadSceneMode.Single);
    }

    private string CollectionToString(ICollection myList)
    {
        string result = "";
        foreach (var item in myList)
        {
            if (myList is IDictionary) result += CollectionToString(myList) + "   ";
            else result += item.ToString() + "  ";
        }
        return result;
    }

}
