using UnityEngine;
using UnityEngine.UI;

public class LobbyUIButton : MonoBehaviour
{

    public bool selected { get; private set; }
    public bool disabled { get; private set; }
    public int ID;

    public void Toggle()
    {
        selected = !selected;
        if (selected) GetComponent<Image>().color = new Color(1f, 1f, 0.8f);
        else GetComponent<Image>().color = Color.white;
    }

    public void Disable()
    {
        disabled = true;
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = new Color(1f, 0.8f, 0.8f);
    }

}
