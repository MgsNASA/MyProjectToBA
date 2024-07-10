using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Start( )
    {
        // Инициализация слайдеров текущими значениями громкости из сохранений
        musicSlider.value = PlayerPrefs.GetFloat ( "MusicVolume" , 1f );
        soundSlider.value = PlayerPrefs.GetFloat ( "SoundVolume" , 1f );

        // Добавление слушателей для изменения громкости
        musicSlider.onValueChanged.AddListener ( OnMusicSliderValueChanged );
        soundSlider.onValueChanged.AddListener ( OnSoundSliderValueChanged );
    }

    private void OnDestroy( )
    {
        // Удаление слушателей, чтобы избежать утечек памяти
        musicSlider.onValueChanged.RemoveListener ( OnMusicSliderValueChanged );
        soundSlider.onValueChanged.RemoveListener ( OnSoundSliderValueChanged );
    }

    private void OnMusicSliderValueChanged( float value )
    {
        AudioManager.Instance.SetMusicVolume ( value );
        PlayerPrefs.SetFloat ( "MusicVolume" , value ); // Сохранение громкости музыки
        PlayerPrefs.Save ();
    }

    private void OnSoundSliderValueChanged( float value )
    {
        AudioManager.Instance.SetSoundVolume ( value );
        PlayerPrefs.SetFloat ( "SoundVolume" , value ); // Сохранение громкости звуков
        PlayerPrefs.Save ();
    }
}
