using Unity.Services.Authentication;
using UnityEngine;

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
    }

    public async void SignUp()
    {
        await UserAccounts.Instance.SignUpWithUsernamePasswordAsync(inputUsername, inputPassword, inputDisplayName);
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
