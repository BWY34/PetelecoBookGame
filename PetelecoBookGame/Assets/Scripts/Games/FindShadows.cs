// System
using System.Linq;
// Unity
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;
using UnityEngine.SceneManagement;
// Assets
using Assets.Enums;
using Assets.Constants;

public class FindShadows : MonoBehaviour
{
    private int FoundObjects = 0;

    private bool HasGameEnded = false;

    private bool IsGameRunning = true;

    // Box that contains all the shadow objects
    [SerializeField]
    private GameObject BoxObj;

    // Game Object that is parent to all clickable objects in scene
    [SerializeField]
    private GameObject ObjectsObj;

    [SerializeField]
    private TMP_Text CountdownText;

    private float Timer = 45.0f;

    #region ConfigModal

    [SerializeField]
    private GameObject PauseModal;

    [SerializeField]
    private Image BlurImg;

    private ModalControl Modal;

    [SerializeField]
    private Slider MusicVolumeSlider;

    [SerializeField]
    private Slider SoundVolumeSlider;

    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject ConfigPanel;

    [SerializeField]
    private TMP_Text PointsText;

    [SerializeField]
    private TMP_Text RecordText;

    [SerializeField]
    private TMP_Text TimeLeftText;

    [SerializeField]
    private Image PlayBtnImage;

    [SerializeField]
    private Sprite PlayIconSprite;

    [SerializeField]
    private Sprite RestartIconSprite;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartAudioSystem();

		AudioManager.Instance.PlayAudio(AudioSources.MEMORY_LOOP, true);

		Modal = new ModalControl(BlurImg, PauseModal, 0.4f);

        Modal.OnOpen = OnOpenModal;
        Modal.OnClose = OnCloseModal;

		MusicVolumeSlider.onValueChanged.AddListener(OnMusicSlideControl);
		SoundVolumeSlider.onValueChanged.AddListener(OnSoundControlSlide);
	}

    // Update is called once per frame
    void Update()
    {
        HandleCountdown();
        Modal?.Update();
    }

    #region GameLogic

    public void OnObjectClick(GameObject Object)
    {
        // sets image as disable so object dissapears from scene
        Image image = Object.GetComponent<Image>();
        image.enabled = false;

        // reduces opacity of coresponding object on ui box
        SetEquivalentObjectAsClicked(Object.name);

        FoundObjects++;

        // If player has found all objects, end game
        if (FoundObjects == 14)
        {
            HandleEndGame();
        }
    }

    private void HandleEndGame()
    {
		AudioManager.Instance.PlayAudio(AudioSources.GAME_START, false);

		var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (cur < 5)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 5);
		}

        HasGameEnded = true;
        Modal.Open();
    }

    private void SetEquivalentObjectAsClicked(string ObjName)
    {
        // gets equivalent object
        string boxObjName = ObjName + "Box";

        GameObject boxObj = GameObject.Find(boxObjName);
        Image image = boxObj.GetComponent<Image>();

        // makes image not black anymore
        var imageColor = new Color(1f, 1f, 1f);
        image.color = imageColor;
    }

    // handles countdown updating
    private void HandleCountdown()
    {
        if (IsGameRunning)
        {
            Timer -= Time.deltaTime;

            if (Timer < 0)
            {
				AudioManager.Instance.PlayAudio(AudioSources.GAME_START, false);

				var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

				if (cur < 5)
				{
					PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 5);
				}

				HasGameEnded = true;
                Modal.Open();
            }
            else
            {
                CountdownText.text = Mathf.Floor(Timer).ToString() + "s";
            }
        }
    }

    private void RestartGame()
    {
        // paints box images black
        PaintBoxObjectsBlack();

        // re enable all clickable objects
        EnableAllClickableObjects();

        // reset game variables
        Timer = 45f;
        FoundObjects = 0;
        HasGameEnded = false;
    }

    private void PaintBoxObjectsBlack()
    {
        // skip(1) because first element of list is the box image
        var childImages = BoxObj.GetComponentsInChildren<Image>().Skip(1);

        // Paint all box Images black
        foreach (Image image in childImages)
        {
            image.color = new Color(0f, 0f, 0f);
        }
    }
    private void EnableAllClickableObjects()
    {
        var childImages = ObjectsObj.GetComponentsInChildren<Image>();

        // Enable all child images
        foreach (Image image in childImages)
        {
            image.enabled = true;
        }
    }
	#endregion

	public void PlayBtnEffect()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
	}

	private void StartAudioSystem()
    {
        var manager = AudioManager.Instance;

        MusicVolumeSlider.value = manager.StartMusicSystem();
        SoundVolumeSlider.value = manager.StartSoundSystem();
    }

    #region ModalLogic

    public void OnPauseClick()
    {
        Modal.Open();
    }

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

    public void OnPlayButtonClick()
    {
        if (HasGameEnded)
        {
            RestartGame();
        }

        Modal.Close();
    }

    public void OnHomeButtonClick()
    {
		SceneManager.LoadSceneAsync((int)GameScenes.MENU);
	}

    public void OnConfigButtonClick()
    {
        if (ConfigPanel.activeSelf)
        {
            ConfigPanel.SetActive(false);
            PausePanel.SetActive(true);
        }
        else
        {
            PausePanel.SetActive(false);
            ConfigPanel.SetActive(true);
        }
    }

    private void OnOpenModal()
    {
        IsGameRunning = false;
        SetPausePanelTexts();
    }

    private void OnCloseModal()
    {
		ConfigPanel.SetActive(false);
		PausePanel.SetActive(true);

		IsGameRunning = true;
    }

    private void SetPausePanelTexts()
    {
		if (HasGameEnded)
		{
			// Check if player has surpassed current record
			int record = PlayerPrefs.GetInt(PlayerPrefsKeys.FindShadowsRecord);

			// if player has surpassed record, set a new one with FoundObjects value
			if (FoundObjects > record)
			{
				PlayerPrefs.SetInt(PlayerPrefsKeys.FindShadowsRecord, FoundObjects);
				RecordText.text = "Novo Recorde!";
				PointsText.text = "Pontuação: " + FoundObjects;
			}
			else
			{
				RecordText.text = "Recorde: " + record.ToString();
				PointsText.text = "Pontuação: " + FoundObjects;
			}

			PlayBtnImage.sprite = RestartIconSprite;
			TimeLeftText.text = FoundObjects == 14 ? "Jogo Finalizado" : "Restam: " + (14 - FoundObjects);
		}
		else
		{
			int record = PlayerPrefs.GetInt(PlayerPrefsKeys.FindShadowsRecord);
			RecordText.text = "Recorde: " + record.ToString();

			PlayBtnImage.sprite = PlayIconSprite;
			TimeLeftText.text = Mathf.Floor(Timer).ToString() + " Segundos Restantes";
			PointsText.text = "Pontuação: " + FoundObjects.ToString();
		}
	}
    #endregion
}
