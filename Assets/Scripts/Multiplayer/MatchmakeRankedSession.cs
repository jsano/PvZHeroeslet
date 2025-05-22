using System;
using Unity.Services.Multiplayer;
using UnityEngine;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
internal class MatchmakeRankedSession : EnterSessionBase
{

    public GameObject loadingScreen;
    public GameObject loadingImage;

    protected override void Awake()
    {
        base.Awake();
        LeanTween.rotateAroundLocal(loadingImage, Vector3.forward, -360f, 2f).setRepeat(-1);
        m_EnterSessionButton.onClick.AddListener(() => loadingScreen.SetActive(true));
        FailedToJoinSession.AddListener((message) => { if (message.Error == SessionError.MatchmakerAssignmentTimeout)
            {
                UserAccounts.Instance.ShowError("Matchmaking timed out");
                loadingScreen.SetActive(false);
            }
        });
    }
    
    protected override EnterSessionData GetSessionData()
    {
        return new EnterSessionData
        {
            SessionAction = SessionAction.StartMatchmaking,
            ranked = true,
            AdditionalOptions = new AdditionalOptions
            {
                MatchmakerOptions = new MatchmakerOptions
                {
                    QueueName = "Default"
                }
            }
        };
    }

    public void Cancel()
    {
        SessionManager.Instance.CancelMatch();
        loadingScreen.SetActive(false);
    }

}
