// Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
// Enums
using Assets.Enums;
// Scripts
using Assets.Scripts;
using Assets.Constants;


public class Menu : MonoBehaviour
{
	//
	// Editor References
	//

	#region EditorRefs

	#region LandScreen

	// Background

	[SerializeField]
	private Image SunHaloImg;

	// Buttons

	[SerializeField]
	private Button PlayBtn;

	[SerializeField]
	private Button ConfigBtn;

	[SerializeField]
	private Button OutBtn;

	[SerializeField]
	private Button CameraBtn;

	#endregion

	#region ConfigModal

	[SerializeField]
	private GameObject ConfigModal;

	[SerializeField]
	private Image BlurImg;

	[SerializeField]
	private Button ConfigOkBtn;

	[SerializeField]
	private Slider MusicVolumeSlider;

	[SerializeField]
	private Slider SoundVolumeSlider;

	#endregion

	#region AudioManager

	[SerializeField]
	private GameObject AudioManagerObj;

	#endregion

	#endregion

	private ModalControl Modal;

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(AudioManagerObj);

		StartGameSystem();

		// Starts audio systems
		StartAudioSystem();

		// Starts the LandScreen callbacks
		SetupUICallbacks();

		Modal = new ModalControl(BlurImg, ConfigModal, 0.4f);
		Modal.OnOpen = SetupModalCallbacks;
		Modal.OnClose = CleanModalCallbacks;

		var manager = AudioManager.Instance;

		if (manager.GetCurrentMusic() != AudioSources.MAIN_LOOP)
		{
			manager.PlayAudio(AudioSources.MAIN_LOOP, true);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Updates halo background
		UpdateHaloRotation();

		// Update Modal Tasks
		Modal?.Update();
	}

	#region MenuLandScreen

	public void ResetProgress()
	{
		PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.MemoryGameRecord, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.FindObjectsRecord, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.FindShadowsRecord, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.ConnectPointsRecord, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.SeparateTrashRecord, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.CardsGameRecord, 0);
		PlayerPrefs.SetInt(PlayerPrefsKeys.AlreadySawCardsGameTutorial, 0);
	}

	private void StartGameSystem()
	{
		bool isFirstTime = PlayerPrefs.GetInt(PlayerPrefsKeys.STARTED) == 0;

		if (isFirstTime)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.STARTED, 1);
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 0);

			PlayerPrefs.SetFloat(PlayerPrefsKeys.NormalizedMusicVolume, 1f);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.NormalizedSoundVolume, 1f);

			PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 0);

			PlayerPrefs.SetInt(PlayerPrefsKeys.MemoryGameRecord, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.FindObjectsRecord, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.FindShadowsRecord, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.ConnectPointsRecord, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SeparateTrashRecord, 0);
		}
	}

	// Rotates the halo around the sun
	private void UpdateHaloRotation()
	{
		const float DegreesPerSecond = 2;

		float dAngle = DegreesPerSecond * Time.deltaTime;

		SunHaloImg.transform.Rotate(new Vector3(0, 0, dAngle));
	}

	// Starts audios
	private void StartAudioSystem()
	{
		var manager = AudioManager.Instance;

		MusicVolumeSlider.value = manager.StartMusicSystem();
		SoundVolumeSlider.value = manager.StartSoundSystem();
	}

	// Set all callbacks for LandScreen
	private void SetupUICallbacks()
	{
		PlayBtn.onClick.AddListener(OnPlayClick);
		ConfigBtn.onClick.AddListener(OnConfigClick);
		OutBtn.onClick.AddListener(OnOutClick);
		CameraBtn.onClick.AddListener(OnCameraClick);
	}

	// Clean all callbacks for LandScreen
	private void CleanUICallbacks()
	{
		PlayBtn.onClick.RemoveListener(OnPlayClick);
		ConfigBtn.onClick.RemoveListener(OnConfigClick);
		OutBtn.onClick.RemoveListener(OnOutClick);
		CameraBtn.onClick.RemoveListener(OnCameraClick);
	}

	// When user goes to progression roadmap screen
	private void OnPlayClick()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
		CleanUICallbacks();
		SceneManager.LoadSceneAsync((int)GameScenes.ROADMAP);
	}

	// When user open config modal
	private void OnConfigClick()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
		Modal.Open();
	}

	// When user close the app
	private void OnOutClick()
	{
		CleanUICallbacks();
		Application.Quit();
	}
	
	private void TransitionToQrReader(string permission = "")
	{
		if (Permission.HasUserAuthorizedPermission(Permission.Camera))
		{
			CleanUICallbacks();
			SceneManager.LoadSceneAsync((int)GameScenes.QR_SCANNER);
		}
	}

	// When user goes to scan screen
	private void OnCameraClick()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);

		if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
		{
			var callbacks = new PermissionCallbacks();
			callbacks.PermissionGranted += TransitionToQrReader;

			Permission.RequestUserPermission(Permission.Camera, callbacks);
		}
		else
		{
			TransitionToQrReader();
		}
	}

	public void PlayBtnEffect()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
	}

	#endregion

	#region ConfigModal

	private void SetupModalCallbacks()
	{
		ConfigOkBtn.onClick.AddListener(OnConfigCloseClick);
		MusicVolumeSlider.onValueChanged.AddListener(OnMusicSlideControl);
		SoundVolumeSlider.onValueChanged.AddListener(OnSoundControlSlide);
	}

	private void CleanModalCallbacks()
	{
		// Clean callbacks
		ConfigOkBtn.onClick.RemoveListener(OnConfigCloseClick);
		MusicVolumeSlider.onValueChanged.RemoveListener(OnMusicSlideControl);
		SoundVolumeSlider.onValueChanged.RemoveListener(OnSoundControlSlide);
	}

	private void OnConfigCloseClick()
	{
		Modal.Close();
	}

	private void OnSoundControlSlide(float value)
	{
		var manager = AudioManager.Instance;

		manager.SetSoundVolume(value);
	}

	private void OnMusicSlideControl(float value)
	{
		var manager = AudioManager.Instance;

		manager.SetMusicVolume(value);
	}

	#endregion
}
