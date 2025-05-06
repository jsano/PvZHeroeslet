using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButtons : MonoBehaviour
{

    private string inputUsername;
    private string inputPassword;
    private string inputDisplayName;
    public GameObject LoginPanel;
    public GameObject SignupPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic("Menu");
    }

    void Update()
    {
        if (UserAccounts.Instance.CachedScore != null) SceneManager.LoadScene("Start");
    }

    public void SwapModes()
    {
        inputUsername = "";
        inputPassword = "";
        inputDisplayName = "";
        LoginPanel.SetActive(!LoginPanel.activeSelf);
        SignupPanel.SetActive(!SignupPanel.activeSelf);
    }

    public async void Login()
    {
        await UserAccounts.Instance.SignInWithUsernamePasswordAsync(inputUsername, inputPassword);
        //SceneManager.LoadScene("Start");
    }

    public async void SignUp()
    {
        await UserAccounts.Instance.SignUpWithUsernamePasswordAsync(inputUsername, inputPassword, inputDisplayName);
        //SceneManager.LoadScene("Start");
    }

    public void SetInputUsername(string s)
    {
        inputUsername = s;
    }

    public void SetInputPassword(string s)
    {
        inputPassword = s;
    }

    public void SetInputDisplayName(string s)
    {
        inputDisplayName = s;
    }

}
