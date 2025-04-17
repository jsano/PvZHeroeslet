using System;
using UnityEngine;

internal static class ManagerFactory
{
    internal static bool IsInitialized { get; private set; }

    internal static void Initialize()
    {
        // important to be set first as the VivoxManager Singleton calls the WidgetEventDispatcher in OnEnable.
        // It would lead to a deadlock if not set first as it would try to create yet another instance. 
        IsInitialized = true;
            
        CreateLazySingletonInstance("SessionManager");
    }
        
    // Reset IsInitialized to support domain reload disabled. 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        IsInitialized = false;
    }
        

    static void CreateLazySingletonInstance(string typeAndAssembly)
    {
        var manager = Type.GetType(typeAndAssembly);
        if (manager == null)
        {
            Debug.LogError($"{typeAndAssembly} not found. Did the assembly or name change?");
            return;
        }

        var baseType = manager.BaseType;
        if (baseType == null)
        {
            Debug.LogError("Base type not found. LazySingleton<T> expected.");
            return;
        }

        var property = baseType.GetProperty("Instance");

        if (property == null)
        {
            Debug.LogError("Instance property not found on LazySingleton<T>. Did the name change?");
            return;
        }

        property.GetMethod.Invoke(null, null);
    }
}
