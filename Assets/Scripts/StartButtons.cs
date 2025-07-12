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
            { AllCards.NameToID("Coffee Grounds"), 2 },
            { AllCards.NameToID("Vegetation Mutation"), 2 },
            { AllCards.NameToID("Banana Peel"), 2 },
            { AllCards.NameToID("Cosmic Pea"), 2 },
            { AllCards.NameToID("Banana Launcher"), 2 },
            { AllCards.NameToID("Whirlwind"), 4 },
            { AllCards.NameToID("Starch-Lord"), 2 }
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
            { AllCards.NameToID("Pogo Bouncer"), 2 },
            { AllCards.NameToID("Iron Boarder"), 2 },
            { AllCards.NameToID("Teleportation Station"), 2 },
            { AllCards.NameToID("Ice Moon"), 2 },
            { AllCards.NameToID("Cakesplosion"), 1 },
            { AllCards.NameToID("Imp"), 2 },
            { AllCards.NameToID("Backyard Bounce"), 2 },
            { AllCards.NameToID("Possessed"), 1 }
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
