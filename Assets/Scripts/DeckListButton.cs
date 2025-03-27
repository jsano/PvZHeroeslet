using UnityEngine;
using UnityEngine.UI;

public class DeckListButton : MonoBehaviour
{

    public int ID;
    private DeckList dl;
    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dl = FindAnyObjectByType<DeckList>(FindObjectsInactive.Include).GetComponent<DeckList>();
        image = GetComponent<Image>();
    }

    public void OnClick()
    {
        foreach (Transform t in transform.parent) t.GetComponent<Image>().color = Color.clear;
        image.color = Color.yellow;
        dl.Show(ID);
    }

}
