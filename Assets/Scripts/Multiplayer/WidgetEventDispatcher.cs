using System;
using System.Collections.Generic;
using Unity.Services.Multiplayer;
using UnityEngine.Events;

/// <summary>
/// The WidgetEventDispatcher collects all widgets and calls relevant events when they occur.
/// </summary>
internal class WidgetEventDispatcher : LazySingleton<WidgetEventDispatcher>
{
    internal UnityEvent OnServicesInitializedEvent = new ();
        
    List<IWidget> m_Widgets = new();

    List<ISessionProvider> m_SessionProviders = new();
    List<ISessionLifecycleEvents> m_SessionLifecycleListeners = new();
    List<ISessionEvents> m_SessionEventListeners = new();

    ISession m_Session;

    internal void OnServicesInitialized()
    {
        for (var index = m_Widgets.Count - 1; index >= 0; index--)
        {
            var widget = m_Widgets[index];
            widget.IsInitialized = true;
            widget.OnServicesInitialized();
        }

        OnServicesInitializedEvent?.Invoke();
    }
        
    /// <summary>
    /// Used to register <see cref="IWidget"/> to the <see cref="WidgetEventDispatcher"/>.
    /// </summary>
    /// <param name="widget">The widget to register.</param>
    internal void RegisterWidget(IWidget widget)
    {
        if(!ManagerFactory.IsInitialized)
            ManagerFactory.Initialize();
            
        if (widget is ISessionProvider sessionAccessor)
        {
            m_SessionProviders.Add(sessionAccessor);
            sessionAccessor.Session = m_Session;
        }

        if (widget is ISessionLifecycleEvents sessionLifecycleEvents)
        {
            m_SessionLifecycleListeners.Add(sessionLifecycleEvents);
        }
            
        if (widget is ISessionEvents sessionEvents)
        {
            m_SessionEventListeners.Add(sessionEvents);
        }
            
        m_Widgets.Add(widget);

        widget.IsInitialized = WidgetServiceInitialization.IsInitialized;
        if(WidgetServiceInitialization.IsInitialized)
            widget.OnServicesInitialized();
    }

        
    /// <summary>
    /// Used to unregister <see cref="IWidget"/> from the <see cref="WidgetEventDispatcher"/>.
    /// </summary>
    /// <param name="widget">The widget to unregister.</param>
    internal void UnregisterWidget(IWidget widget)
    {
        if (widget is ISessionProvider sessionAccessor)
        {
            sessionAccessor.Session = null;
            m_SessionProviders.Remove(sessionAccessor);
        }
            
        if (widget is ISessionLifecycleEvents sessionLifecycleEvents)
        {
            m_SessionLifecycleListeners.Remove(sessionLifecycleEvents);
        }

        if (widget is ISessionEvents sessionEvents)
        {
            m_SessionEventListeners.Remove(sessionEvents);
        }
            
        m_Widgets.Remove(widget);
    }

    internal void OnSessionJoined(ISession session)
    {
        m_Session = session;

        for (var index = m_SessionProviders.Count - 1; index >= 0; index--)
        {
            var sessionAccessor = m_SessionProviders[index];
            sessionAccessor.Session = m_Session;
        }

        for (var index = m_SessionLifecycleListeners.Count - 1; index >= 0; index--)
        {
            var sessionLifecycleListeners = m_SessionLifecycleListeners[index];
            sessionLifecycleListeners.OnSessionJoined();
        }
    }

    internal void OnSessionLeft()
    {
        m_Session = null;

        for (var index = m_SessionProviders.Count - 1; index >= 0; index--)
        {
            var sessionAccessor = m_SessionProviders[index];
            sessionAccessor.Session = null;
        }

        for (var index = m_SessionLifecycleListeners.Count - 1; index >= 0; index--)
        {
            var sessionLifecycleListener = m_SessionLifecycleListeners[index];
            sessionLifecycleListener.OnSessionLeft();
        }
    }
        
    internal void OnSessionJoining()
    {
        for (var index = m_SessionLifecycleListeners.Count - 1; index >= 0; index--)
        {
            var sessionLifecycleListener = m_SessionLifecycleListeners[index];
            sessionLifecycleListener.OnSessionJoining();
        }
    }
        
    internal void OnSessionFailedToJoin(SessionException sessionException)
    {
        for (var index = m_SessionLifecycleListeners.Count - 1; index >= 0; index--)
        {
            var sessionLifecycleListener = m_SessionLifecycleListeners[index];
            sessionLifecycleListener.OnSessionFailedToJoin(sessionException);
        }
    }

    internal void OnSessionChanged()
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnSessionChanged();
        }
    }

    internal void OnSessionStateChanged(SessionState sessionState)
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnSessionStateChanged(sessionState);
        }
    }
        
    internal void OnPlayerJoinedSession(string playerId)
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnPlayerJoinedSession(playerId);
        }
    }
        
    internal void OnPlayerLeftSession(string playerId)
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnPlayerLeftSession(playerId);
        }
    }
        
    internal void OnSessionPropertiesChanged()
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnSessionPropertiesChanged();
        }
    }
        
    internal void OnPlayerPropertiesChanged()
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnPlayerPropertiesChanged();
        }
    }
        
    internal void OnRemovedFromSession()
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnRemovedFromSession();
        }
    }
        
    internal void OnSessionDeleted()
    {
        for (var index = m_SessionEventListeners.Count - 1; index >= 0; index--)
        {
            var sessionEventListener = m_SessionEventListeners[index];
            sessionEventListener.OnSessionDeleted();
        }
    }

}

