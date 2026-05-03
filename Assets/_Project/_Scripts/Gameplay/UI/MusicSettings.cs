using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{
	[SerializeField] private AudioMixer mixer;
	[SerializeField] private GameObject pausePanel;

	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider masterSlider;

	private void Start()
	{
		sfxSlider.value = 0.5f;
		musicSlider.value = 0.5f;
		masterSlider.value = 0.5f;
		OnSfxSliderChanged(sfxSlider.value);
		OnMusicSliderChanged(musicSlider.value);
		OnMasterSliderChanged(masterSlider.value);
		if (SceneManager.GetActiveScene().name != Constants.MainMenuSceneName) pausePanel.SetActive(false);

		sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
		musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
		masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
	}

	private void OnSfxSliderChanged(float value)
	{
		mixer.SetFloat("SoundVolume", ToDecibels(value));
	}
	private void OnMusicSliderChanged(float value)
	{
		mixer.SetFloat("MusicVolume", ToDecibels(value));
	}
	private void OnMasterSliderChanged(float value)
	{
		mixer.SetFloat("MasterVolume", ToDecibels(value));
	}

	private float ToDecibels(float value)
	{
		if (value <= 0.0001f)
			return -80f;

		return Mathf.Log10(value) * 20f;
	}
}
