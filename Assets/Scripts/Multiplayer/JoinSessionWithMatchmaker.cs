using System;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class JoinSessionWithMatchmaker : EnterSessionBase
{
    [Header("Matchmaker Options")]
    [Tooltip("The user will initiate the Matchmaking in this Queue.")]
    [SerializeField]
    string m_QueueName = "default";

    protected override EnterSessionData GetSessionData()
    {
        return new EnterSessionData
        {
            SessionAction = SessionAction.StartMatchmaking,
            AdditionalOptions = new AdditionalOptions
            {
                MatchmakerOptions = new MatchmakerOptions
                {
                    QueueName = m_QueueName
                }
            }
        };
    }
}
