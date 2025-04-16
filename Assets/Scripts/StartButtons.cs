using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Multiplayer;

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
            { AllCards.NameToID("Sunflower"), 1 },
            { AllCards.NameToID("Cornucopia"), 1 },
            { AllCards.NameToID("Pineclone"), 1 },
            { AllCards.NameToID("Seedling"), 2 },
            { AllCards.NameToID("Bananasaurus Rex"), 1 },
            { AllCards.NameToID("Sting Bean"), 1 },
            { AllCards.NameToID("Poppin' Poppies"), 1 },
            { AllCards.NameToID("Winter Melon"), 1 },
            { AllCards.NameToID("Squash"), 1 }
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
            { AllCards.NameToID("Mini-Ninja"), 2 },
            { AllCards.NameToID("Teleport"), 2 },
            { AllCards.NameToID("Smoke Bomb"), 1 },
            { AllCards.NameToID("Pied Piper"), 1 },
            { AllCards.NameToID("Lurch for Lunch"), 1 },
            { AllCards.NameToID("Disco"), 1 },
            { AllCards.NameToID("Zombot Plank Walker"), 1 },
            { AllCards.NameToID("Trickster"), 1 }
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
