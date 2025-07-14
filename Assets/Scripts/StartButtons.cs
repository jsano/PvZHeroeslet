using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Multiplayer;
using System.Threading;
using System;

public class StartButtons : NetworkBehaviour
{
    
    public void ChangeNetworkScene(string scene)
    {
        if (NetworkManager.Singleton.IsHost)
            NetworkManager.SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void ChangeScene(string scene)
    {
        // Prevent multiple NetworkManagers
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Shutdown();
            Destroy(NetworkManager.gameObject);
        }
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // DEBUG ===
    public void StartHost()
    {
        UserAccounts.allDecks.Add("temp10", new DeckBuilder.Deck(10));
        UserAccounts.allDecks["temp10"].cards = new() {
            { AllCards.NameToID("Solar Winds"), 2 },
            { AllCards.NameToID("Banana Bomb"), 2 },
            { AllCards.NameToID("Wall-nut"), 2 },
            { AllCards.NameToID("Bubble Up"), 2 },
            { AllCards.NameToID("Peashooter"), 2 },
            { AllCards.NameToID("Whirlwind"), 4 },
            { AllCards.NameToID("Apple-Saucer"), 2 }
        };

        NetworkManager.Singleton.StartHost();
        GameObject.Find("Host").SetActive(false);
        GameObject.Find("Client").SetActive(false);
        UserAccounts.GameStats.DeckName = "temp10";
        UserAccounts.GameStats.PlantHero = 0;
        UserAccounts.GameStats.ZombieHero = 15;
        NetworkManager.OnConnectionEvent += P2Joined;
    }

    public void StartClient()
    {
        UserAccounts.allDecks.Add("temp11", new DeckBuilder.Deck(18));
        UserAccounts.allDecks["temp11"].cards = new() {
            { AllCards.NameToID("Medulla Nebula"), 2 },
            { AllCards.NameToID("Triplication"), 2 },
            { AllCards.NameToID("Moonwalker"), 2 },
            { AllCards.NameToID("Cosmic Scientist"), 2 },
            { AllCards.NameToID("Cryo-brain"), 1 },
            { AllCards.NameToID("Imp"), 2 },
            { AllCards.NameToID("Backyard Bounce"), 2 },
            { AllCards.NameToID("Lurch for Lunch"), 2 }
        };

        UserAccounts.GameStats.DeckName = "temp11";
        UserAccounts.GameStats.PlantHero = 0;
        UserAccounts.GameStats.ZombieHero = 15;
        NetworkManager.Singleton.StartClient();
    }

    private void P2Joined(NetworkManager nm, ConnectionEventData data)
    {
        if (data.EventType == ConnectionEvent.PeerConnected)
        {
            NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
    // ===

}
