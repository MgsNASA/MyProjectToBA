using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get; private set;
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range ( 0f , 1f )] public float volume = 1f;
        [Range ( 0.1f , 3f )] public float pitch = 1f;
        public bool loop = false;
        public bool isMusic = false;
        public bool playOnAwake = false;

        [HideInInspector] public AudioSource source;
    }

    public List<Sound> sounds;

    private float musicVolume = 1f;
    private float soundVolume = 1f;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad ( gameObject );
        }
        else
        {
            Destroy ( gameObject );
            return;
        }

        foreach ( Sound s in sounds )
        {
            s.source = gameObject.AddComponent<AudioSource> ();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;

            if ( s.playOnAwake )
            {
                s.source.Play ();
            }
        }

        // Загрузка сохраненных значений громкости
        musicVolume = PlayerPrefs.GetFloat ( "MusicVolume" , 1f );
        soundVolume = PlayerPrefs.GetFloat ( "SoundVolume" , 1f );
        ApplyVolumes ();
    }

    public void Play( string name )
    {
        Sound s = sounds.Find ( sound => sound.name == name );
        if ( s == null )
        {
            Debug.LogWarning ( "Sound: " + name + " not found!" );
            return;
        }
        s.source.Play ();
    }

    public void Stop( string name )
    {
        Sound s = sounds.Find ( sound => sound.name == name );
        if ( s == null )
        {
            Debug.LogWarning ( "Sound: " + name + " not found!" );
            return;
        }
        s.source.Stop ();
    }

    public void SetMusicVolume( float volume )
    {
        musicVolume = volume;
        ApplyVolumes ();
        PlayerPrefs.SetFloat ( "MusicVolume" , volume );
        PlayerPrefs.Save ();
    }

    public void SetSoundVolume( float volume )
    {
        soundVolume = volume;
        ApplyVolumes ();
        PlayerPrefs.SetFloat ( "SoundVolume" , volume );
        PlayerPrefs.Save ();
    }

    private void ApplyVolumes( )
    {
        foreach ( Sound s in sounds )
        {
            if ( s.isMusic )
            {
                s.source.volume = musicVolume;
            }
            else
            {
                s.source.volume = soundVolume;
            }
        }
    }

    public float GetMusicVolume( )
    {
        return musicVolume;
    }

    public float GetSoundVolume( )
    {
        return soundVolume;
    }
}
