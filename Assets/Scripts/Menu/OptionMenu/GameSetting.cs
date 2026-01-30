using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSetting : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;

    [Header("Value Labels")]
    [SerializeField] private TextMeshProUGUI volumeValueText;
    [SerializeField] private TextMeshProUGUI musicVolumeValueText;
    [SerializeField] private TextMeshProUGUI soundEffectVolumeValueText;
    [SerializeField] private TextMeshProUGUI cheatModeValueText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 100;
            volumeSlider.value = GameManager.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);

            musicVolumeSlider.minValue = 0;
            musicVolumeSlider.maxValue = 100;
            musicVolumeSlider.value = GameManager.Instance.MusicVolume;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChanged);

            soundEffectVolumeSlider.minValue = 0;
            soundEffectVolumeSlider.maxValue = 100;
            soundEffectVolumeSlider.value = GameManager.Instance.SoundEffectVolume;
            soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeSliderChanged);
        }

        RefreshUI();
    }

    public void ToggleCheatModeLeft()
    {
        ToggleCheatMode();
    }

    public void ToggleCheatModeRight()
    {
        ToggleCheatMode();
    }

    public void OnVolumeSliderChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Volume = intValue;
        }

        if (volumeValueText != null)
        {
            volumeValueText.text = intValue.ToString();
        }
    }

    public void OnMusicVolumeSliderChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.MusicVolume = intValue;
        }

        if (musicVolumeValueText != null)
        {
            musicVolumeValueText.text = intValue.ToString();
        }
    }

    public void OnSoundEffectVolumeSliderChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SoundEffectVolume = intValue;
        }

        if (soundEffectVolumeValueText != null)
        {
            soundEffectVolumeValueText.text = intValue.ToString();
        }
    }

    private void ToggleCheatMode()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.CheatMode = !GameManager.Instance.CheatMode;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (volumeValueText != null)
        {
            volumeValueText.text = GameManager.Instance.Volume.ToString();
        }

        if (musicVolumeValueText != null)
        {
            musicVolumeValueText.text = GameManager.Instance.MusicVolume.ToString();
        }

        if (soundEffectVolumeValueText != null)
        {
            soundEffectVolumeValueText.text = GameManager.Instance.SoundEffectVolume.ToString();
        }

        if (cheatModeValueText != null)
        {
            cheatModeValueText.text = GameManager.Instance.CheatMode ? "开" : "关";
        }
    }
}
