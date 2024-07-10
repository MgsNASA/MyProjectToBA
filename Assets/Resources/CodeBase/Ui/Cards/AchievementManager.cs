    using System.Collections.Generic;
    using UnityEngine;

    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance
        {
            get; private set;
        }

        private Dictionary<Achievement.AchievementType , Achievement> achievements;
        [SerializeField]
        private List<CardAchievement> achievementCards;

        private void Awake( )
        {
            if ( Instance == null )
            {
                Instance = this;
                DontDestroyOnLoad ( gameObject );
                achievements = new Dictionary<Achievement.AchievementType , Achievement> ();
     
              //  RegisterAllCardAchievements (); // Регистрация всех дочерних CardAchievement
            }
            else
            {
                Destroy ( gameObject );
            }
        }

        private void RegisterAllCardAchievements( )
        {
            // Найти все дочерние объекты типа CardAchievement
            var cards = GetComponentsInChildren<CardAchievement> ( true );
            foreach ( var card in cards )
            {
                RegisterCard ( card );
            }
        }

        public void AddAchievement( Achievement achievement )
        {
            if ( achievement != null && !achievements.ContainsKey ( achievement.achievementType ) )
            {
                achievements.Add ( achievement.achievementType , achievement );
            }
        }

        public Achievement GetAchievement( Achievement.AchievementType type )
        {
            return achievements.TryGetValue ( type , out var achievement ) ? achievement : null;
        }

        public void RegisterCard( CardAchievement card )
        {
            if ( !achievementCards.Contains ( card ) )
            {
                achievementCards.Add ( card );
            }
        }

        public void UnregisterCard( CardAchievement card )
        {
            if ( achievementCards.Contains ( card ) )
            {
                achievementCards.Remove ( card );
            }
        }

        public void CheckAndAchieve( Achievement.AchievementType type )
        {
            var achievement = GetAchievement ( type );
            if ( achievement != null && !achievement.isAchieved && achievement.CheckCondition () )
            {
                achievement.Achieve ();
                Debug.Log ( $"Achievement {type} achieved!" );
                UpdateCards ( type );
            }
        }

        private void UpdateCards( Achievement.AchievementType type )
        {
            foreach ( var card in achievementCards )
            {
                if ( card.GetAchievementType () == type )
                {
                    card.UpdateUI ();
                }
            }
        }
    }
