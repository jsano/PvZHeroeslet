using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject optionsMenu;
    
    public void ToggleOptions()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

}
