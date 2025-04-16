using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Multiplayer;
using UnityEngine;


/// <summary>
/// The SessionManager handles the lifecycle of a Session.
///
/// When the application is quit the SessionManager will leave the active session.
/// </summary>
[DefaultExecutionOrder(-100)]
internal class SessionManager : LazySingleton<SessionManager>
{
    bool m_Initialized;

    QuerySessionsResults m_SessionQueryResults;

    WidgetEventDispatcher m_WidgetEventDispatcher;

    ISession m_ActiveSession;

    internal ISession ActiveSession
    {
        get => m_ActiveSession;
        private set
        {
            if (value != null)
            {
                m_ActiveSession = value;
                RegisterSessionEvents();
                Debug.Log($"Joined Session {m_ActiveSession.Id}");
                m_WidgetEventDispatcher.OnSessionJoined(m_ActiveSession);
            }
            else if (m_ActiveSession != null)
            {
                m_ActiveSession = null;
                m_WidgetEventDispatcher.OnSessionLeft();
            }
        }
    }

    EnterSessionData EnterSessionData { get; set; }

    async void Awake()
    {
        if (!m_Initialized)
        {
            var widgetDependencies = WidgetDependencies.Instance;
                
            WidgetEventDispatcher.Instance.OnServicesInitializedEvent.AddListener(OnServicesInitialized);
            await widgetDependencies.ServiceInitialization.InitializeAsync();
        }
    }

    void Start()
    {
        m_WidgetEventDispatcher = WidgetEventDispatcher.Instance;
    }

    void OnServicesInitialized()
    {
        m_Initialized = true;
    }
        
    internal async Task EnterSession(EnterSessionData enterSessionData)
    {
        try
        {
            if (!m_Initialized)
                throw new InvalidOperationException($"Services are not initialized.");

            EnterSessionData = enterSessionData;

            if (m_ActiveSession != null)
            {
                await LeaveSession();
            }

            var playerProperties = await GetPlayerProperties();

            Debug.Log("Joining Session...");
            m_WidgetEventDispatcher.OnSessionJoining();

            var joinSessionOptions = new JoinSessionOptions
            {
                PlayerProperties = playerProperties
            };

            var sessionOptions = new SessionOptions
            {
                MaxPlayers = 2,
                IsLocked = false,
                IsPrivate = enterSessionData.IsPrivate,
                PlayerProperties = playerProperties,
                Name = enterSessionData.SessionAction == SessionAction.Create ? enterSessionData.SessionName : Guid.NewGuid().ToString()
            };
                
            SetConnection(ref sessionOptions);

            switch (enterSessionData.SessionAction)
            {
                case SessionAction.Create:
                    ActiveSession = await WidgetDependencies.Instance.MultiplayerService.CreateSessionAsync(sessionOptions);
                    break;
                case SessionAction.StartMatchmaking:
                    ActiveSession = await WidgetDependencies.Instance.MultiplayerService.MatchmakeSessionAsync(enterSessionData.AdditionalOptions.MatchmakerOptions, sessionOptions);
                    break;
                case SessionAction.QuickJoin:
                    var quickJoinOptions = new QuickJoinOptions
                    {
                        CreateSession = enterSessionData.AdditionalOptions.AutoCreateSession
                    };
                    ActiveSession = await WidgetDependencies.Instance.MultiplayerService.MatchmakeSessionAsync(quickJoinOptions, sessionOptions);
                    break;
                case SessionAction.JoinByCode:
                    ActiveSession = await WidgetDependencies.Instance.MultiplayerService.JoinSessionByCodeAsync(enterSessionData.JoinCode, joinSessionOptions);
                    break;
                case SessionAction.JoinById:
                    ActiveSession = await WidgetDependencies.Instance.MultiplayerService.JoinSessionByIdAsync(enterSessionData.Id, joinSessionOptions);
                    break;
                case SessionAction.Invalid:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (SessionException sessionException)
        {
            HandleSessionException(sessionException);
        }
        catch (AggregateException ae)
        {
            ae.Handle(ex =>
            {
                if (ex is SessionException sessionException)
                {
                    HandleSessionException(sessionException);
                    return true;
                }

                return false;
            });
        }
    }

    void HandleSessionException(SessionException sessionException)
    {
        Debug.LogException(sessionException);
        m_WidgetEventDispatcher.OnSessionFailedToJoin(sessionException);
        ActiveSession = null;
    }

    async Task<Dictionary<string, PlayerProperty>> GetPlayerProperties()
    {
        var playerName = await WidgetDependencies.Instance.AuthenticationService.GetPlayerNameAsync();
        var playerNameProperty = new PlayerProperty(playerName, VisibilityPropertyOptions.Member);
        var playerProperties = new Dictionary<string, PlayerProperty> { { "w_PlayerName", playerNameProperty } };
        return playerProperties;
    }

    static void SetConnection(ref SessionOptions options)
    {
        options.WithRelayNetwork();
    }
        
    internal async Task<IList<ISessionInfo>> QuerySessions()
    {
        var sessionQueryOptions = new QuerySessionsOptions();
        m_SessionQueryResults = await WidgetDependencies.Instance.MultiplayerService.QuerySessionsAsync(sessionQueryOptions);
        return m_SessionQueryResults.Sessions;
    }
        
    internal async Task LeaveSession()
    {
        if (ActiveSession != null)
        {
            UnregisterPlayerEvents();
                
            try
            {
                await ActiveSession.LeaveAsync();    
            }
            catch
            {
                // Ignored as we are exiting the game
            }
            finally
            {
                ActiveSession = null;
            }
        }
    }

    internal async void KickPlayer(string playerId)
    {
        if (!ActiveSession.IsHost)
            return;

        await ActiveSession.AsHost().RemovePlayerAsync(playerId);
    }
        
    void RegisterSessionEvents()
    {
        ActiveSession.Changed += m_WidgetEventDispatcher.OnSessionChanged;
        ActiveSession.StateChanged += m_WidgetEventDispatcher.OnSessionStateChanged;
        ActiveSession.PlayerJoined += m_WidgetEventDispatcher.OnPlayerJoinedSession;
        ActiveSession.PlayerLeaving += m_WidgetEventDispatcher.OnPlayerLeftSession;
        ActiveSession.SessionPropertiesChanged += m_WidgetEventDispatcher.OnSessionPropertiesChanged;
        ActiveSession.PlayerPropertiesChanged += m_WidgetEventDispatcher.OnPlayerPropertiesChanged;
        ActiveSession.RemovedFromSession += m_WidgetEventDispatcher.OnRemovedFromSession;
        ActiveSession.Deleted += m_WidgetEventDispatcher.OnSessionDeleted;
            
        ActiveSession.RemovedFromSession += OnRemovedFromSession;
    }
        
    void UnregisterPlayerEvents()
    {
        ActiveSession.Changed -= m_WidgetEventDispatcher.OnSessionChanged;
        ActiveSession.StateChanged -= m_WidgetEventDispatcher.OnSessionStateChanged;
        ActiveSession.PlayerJoined -= m_WidgetEventDispatcher.OnPlayerJoinedSession;
        ActiveSession.PlayerLeaving -= m_WidgetEventDispatcher.OnPlayerLeftSession;
        ActiveSession.SessionPropertiesChanged -= m_WidgetEventDispatcher.OnSessionPropertiesChanged;
        ActiveSession.PlayerPropertiesChanged -= m_WidgetEventDispatcher.OnPlayerPropertiesChanged;
        ActiveSession.RemovedFromSession -= m_WidgetEventDispatcher.OnRemovedFromSession;
        ActiveSession.Deleted -= m_WidgetEventDispatcher.OnSessionDeleted;
            
        ActiveSession.RemovedFromSession -= OnRemovedFromSession;
    }

    async void OnRemovedFromSession()
    {
        await LeaveSession();
    }
}
