using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;

public class LeaderboardManager : MonoBehaviour
{
    private string leaderboardId = "devplayers";
    [Header("Leaderboard Settings")]
    //[SerializeField] private int maxEntriesToShow = 10;
    [SerializeField] private bool includePlayerEntry = true;

    [Header("UI References")]
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform entriesContainer;
    [SerializeField] private Button refreshButton;
    [SerializeField] private GameObject loadingIndicator;

    // List to track instantiated entry objects for cleanup
    private List<GameObject> instantiatedEntries = new List<GameObject>();

    void Awake()
    {
        // Set up button listener
        if (refreshButton != null)
            refreshButton.onClick.AddListener(FetchLeaderboard);
    }

    async void Start()
    {
        try
        {
            await FetchLeaderboardAsync();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void FetchLeaderboard()
    {
        StartCoroutine(FetchLeaderboardRoutine());
    }

    private IEnumerator FetchLeaderboardRoutine()
    {
        // Show loading indicator
        SetLoading(true);

        // Run the async operation
        var operation = FetchLeaderboardAsync();

        // Wait for completion
        yield return new WaitUntil(() => operation.IsCompleted);

        // Hide loading indicator
        SetLoading(false);
    }

    private async Task FetchLeaderboardAsync()
    {
        try
        {
            // Clear existing entries
            ClearLeaderboardEntries();

            // Query for top scores
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
                leaderboardId/*,
                new GetScoresOptions
                {
                    Limit = maxEntriesToShow
                }*/
            );

            // Display scores
            DisplayLeaderboardEntries(scoresResponse.Results);

            // Get player's own score if needed
            if (includePlayerEntry && AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    var playerScoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(
                        leaderboardId
                    );

                    // Check if player score is already in the displayed list
                    bool alreadyDisplayed = false;
                    foreach (var entry in scoresResponse.Results)
                    {
                        if (entry.PlayerId == AuthenticationService.Instance.PlayerId)
                        {
                            alreadyDisplayed = true;
                            break;
                        }
                    }

                    // If player score exists but isn't in the top list, add it separately
                    if (!alreadyDisplayed && playerScoreResponse != null)
                    {
                        // Create a separator if needed
                        if (scoresResponse.Results.Count > 0)
                        {
                            CreateSeparator();
                        }

                        // Create the player entry with their rank
                        CreateLeaderboardEntry(
                            playerScoreResponse.Rank,
                            playerScoreResponse.PlayerId,
                            playerScoreResponse.PlayerName ?? "You",
                            playerScoreResponse.Score,
                            true
                        );
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"Couldn't get player score: {e.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching leaderboard: {e.Message}");
        }
    }

    private void DisplayLeaderboardEntries(List<LeaderboardEntry> entries)
    {
        if (entries.Count == 0)
        {
            // Show "No entries yet" message
            GameObject entryObj = Instantiate(entryPrefab, entriesContainer);
            instantiatedEntries.Add(entryObj);

            TextMeshProUGUI rankText = entryObj.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = entryObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreText = entryObj.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

            rankText.text = "-";
            nameText.text = "No entries yet";
            scoreText.text = "-";

            return;
        }

        // Process each leaderboard entry
        foreach (var entry in entries)
        {
            CreateLeaderboardEntry(
                entry.Rank,
                entry.PlayerId,
                string.IsNullOrEmpty(entry.PlayerName) ? "Anonymous" : entry.PlayerName,
                entry.Score,
                AuthenticationService.Instance.IsSignedIn && entry.PlayerId == AuthenticationService.Instance.PlayerId
            );
        }
    }

    private void CreateLeaderboardEntry(int rank, string playerId, string playerName, double score, bool isLocalPlayer)
    {
        GameObject entryObj = Instantiate(entryPrefab, entriesContainer);
        instantiatedEntries.Add(entryObj);

        // Get references to text components
        TextMeshProUGUI rankText = entryObj.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = entryObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = entryObj.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        // Populate data
        rankText.text = $"{rank+1}";
        nameText.text = playerName.Substring(0, playerName.IndexOf("#"));
        scoreText.text = FormatScore(score);

        // Highlight the local player's entry
        if (isLocalPlayer)
        {
            // Get background image
            Image background = entryObj.GetComponent<Image>();
            if (background != null)
            {
                // Change color to highlight
                background.color = new Color(0.9f, 0.9f, 0.4f, 0.3f);
            }
        }
    }

    private void CreateSeparator()
    {
        GameObject entryObj = Instantiate(entryPrefab, entriesContainer);
        instantiatedEntries.Add(entryObj);

        TextMeshProUGUI rankText = entryObj.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = entryObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = entryObj.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        rankText.text = "...";
        nameText.text = "...";
        scoreText.text = "...";

        // Make separator visually distinct
        Image background = entryObj.GetComponent<Image>();
        if (background != null)
        {
            background.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        }
    }

    // Helper method to format score (can be customized based on your game's needs)
    private string FormatScore(double score)
    {
        // Format as integer if it's a whole number
        if (score == Math.Floor(score))
        {
            return score.ToString("N0");
        }

        // Otherwise format with decimal places
        return score.ToString("N1");
    }

    private void ClearLeaderboardEntries()
    {
        foreach (var entry in instantiatedEntries)
        {
            Destroy(entry);
        }
        instantiatedEntries.Clear();
    }

    private void SetLoading(bool isLoading)
    {
        if (loadingIndicator != null)
            loadingIndicator.SetActive(isLoading);

        if (refreshButton != null)
            refreshButton.interactable = !isLoading;
    }

}