using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry> ();
    public GameObject top3EntryPrefab;
    public GameObject regularEntryPrefab;
    public Transform leaderboardContainer;
    public Transform textContainer;
    public GameObject placeholderTextPrefab; // Префаб для текста "Play First"
    private GameObject currentPlaceholderText;

    public void AddEntry( int score )
    {
        // Check if the score is zero, if so, don't add it to the leaderboard
        if ( score == 0 )
        {
            Debug.Log ( "Score is zero, not adding to the leaderboard." );
            return;
        }

        LeaderboardEntry newEntry = new LeaderboardEntry ( score );
        leaderboard.Add ( newEntry );
        leaderboard.Sort ( ( a , b ) => b.score.CompareTo ( a.score ) );
        UpdateLeaderboardUI ();
    }

    public void DisplayLeaderboard( )
    {
        foreach ( var entry in leaderboard )
        {
            Debug.Log ( $"Score: {entry.score}" );
        }
    }

    public void SaveLeaderboard( )
    {
        for ( int i = 0; i < leaderboard.Count; i++ )
        {
            PlayerPrefs.SetInt ( $"Player_{i}_Score" , leaderboard [ i ].score );
        }
        PlayerPrefs.SetInt ( "LeaderboardCount" , leaderboard.Count );
        PlayerPrefs.Save ();
    }

    public void LoadLeaderboard( )
    {
        int count = PlayerPrefs.GetInt ( "LeaderboardCount" , 0 );
        leaderboard.Clear ();
        for ( int i = 0; i < count; i++ )
        {
            int score = PlayerPrefs.GetInt ( $"Player_{i}_Score" , 0 );
            leaderboard.Add ( new LeaderboardEntry ( score ) );
        }
        UpdateLeaderboardUI ();
    }

    private void UpdateLeaderboardUI( )
    {
        // Удаление всех существующих элементов в контейнере таблицы лидеров
        foreach ( Transform child in leaderboardContainer )
        {
            Destroy ( child.gameObject );
        }

        // Удаление текущего объекта текста "Play First" если он существует
        if ( currentPlaceholderText != null )
        {
            Destroy ( currentPlaceholderText );
            currentPlaceholderText = null;
        }

        // Если таблица лидеров пуста, показываем текст "Play First"
        if ( leaderboard.Count == 0 )
        {
            currentPlaceholderText = Instantiate ( placeholderTextPrefab , textContainer );
            TextMeshProUGUI placeholderText = currentPlaceholderText.GetComponent<TextMeshProUGUI> ();
            placeholderText.text = "Play First";
        }
        else
        {
            // Добавляем записи в таблицу лидеров
            for ( int i = 0; i < leaderboard.Count; i++ )
            {
                GameObject entryObj;
                if ( i < 3 )
                {
                    entryObj = Instantiate ( top3EntryPrefab , leaderboardContainer );
                }
                else
                {
                    entryObj = Instantiate ( regularEntryPrefab , leaderboardContainer );
                }

                // Ищем компоненты TextMeshProUGUI по имени
                TextMeshProUGUI [ ] textComponents = entryObj.GetComponentsInChildren<TextMeshProUGUI> ( true );
                foreach ( var textComponent in textComponents )
                {
                    if ( textComponent.name == "ScoreText" )
                    {
                        textComponent.text = leaderboard [ i ].score.ToString ();
                    }
                    else if ( textComponent.name == "PositionText" )
                    {
                        textComponent.text = ( i + 1 ).ToString ();
                    }
                }
            }
        }
    }

    private void Start( )
    {
        LoadLeaderboard ();
        UpdateLeaderboardUI ();
    }
}
