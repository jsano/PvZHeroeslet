using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{

    private Button b;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        b = GetComponent<Button>();
    }

    private void Update()
    {
        if (!DeckButton.deleting) b.interactable = true;
    }

    public void OnClick()
    {
        if (UserAccounts.allDecks.Keys.Count == 0) return;
        DeckButton.deleting = true;
        b.interactable = false;
    }

}
