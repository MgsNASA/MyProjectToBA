using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour
{
    public Image hourglassImage;    // The hourglass image
    public Image chestClosed;       // Closed chest image
    public Image chestOpen;         // Open chest image
    public Button bonusButton;      // The bonus button
    public TextMeshProUGUI timerText;  // Text to show the remaining time
    public float bonusCooldownHours = 24f; // The time in hours before the bonus button becomes active
    private DateTime nextBonusTime; // The next time the bonus is available
    private MoneyManager moneyManager;

    private void Start( )
    {
        // Load the next bonus time or set it to now if not saved
        if ( PlayerPrefs.HasKey ( "NextBonusTime" ) )
        {
            long temp = Convert.ToInt64 ( PlayerPrefs.GetString ( "NextBonusTime" ) );
            nextBonusTime = DateTime.FromBinary ( temp );
        }
        else
        {
            nextBonusTime = DateTime.Now;
        }

        moneyManager = FindObjectOfType<MoneyManager> ();  // Find the MoneyManager in the scene

        bonusButton.gameObject.SetActive ( false );
        bonusButton.onClick.AddListener ( ClaimBonus );  // Add listener to the button click event

        StartCoroutine ( AnimateHourglass () );
    }

    private void Update( )
    {
        // Calculate remaining time
        TimeSpan remainingTime = nextBonusTime - DateTime.Now;

        if ( remainingTime.TotalSeconds <= 0 )
        {
            // Show the bonus button
            hourglassImage.gameObject.SetActive ( false );
            bonusButton.gameObject.SetActive ( true );
            timerText.text = "";

            // Show the open chest
            chestClosed.gameObject.SetActive ( false );
            chestOpen.gameObject.SetActive ( true );
        }
        else
        {
            // Update the timer text without leading zeros
            string days = remainingTime.Days > 0 ? remainingTime.Days.ToString () : "";
            string hours = remainingTime.Hours > 0 ? remainingTime.Hours.ToString () : "";
            string minutes = remainingTime.Minutes > 0 ? remainingTime.Minutes.ToString () : "";
            string seconds = remainingTime.Seconds > 0 ? remainingTime.Seconds.ToString () : "";

            timerText.text = string.Format ( "{0}{1}{2}{3}" ,
                days != "" ? days + ":" : "" ,
                hours != "" ? hours + ":" : "" ,
                minutes != "" ? minutes + ":" : "" ,
                seconds );

            // Show the closed chest
            chestClosed.gameObject.SetActive ( true );
            chestOpen.gameObject.SetActive ( false );
        }

        // Check for space key press to clear and restart the timer
        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            Clear ();
        }
    }

    public void ClaimBonus( )
    {
        if ( moneyManager != null )
        {
            moneyManager.Add ( 1000 );  // Add 1000 coins to the player's total
        }
        else
        {
            Debug.LogError ( "MoneyManager not found!" );
        }

        // Set the next bonus time to the specified cooldown hours from now
        nextBonusTime = DateTime.Now.AddHours ( bonusCooldownHours );
        PlayerPrefs.SetString ( "NextBonusTime" , nextBonusTime.ToBinary ().ToString () );
        PlayerPrefs.Save ();

        // Hide the bonus button and show the hourglass
        bonusButton.gameObject.SetActive ( false );
        hourglassImage.gameObject.SetActive ( true );

        // Show the closed chest
        chestClosed.gameObject.SetActive ( true );
        chestOpen.gameObject.SetActive ( false );
    }

    public void Clear( )
    {
        // Reset the next bonus time to now
        nextBonusTime = DateTime.Now;
        PlayerPrefs.DeleteKey ( "NextBonusTime" );
        bonusButton.gameObject.SetActive ( false );
        hourglassImage.gameObject.SetActive ( true );
        Debug.Log ( "Bonus timer reset." );

        // Show the closed chest
        chestClosed.gameObject.SetActive ( true );
        chestOpen.gameObject.SetActive ( false );
    }

    private IEnumerator AnimateHourglass( )
    {
        while ( true )
        {
            // Wait for 15 seconds
            yield return new WaitForSecondsRealtime ( 15f );

            // Animate the hourglass (rotate 360 degrees)
            float duration = 1f; // Duration of the rotation
            float elapsedTime = 0f;

            Quaternion startRotation = hourglassImage.transform.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler ( 0 , 0 , 360 );

            while ( elapsedTime < duration )
            {
                hourglassImage.transform.rotation = Quaternion.Slerp ( startRotation , endRotation , elapsedTime / duration );
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            hourglassImage.transform.rotation = startRotation; // Ensure it's exactly at the start rotation
        }
    }
}
