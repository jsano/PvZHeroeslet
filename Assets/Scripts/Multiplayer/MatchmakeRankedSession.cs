using System;
using Unity.Services.Multiplayer;
using UnityEngine;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
internal class MatchmakeRankedSession : EnterSessionBase
{
    
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
}
