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
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // DEBUG ===
    public void StartHost()
    {
        UserAccounts.allDecks.Add("temp10", new DeckBuilder.Deck(0));
        UserAccounts.allDecks["temp10"].cards = new() {
            { AllCards.NameToID("Poppin' Poppies"), 1 },
            { AllCards.NameToID("Pineclone"), 1 },
            { AllCards.NameToID("Wall-nut Bowling"), 1 },
            { AllCards.NameToID("Winter Melon"), 1 },
            { AllCards.NameToID("Bananasaurus Rex"), 1 },
            { AllCards.NameToID("The Great Zucchini"), 1 },
            { AllCards.NameToID("Grow-shroom"), 1 },
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
        UserAccounts.allDecks.Add("temp11", new DeckBuilder.Deck(15));
        UserAccounts.allDecks["temp11"].cards = new() {
            { AllCards.NameToID("Disco"), 2 },
            { AllCards.NameToID("Smoke Bomb"), 1 },
            { AllCards.NameToID("Pied Piper"), 1 },
            { AllCards.NameToID("Lurch for Lunch"), 1 },
            { AllCards.NameToID("Fun-Dead Raiser"), 1 },
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
