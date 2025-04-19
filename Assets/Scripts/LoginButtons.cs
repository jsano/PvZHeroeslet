using UnityEngine;

public class LoginButtons : MonoBehaviour
{

    private string inputUsername;
    private string inputPassword;
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
        LoginPanel.SetActive(!LoginPanel.activeSelf);
        SignupPanel.SetActive(!SignupPanel.activeSelf);
    }

    public async void Login()
    {
        await UserAccounts.Instance.SignInWithUsernamePasswordAsync(inputUsername, inputPassword);
    }

    public async void SignUp()
    {
        await UserAccounts.Instance.SignUpWithUsernamePasswordAsync(inputUsername, inputPassword);
    }

    public void SetInputUsername(string s)
    {
        inputUsername = s;
    }

    public void SetInputPassword(string s)
    {
        inputUsername = s;
    }

}
