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
            { AllCards.NameToID("Haunted Pumpking"), 4 },
            { AllCards.NameToID("Embiggen"), 4 },
            { AllCards.NameToID("Force Field"), 2 },
            { AllCards.NameToID("Mushroom Grotto"), 2 },
            { AllCards.NameToID("Snowdrop"), 2 },
            { AllCards.NameToID("More Spore"), 2 },
            { AllCards.NameToID("Sunflower"), 4 },
            { AllCards.NameToID("Mush-boom"), 2 }
        };
        UserAccounts.GameStats.Buffs = new int[]
        {
            AllCards.NameToID("Dulled Pain"),
            AllCards.NameToID("Sugar Crash"),
            AllCards.NameToID("Mercy Kill"),
            AllCards.NameToID("Enduring Resolve"),
            AllCards.NameToID("Deep Freeze"),
            AllCards.NameToID("Lingering Relief"),
        };

        NetworkManager.Singleton.StartHost();
        GameObject.Find("Host").SetActive(false);
        GameObject.Find("Client").SetActive(false);
        UserAccounts.GameStats.DeckName = "temp10";
        UserAccounts.GameStats.PlantHero = 0;
        UserAccounts.GameStats.ZombieHero = 15;
        UserAccounts.GameStats.team = Card.Team.A;
        NetworkManager.OnConnectionEvent += P2Joined;
    }

    public void StartClient()
    {
        UserAccounts.allDecks.Add("temp11", new DeckBuilder.Deck(18));
        UserAccounts.allDecks["temp11"].cards = new() {
            { AllCards.NameToID("Toxic Waste Imp"), 2 },
            { AllCards.NameToID("Overstuffed Zombie"), 4 },
            { AllCards.NameToID("Excavator"), 2 },
            { AllCards.NameToID("Imp-throwing Imp"), 2 },
            { AllCards.NameToID("Fire Rooster"), 2 },
            { AllCards.NameToID("Imposter"), 2 },
            { AllCards.NameToID("Total Eclipse"), 2 },
            { AllCards.NameToID("Graveyard"), 2 },
        };
        UserAccounts.GameStats.Buffs = new int[]
        {
            AllCards.NameToID("Dulled Pain"),
            AllCards.NameToID("Sugar Crash"),
            AllCards.NameToID("Mercy Kill"),
            AllCards.NameToID("Enduring Resolve"),
            AllCards.NameToID("Deep Freeze"),
            AllCards.NameToID("Lingering Relief"),
        };

        UserAccounts.GameStats.DeckName = "temp11";
        UserAccounts.GameStats.PlantHero = 0;
        UserAccounts.GameStats.ZombieHero = 15;
        UserAccounts.GameStats.team = Card.Team.B;
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
