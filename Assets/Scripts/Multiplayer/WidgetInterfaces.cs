using System;
using Unity.Services.Multiplayer;

/// <summary>
/// Get access to the current session.
///
/// Requires inheriting from <see cref="WidgetBehaviour"/>.
/// </summary>
internal interface ISessionProvider
{
    /// <summary>
    /// The current active Session.
    /// </summary>
    ISession Session { get; set; }
}
    
/// <summary>
/// Set and get the PlayerID.
/// Needs to be set for events that act on the individual player, like <see cref="IChatParticipantEvents"/>.
///
/// Is used by the <see cref="WidgetEventDispatcher"/> to get the PlayerID of the current player.
/// </summary>
internal interface IPlayerId
{
    /// <summary>
    /// The PlayerId of the signed in player.
    /// </summary>
    string PlayerId { get; set; }
}
    
/// <summary>
/// Get access to the events of the Session lifecycle.
/// 
/// Requires inheriting from <see cref="WidgetBehaviour"/> to be called.
/// </summary>
internal interface ISessionLifecycleEvents
{
    /// <summary>
    /// Called when a Session is about to be joined.
    /// </summary>
    void OnSessionJoining() { }
        
    /// <summary>
    /// Called when joining a Session failed.
    /// </summary>
    /// <param name="sessionException">The reason it failed.</param>
    void OnSessionFailedToJoin(SessionException sessionException) { }
        
    /// <summary>
    /// Called when a Session was joined successfully.
    /// </summary>
    void OnSessionJoined() { }
        
    /// <summary>
    /// Called when a Session was left.
    /// </summary>
    void OnSessionLeft() { }
}

/// <summary>
/// Get access to the events of a Session.
/// 
/// Requires inheriting from <see cref="WidgetBehaviour"/> to be called.
/// </summary>
internal interface ISessionEvents
{
    /// <summary>
    /// Called when a Session was changed.
    /// </summary>
    void OnSessionChanged() { }
        
    /// <summary>
    /// Called when the Session state changed.
    /// </summary>
    /// <param name="sessionState">The new state.</param>
    void OnSessionStateChanged(SessionState sessionState) { }
        
    /// <summary>
    /// Called when a player joins the Session.
    /// </summary>
    /// <param name="playerId">The id of the player.</param>
    void OnPlayerJoinedSession(string playerId) { }
        
    /// <summary>
    /// Called when a player left a Session.
    /// </summary>
    /// <param name="playerId">The id of the player.</param>
    void OnPlayerLeftSession(string playerId) { }
        
    /// <summary>
    /// Called when a SessionProperty changed.
    /// </summary>
    void OnSessionPropertiesChanged() { }
        
    /// <summary>
    /// Called when a PlayerProperty changed.
    /// </summary>
    void OnPlayerPropertiesChanged() { }
        
    /// <summary>
    /// Called when the active player was removed from the Session.
    /// </summary>
    void OnRemovedFromSession() { }

    /// <summary>
    /// Called when the Session was deleted.
    /// </summary>
    void OnSessionDeleted() { }

}
