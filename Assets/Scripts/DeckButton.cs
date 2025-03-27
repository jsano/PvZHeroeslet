using UnityEngine;

public class DeckButton : MonoBehaviour
{

    [HideInInspector]
    public string deckName;
    private DeckList DL;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DL = FindAnyObjectByType<DeckList>(FindObjectsInactive.Include).GetComponent<DeckList>();
    }

    public void Remove()
    {
        DL.Remove(deckName);
    }

    public void LoadDeck()
    {
        DL.LoadDeck(deckName);
    }

}
