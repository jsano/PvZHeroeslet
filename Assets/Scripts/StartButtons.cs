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
            { AllCards.NameToID("More Spore"), 4 },
            { AllCards.NameToID("Plucky Clover"), 2 },
            { AllCards.NameToID("Banana Bomb"), 4 },
            { AllCards.NameToID("Mushroom Grotto"), 2 },
            { AllCards.NameToID("Transmogrify"), 2 },
            { AllCards.NameToID("Witch Hazel"), 2 },
            { AllCards.NameToID("Seedling"), 2 },
            { AllCards.NameToID("Sap-fling"), 2 }
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
        UserAccounts.allDecks.Add("temp11", new DeckBuilder.Deck(21));
        UserAccounts.allDecks["temp11"].cards = new() {
            { AllCards.NameToID("Hippity-hop Gargantuar"), 2 },
            { AllCards.NameToID("Tomb Raiser"), 2 },
            { AllCards.NameToID("Captain Flameface"), 2 },
            { AllCards.NameToID("Imp-throwing Imp"), 2 },
            { AllCards.NameToID("Fire Rooster"), 2 },
            { AllCards.NameToID("Imposter"), 2 },
            { AllCards.NameToID("Zombot Plank Walker"), 2 },
            { AllCards.NameToID("Transformation Station"), 4 },
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
