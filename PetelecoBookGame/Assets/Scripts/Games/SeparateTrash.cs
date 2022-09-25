// System
using System;
using System.Linq;
using System.Collections.Generic;
// Unity
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// Assets
using Assets.Enums;
using Assets.Constants;
using Assets.Scripts;
using UnityEngine.SceneManagement;


// Types of thrash in game
public enum TrashType
{
	GLASS,
	METAL,
	ORGANIC,
	PAPER,
	PLASTIC,
	NOT_RECYCABLE
};

// Trash description for Editor Refs
[Serializable]
public class Trash
{
	public TrashType Type;
	public Sprite Sprite;
};

// TrashCan description for Editor Refs
[Serializable]
public class TrashCan
{
	public TrashType Type;
	public GameObject Obj;
}


public class SeparateTrash : MonoBehaviour
{
	#region GameEditorRefs

	[SerializeField]
	private List<Trash> Trashes;

	[SerializeField]
	private List<TrashCan> Cans;

	[SerializeField]
	private TMP_Text ScoreTxt;

	#endregion

	#region GameMembers

	// Score and advance control
	private int Score = 0;
	private int TrashIndex = 0;

	// Trash object
	private GameObject Trash;
	private TrashType CurrentTrashType;

	// Colision
	private bool IsHoldingTrash = false;

	// Flow control
	private bool GameFinished = false;
	private bool IsGameRunning = true;

	EventSystem m_EventSystem;
	GraphicRaycaster m_Raycaster;
	PointerEventData m_PointerEventData;

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
		StartAudioSystem();

		AudioManager.Instance.PlayAudio(AudioSources.CALM_LOOP, true);

		// Fetch the Raycaster from the GameObject (the Canvas)
		m_Raycaster = GetComponent<GraphicRaycaster>();
		// Fetch the Event System from the Scene
		m_EventSystem = GetComponent<EventSystem>();

		Modal = new ModalControl(BlurImg, PauseModal, 0.4f);

		Modal.OnOpen = OnOpenModal;
		Modal.OnClose = OnCloseModal;

		ResetGame();

		SetupUICallbacks();
	}

	// Update is called once per frame
	void Update()
	{
		Modal?.Update();

		if (!IsGameRunning)
		{
			return;
		}

		// Listen for start touch event
		if (!IsHoldingTrash && (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
		{
			//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
			foreach (RaycastResult result in GetCollisions())
			{
				if (result.gameObject.name == "Trash")
				{
					IsHoldingTrash = true;
					Trash.transform.position = Input.mousePosition;
				}
			}
		}

		// Update trash position
		if (IsHoldingTrash && Input.touchCount > 0)
		{
			Trash.transform.position = Input.GetTouch(0).position;
		}

		// Listen for end touch event
		if (IsHoldingTrash && (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
		{
			IsHoldingTrash = false;

			//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
			foreach (RaycastResult result in GetCollisions())
			{
				string name = result.gameObject.name;

				if (name != "Trash")
				{
					if (GetCanType(name) == CurrentTrashType)
					{
						++Score;
						ScoreTxt.text = "Pontuação: " + Score;

						DestroyTrash();

						++TrashIndex;

						if (TrashIndex > 17)
						{
							FinishGame();
							return;
						}

						// Creates first trash
						CurrentTrashType = Trashes[TrashIndex].Type;
						CreateNewTrash(Trashes[TrashIndex].Sprite);
					}
					else
					{
						Handheld.Vibrate();
						--Score;
						if (Score < 0)
						{
							Score = 0;
						}
						ScoreTxt.text = "Pontuação: " + Score;
					}
				}
			}
		}
	}

	// Starts audios
	private void StartAudioSystem()
	{
		var manager = AudioManager.Instance;

		MusicVolumeSlider.value = manager.StartMusicSystem();
		SoundVolumeSlider.value = manager.StartSoundSystem();
	}

	#region GameLogic

	private void ResetGame()
	{
		Score = 0;
		TrashIndex = 0;

		// Sort trashes
		var rnd = new System.Random();
		Trashes = Trashes.OrderBy(e => rnd.Next()).ToList();

		// Creates first trash
		CurrentTrashType = Trashes[TrashIndex].Type;
		CreateNewTrash(Trashes[TrashIndex].Sprite);

		ScoreTxt.text = "Pontuação: " + Score;
	}

	private void FinishGame()
	{
		AudioManager.Instance.PlayAudio(AudioSources.GAME_START, false);

		var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (cur < 6)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 6);
		}

		IsGameRunning = false;
		GameFinished = true;
		Modal.Open();
	}

	private TrashType GetCanType(string name)
	{
		switch (name)
		{
			case "Glass":
				return TrashType.GLASS;
			case "Metal":
				return TrashType.METAL;
			case "Organic":
				return TrashType.ORGANIC;
			case "Paper":
				return TrashType.PAPER;
			case "Plastic":
				return TrashType.PLASTIC;
			case "NotRecycable":
				return TrashType.NOT_RECYCABLE;
		}

		return TrashType.GLASS;
	}

	private List<RaycastResult> GetCollisions()
	{
		//Set up the new Pointer Event
		m_PointerEventData = new PointerEventData(m_EventSystem);
		//Set the Pointer Event Position to that of the mouse position
		m_PointerEventData.position = Input.mousePosition;

		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		m_Raycaster.Raycast(m_PointerEventData, results);

		return results;
	}

	void CreateNewTrash(Sprite s)
	{
		Trash = new GameObject("Trash");
		Trash.transform.SetParent(GameObject.Find("TrashStorage").transform);

		RectTransform rect = Trash.AddComponent<RectTransform>();
		rect.localScale = Vector3.one;
		rect.anchorMin = new Vector2(0.5f, 0f);
		rect.anchorMax = new Vector2(0.5f, 0f);
		rect.anchoredPosition = new Vector2(0f, 100f);
		rect.sizeDelta = new Vector2(130, 130);

		Image image = Trash.AddComponent<Image>();
		image.sprite = s;
	}

	void DestroyTrash()
	{
		Trash.SetActive(false);
		Destroy(Trash);
	}

	private void SetupUICallbacks()
	{
		MusicVolumeSlider.onValueChanged.AddListener(OnMusicSlideControl);
		SoundVolumeSlider.onValueChanged.AddListener(OnSoundControlSlide);
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
		int record = PlayerPrefs.GetInt(PlayerPrefsKeys.SeparateTrashRecord);

		if (!GameFinished)
		{
			PlayBtnImage.sprite = PlayIconSprite;

			if (record == 0)
			{
				RecordText.text = "Recorde: 0";
			}
			else
			{
				RecordText.text = "Recorde: " + record.ToString();
			}

			PointsText.text = "Pontuação atual: " + Score;
		}
		else
		{
			PlayBtnImage.sprite = RestartIconSprite;

			if (Score > record)
			{
				PlayerPrefs.SetInt(PlayerPrefsKeys.SeparateTrashRecord, Score);
				RecordText.text = "Novo Recorde!";
			}
			else
			{
				RecordText.text = "Recorde: " + Score;
			}

			PointsText.text = "Pontuação: " + Score;
		}
	}

	public void PlayBtnEffect()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
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

