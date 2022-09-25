// System
using System.Linq;
// Unity
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Assets
using Assets.Enums;
using Assets.Scripts;
using Assets.Constants;


public class ConnectPoints : MonoBehaviour
{
	#region GameEditorRefs

	// Infos for player
	[SerializeField]
	private Text NextPointText;

	// Lines parent
	[SerializeField]
	private GameObject Lines;

	// Points parent
	[SerializeField]
	private GameObject Points;

	#endregion

	#region GameMembers

	// Flow controls
	private int CurrentPointIndex = 0;

	private bool GameFinished = false;
	private bool IsGameRunning = false;

	// Metrics
	private float Timer = 0f;

	// Points
	private string[] PointsLabel = new string[]
	{
		"E2", "F1", "H0", "J1", "K3", "L4", "M5", "N7", "M8", "O9",
		"K10", "J11", "K12", "L13", "M14", "N16", "L16", "I16", "F16",
		"F15", "F14", "D15", "C14", "B13", "C12", "D12", "D11", "E9",
		"B9", "C7", "E6", "C6", "C4", "D4", "E3"
	};

	#endregion

	#region ConfigModal

	// Outside modal
	[SerializeField]
	private Image BlurImg;

	[SerializeField]
	private GameObject PauseModal;

	// Configs
	[SerializeField]
	private Slider MusicVolumeSlider;

	[SerializeField]
	private Slider SoundVolumeSlider;

	[SerializeField]
	private GameObject ConfigPanel;

	// Game pause info screen
	[SerializeField]
	private GameObject PausePanel;

	[SerializeField]
	private TMP_Text PointsText;

	[SerializeField]
	private TMP_Text RecordText;

	// Uderline buttons controls
	[SerializeField]
	private Image PlayBtnImage;

	[SerializeField]
	private Sprite PlayIconSprite;

	[SerializeField]
	private Sprite RestartIconSprite;

	// Modal controller
	private ModalControl Modal;

	#endregion

	// Start is called before the first frame update
	void Start()
	{
		AudioManager.Instance.PlayAudio(AudioSources.CALM_LOOP, true);

		StartAudioSystem();

		Modal = new ModalControl(BlurImg, PauseModal, 0.4f);

		Modal.OnOpen = OnOpenModal;
		Modal.OnClose = OnCloseModal;

		ResetGame();

		SetupUICallbacks();
	}

	void Update()
	{
		if (IsGameRunning)
		{
			Timer += Time.deltaTime;
		}

		Modal?.Update();
	}

	#region GameLogic

	// Cleans every members and restart game
	private void ResetGame()
	{
		Timer = 0f;
		IsGameRunning = true;
		GameFinished = false;
		CurrentPointIndex = 0;

		foreach (var line in Lines.GetComponentsInChildren<Transform>(true).Skip(1))
		{
			line.gameObject.SetActive(false);
		}

		for (int i = 0; i < 35; ++i)
		{
			var btn = GameObject.Find("Button (" + i + ")");
			btn.GetComponent<Button>().interactable = true;
			btn.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
		}

		NextPointText.text = "Próximo\r\nPonto:\r\n" + PointsLabel[CurrentPointIndex];
	}

	private void FinishGame()
	{
		AudioManager.Instance.PlayAudio(AudioSources.GAME_START, false);

		var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (cur < 2)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 2);
		}

		IsGameRunning = false;
		GameFinished = true;
		Modal.Open();
	}

	public void OnPointClick(int index)
	{
		if (CurrentPointIndex != index)
		{
			Handheld.Vibrate();
		}
		else
		{
			var btn = GameObject.Find("Button (" + CurrentPointIndex + ")");
			btn.GetComponent<Button>().interactable = false;
			btn.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);

			if (CurrentPointIndex == 34)
			{
				var line1 = Lines.GetComponentsInChildren<Transform>(true)
							.FirstOrDefault(e => e.name == ("Line (" + (CurrentPointIndex - 1) + ")"))
							.gameObject;
				line1.SetActive(true);
				var line2 = Lines.GetComponentsInChildren<Transform>(true)
							.FirstOrDefault(e => e.name == ("Line (" + (CurrentPointIndex) + ")"))
							.gameObject;
				line2.SetActive(true);

				FinishGame();
				return;
			}

			NextPointText.text = "Clique no\r\nPonto:\r\n" + PointsLabel[CurrentPointIndex + 1];

			if (CurrentPointIndex == 0)
			{
				++CurrentPointIndex;
				return;
			}

			var line = Lines.GetComponentsInChildren<Transform>(true)
							.FirstOrDefault(e => e.name == ("Line (" + (CurrentPointIndex - 1) + ")"))
							.gameObject;
			line.SetActive(true);

			++CurrentPointIndex;
		}
	}

	// UI

	public void PlayBtnEffect()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
	}

	private void SetupUICallbacks()
	{
		MusicVolumeSlider.onValueChanged.AddListener(OnMusicSlideControl);
		SoundVolumeSlider.onValueChanged.AddListener(OnSoundControlSlide);
	}

	// Starts audios
	private void StartAudioSystem()
	{
		var manager = AudioManager.Instance;

		MusicVolumeSlider.value = manager.StartMusicSystem();
		SoundVolumeSlider.value = manager.StartSoundSystem();
	}

	#endregion

	#region ModalLogic

	// Editor ref
	public void OnPauseClick()
	{
		Modal.Open();
	}

	// Editor ref
	public void OnPlayButtonClick()
	{
		if (GameFinished)
		{
			ResetGame();
		}

		Modal.Close();
	}

	// Editor ref
	public void OnHomeButtonClick()
	{
		SceneManager.LoadSceneAsync((int)GameScenes.MENU);
	}

	// Editor Ref
	public void OnConfigButtonClick()
	{
		PausePanel.SetActive(ConfigPanel.activeSelf);
		ConfigPanel.SetActive(!ConfigPanel.activeSelf);
	}

	// Modal Open Callback
	private void OnOpenModal()
	{
		IsGameRunning = false;
		SetPausePanelTexts();
	}

	// Modal Close Callback
	private void OnCloseModal()
	{
		ConfigPanel.SetActive(false);
		PausePanel.SetActive(true);

		IsGameRunning = true;
	}

	private void SetPausePanelTexts()
	{
		int record = PlayerPrefs.GetInt(PlayerPrefsKeys.ConnectPointsRecord);

		if (!GameFinished)
		{
			PlayBtnImage.sprite = PlayIconSprite;

			if (record == 0)
			{
				RecordText.text = "Recorde: ---";
			}
			else
			{
				RecordText.text = "Recorde: " + record.ToString() + "s";
			}

			PointsText.text = "Pontuação atual: " + Mathf.Floor(Timer) + "s";
		}
		else
		{
			PlayBtnImage.sprite = RestartIconSprite;

			if (Mathf.Floor(Timer) < record || record == 0)
			{
				PlayerPrefs.SetInt(PlayerPrefsKeys.ConnectPointsRecord, Mathf.FloorToInt(Timer));
				RecordText.text = "Novo Recorde!";
			}
			else
			{
				RecordText.text = "Recorde: " + record.ToString() + "s";
			}

			PointsText.text = "Pontuação: " + Mathf.Floor(Timer) + "s";
		}
	}

	// Sounds configs
	public void OnSoundControlSlide(float value)
	{
		var manager = AudioManager.Instance;

		manager.SetSoundVolume(value);
	}

	public void OnMusicSlideControl(float value)
	{
		var manager = AudioManager.Instance;

		manager.SetMusicVolume(value);
	}

	#endregion
}
