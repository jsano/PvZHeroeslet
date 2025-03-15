using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckList : MonoBehaviour
{

    public int ID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void New()
    {
        string n = "New " + ID;
        Debug.Log(UserAccounts.allDecks);
        UserAccounts.allDecks.Add(n, new DeckBuilder.Deck(ID));
        DeckBuilder.deckName = n;
        SceneManager.LoadScene("DeckBuilder", LoadSceneMode.Single);
    }

}
