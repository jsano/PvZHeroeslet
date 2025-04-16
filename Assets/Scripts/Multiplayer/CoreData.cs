using System;
using Unity.Services.Multiplayer;

/// <summary>
/// The different types of SessionActions.
/// </summary>
public enum SessionAction
{
    Invalid,
    Create,
    StartMatchmaking,
    QuickJoin,
    JoinByCode,
    JoinById
}
    
/// <summary>
/// Data to enter a session.
/// </summary>
public struct EnterSessionData
{
    public SessionAction SessionAction;
    public string SessionName;
    public string JoinCode;
    public string Id;
    public bool IsPrivate;
    public AdditionalOptions AdditionalOptions;
}

/// <summary>
/// Additional data to enter specific session types.
/// </summary>
public struct AdditionalOptions
{
    public MatchmakerOptions MatchmakerOptions;
    public bool AutoCreateSession;
}


