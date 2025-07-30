using System;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public GameObject optionsMenu;
    public Slider music;
    public Slider sfx;
    public TMP_Dropdown resolutions;
    
    public void ToggleOptions()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);

        music.value = PlayerPrefs.GetFloat("music", 1);
        sfx.value = PlayerPrefs.GetFloat("sfx", 1);
        for (int i = 0; i < resolutions.options.Count; i++)
        {
            if (Screen.width == int.Parse(resolutions.options[i].text.Split(" x ")[0])) resolutions.value = i;
        }
        if (SceneManager.GetActiveScene().name == "Game") transform.Find("Logout").gameObject.SetActive(false);
    }

    public void SetDimensions(int index)
    {
        var temp = resolutions.options[index].text.Split(" x ");
        Screen.SetResolution(int.Parse(temp[0]), int.Parse(temp[1]), false);
    }

    public void LogOut()
    {
        AuthenticationService.Instance.SignOut(true);
    }

}
