// System
using System.Linq;
using System.Collections.Generic;
// Unity
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
// Assets
using Assets.Scripts;
using Assets.Constants;
using Assets.Enums;

public class MemoryGame : MonoBehaviour
{
    private enum GameStates
    {
        SHOWING_CARDS = 0,
        SELECTING_FIRST_CARD,
        SELECTING_SECOND_CARD,
        SELECTED_WRONG_CARDS,
        GAME_ENDED
    }

    private GameStates GameState = GameStates.SHOWING_CARDS;

    private int Score = 0;

    private bool IsGameRunning = true;

    private float Timer = 4.0f;

    // Stores the time the game has been running, for record purposes
    private float GameRunnningTime = 0.0f;

    private int FirstSelectedCardIndex;
    private int SecondSelectedCardIndex;

    [SerializeField]
    private List<Image> Cards;

    [SerializeField]
    private List<Sprite> CardSprites;

    [SerializeField]
    private Sprite BackCardSprite;

    private List<Sprite> ShuffledSprites;

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
        ShuffledSprites = new List<Sprite>();

		AudioManager.Instance.PlayAudio(AudioSources.MEMORY_LOOP, true);

		StartGame();

        StartAudioSystem();

        Modal = new ModalControl(BlurImg, PauseModal, 0.4f);

        Modal.OnOpen = OnOpenModal;
        Modal.OnClose = OnCloseModal;

		MusicVolumeSlider.onValueChanged.AddListener(OnMusicSlideControl);
		SoundVolumeSlider.onValueChanged.AddListener(OnSoundControlSlide);
	}

    // Update is called once per frame
    void Update()
    {
        Modal?.Update();
        UpdateTimers();
    }

    #region GameLogic

    // Resets scores and variables, shuffles cards, shows cards and sets game state
    private void StartGame()
    {
        // If Game was previously ended, should enable all buttons first
        if (GameState == GameStates.GAME_ENDED)
        {
            EnableAllCards();
        }

        // Resets game running time
        GameRunnningTime = 0.0f;

        // reset game score
        Score = 0;

        // clears shuffled sprite list for new game
        ShuffledSprites.Clear();

        // shuffles sprites for new game
        ShuffleSprites();

        // setup timers and game state
        GameState = GameStates.SHOWING_CARDS;
        Timer = 4.0f;

        // show all card faces
        ShowAllCards();
    }

    private void UpdateTimers()
    {
        // if game isnt running timers shouldnt be updated
        if (!IsGameRunning)
        {
            return;
        }

        GameRunnningTime += Time.deltaTime;

        if (GameState == GameStates.SHOWING_CARDS)
        {
            Timer -= Time.deltaTime;

            if (Timer < 0)
            {
                HideAllCards();
                GameState = GameStates.SELECTING_FIRST_CARD;
            }
        }
        else if (GameState == GameStates.SELECTED_WRONG_CARDS)
        {
            Timer -= Time.deltaTime;

            if (Timer < 0)
            {
                GameState = GameStates.SELECTING_FIRST_CARD;
                // Make selected  buttons show backcard again
                Cards[FirstSelectedCardIndex].sprite = BackCardSprite;
                Cards[SecondSelectedCardIndex].sprite = BackCardSprite;
            }
        }
    }

    private void ShuffleSprites()
    {
        // Shuffles list of range 0 to Input - 1 and gets first 8
        System.Random random = new System.Random();
        List<int> chosenIndexes = Enumerable.Range(0, CardSprites.Count - 1)
                                    .OrderBy(c => random.Next())
                                    .Take(8)
                                    .ToList();

        // doubles shuffled list so we get 2 of each number
        List<int> shuffledIndexes = chosenIndexes.Concat(chosenIndexes.OrderBy(c => random.Next())).ToList();

        // adds sprite of given index to list of shuffled sprites
        foreach (int index in shuffledIndexes)
        {
            ShuffledSprites.Add(CardSprites[index]);
        }
    }

    private void ShowAllCards()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Image cardImage = Cards[i];
            cardImage.sprite = ShuffledSprites[i];
        }
    }

    private void HideAllCards()
    {
        foreach (Image card in Cards)
        {
            card.sprite = BackCardSprite;
        }
    }

    private void EnableAllCards()
    {
        foreach (Image card in Cards)
        {
            card.GetComponent<Button>().interactable = true;
        }
    }

    public void OnCardClick(int index)
    {
        if (GameState == GameStates.SELECTING_FIRST_CARD)
        {
            // sets first selected card index and its sprite
            FirstSelectedCardIndex = index;
            var firstSelectedCard = Cards[index];

            firstSelectedCard.sprite = ShuffledSprites[index];

            // sets next game state
            GameState = GameStates.SELECTING_SECOND_CARD;
        }

        else if (GameState == GameStates.SELECTING_SECOND_CARD)
        {
            // if player pressed the same button, shouldnt do anything
            if (index == FirstSelectedCardIndex)
            {
                return;
            }
            // sets second selected card and its sprite
            SecondSelectedCardIndex = index;
            Image secondSelectedCard = Cards[index];

            secondSelectedCard.sprite = ShuffledSprites[index];

            Image firstSelectedCard = Cards[FirstSelectedCardIndex];

            // check for matching card
            if (secondSelectedCard.sprite.name == firstSelectedCard.sprite.name)
            {
                secondSelectedCard.GetComponent<Button>().interactable = false;
                firstSelectedCard.GetComponent<Button>().interactable = false;

                Score++;

                if (Score == Cards.Count / 2)
                {
                    HandleWinGame();
                }
                else
                {
                    GameState = GameStates.SELECTING_FIRST_CARD;
                }
            }
            // if we dont have a match, change game state
            else
            {
                GameState = GameStates.SELECTED_WRONG_CARDS;
                Timer = 1.5f;
            }
        }
    }

    private void HandleWinGame()
    {
		AudioManager.Instance.PlayAudio(AudioSources.GAME_START, false);

		var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (cur < 7)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 7);
		}

		GameState = GameStates.GAME_ENDED;

        int currentRecord = PlayerPrefs.GetInt(PlayerPrefsKeys.MemoryGameRecord);

        Modal.Open();
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
        if (GameState == GameStates.GAME_ENDED)
        {
            StartGame();
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
        if (GameState == GameStates.GAME_ENDED)
        {
			int record = PlayerPrefs.GetInt(PlayerPrefsKeys.MemoryGameRecord);

			if (Mathf.Floor(GameRunnningTime) < record || record == 0)
            {
				PlayerPrefs.SetInt(PlayerPrefsKeys.MemoryGameRecord, Mathf.FloorToInt(GameRunnningTime));
				RecordText.text = "Novo Recorde!";
                PointsText.text = "Tempo Atual: " + Mathf.Floor(GameRunnningTime).ToString() + "s";
			}
            else
            {
				RecordText.text = "Recorde: " + record.ToString() + "s";
				PointsText.text = "Tempo Atual: " + Mathf.Floor(GameRunnningTime).ToString() + "s";
			}

            PlayBtnImage.sprite = RestartIconSprite;
            TimeLeftText.text = "Jogo Finalizado";
        }
        else
        {
			int record = PlayerPrefs.GetInt(PlayerPrefsKeys.MemoryGameRecord);
			RecordText.text = "Recorde: " + record.ToString() + "s";

			PlayBtnImage.sprite = PlayIconSprite;
			PointsText.text = "Tempo Atual: " + Mathf.Floor(GameRunnningTime).ToString() + "s";
			TimeLeftText.text = "";
		}
    }

	public void PlayBtnEffect()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
	}
	#endregion
}
