// System
using System.Linq;
using System.Collections.Generic;
// Unity
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Assets
using Assets.Scripts;
using Assets.Constants;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using Assets.Enums;

public enum GameStates
{
    TUTORIAL = 0,
    SHOWING_CARD,
    SELECTING_OBJECTS,
    GAME_ENDED
}

[Serializable]
public enum SelectableObjects
{
    BASKETBALL = 0,
    DOG,
    SKATE,
    GLOVES,
    FLIP_FLOPS,
}

[Serializable]
public class Card
{
    public Sprite sprite;
    public SelectableObjects answer;
}

public class CardsGame : MonoBehaviour
{
    #region GameVariables

    private GameStates GameState;

    private bool IsGameRunning = false;

    private float TimeLeft = 60.0f;

    [SerializeField]
    private List<Card> Cards;

    private List<Card> ShuffledCards;

    [SerializeField]
    private Card CurrentCard;

    [SerializeField]
    private Image CurrentCardImage;

    [SerializeField]
    private TMP_Text ScoreText;

    [SerializeField]
    private TMP_Text TimeLeftText;

    private int Score = 0;

    private float Timer = 1.0f;

    #endregion

    #region TutorialVariables

    [SerializeField]
    GameObject TutorialModal;

    [SerializeField]
    GameObject FirstTutorialPanel;

    [SerializeField]
    GameObject SecondTutorialPanel;

    #endregion

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
    private TMP_Text BottomText;

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
        ShuffledCards = new List<Card>();

		AudioManager.Instance.PlayAudio(AudioSources.CALM_LOOP, true);

		FirstOpenGame();

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
        HandleAppearing();
    }

    #region GameLogic

    // checks if should open tutorial or not
    // called on first starting game
    private void FirstOpenGame()
    {
        bool alreadySawTutorial = PlayerPrefs.GetInt(PlayerPrefsKeys.AlreadySawCardsGameTutorial) == 1;

        // If has already seen tutorial, start game normally
        if (alreadySawTutorial)
        {
            StartGame();
        }

        // if player hasnt seen tutorial, show it
        else
        {
            ShowTutorial();
        }
    }

    private void StartGame()
    {
        // reset game score
        Score = 0;

        // clears shuffled sprite list for new game
        ShuffledCards.Clear();

        // shuffles sprites for new game
        ShuffleCards();

        // setup timers and game state
        GameState = GameStates.SHOWING_CARD;
        Timer = 0.0f;

        TimeLeft = 60.0f;

        IsGameRunning = true;

        ShowNextCard();

        UpdateScoreText();

	}

    private void UpdateTimers()
    {
        if (IsGameRunning)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft < 0.0f)
            {
                FinishGame();
            }
            else
            {
                TimeLeftText.text = Mathf.FloorToInt(TimeLeft).ToString() + "s";
            }
        }
    }

    // handles appearing animation of objects
    // is called every frame to update alpha
    private void HandleAppearing()
    {
        // if state is showing card, should do card animation
        if (GameState == GameStates.SHOWING_CARD)
        {
            Timer += Time.deltaTime;

            if (Timer > 0.025f)
            {
                // handles animating and fading the card
                var cardColor = CurrentCardImage.color;
                cardColor.a += 0.1f;
                CurrentCardImage.color = cardColor;

                Timer = 0.0f;

                // if card color is already fully opaque, change game state
                if (cardColor.a >= 1.0f)
                {
                    GameState = GameStates.SELECTING_OBJECTS;
                }
            }
        }
    }

    // shuffles cards
    private void ShuffleCards()
    {
        // Shuffles cards list
        System.Random random = new System.Random();
        ShuffledCards = Cards.OrderBy(c => random.Next()).ToList();
    }

    // Shows next card in shuffled cards
    // meaning it assigns its sprite to the only card on screen
    private void ShowNextCard()
    {
        // if there are no cards left, handle end game
        if (ShuffledCards.Count == 0)
        {
            FinishGame();
        }
        // if there are cards left, replace sprite
        else
        {
            // current card is now the first on the deck
            CurrentCard = ShuffledCards.First();

            // removes current card from the deck
            ShuffledCards.RemoveAt(0);

            // replace sprite with new current one
            CurrentCardImage.sprite = CurrentCard.sprite;

            // makes card fade
            CurrentCardImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        Timer = 0.0f;
        GameState = GameStates.SHOWING_CARD;
    }

    private void FinishGame()
    {
		AudioManager.Instance.PlayAudio(AudioSources.GAME_START, false);

		var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (cur < 16)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 16);
		}

		IsGameRunning = false;
        GameState = GameStates.GAME_ENDED;
        Modal.Open();
    }

    // ref set by editor on each object
    public void OnObjectClick(int objectType)
    {
        if (objectType == (int)CurrentCard.answer)
        {
            Score++;
            UpdateScoreText();
            ShowNextCard();
        }
        else
        {
            Handheld.Vibrate();
            Score--;
            if (Score < 0)
            {
                Score = 0;
            }
            UpdateScoreText();
            ShowNextCard();
        }
    }

    // updates score text with new score
    private void UpdateScoreText()
    {
        ScoreText.text = "Pontuação: " + Score;
    }

    #endregion

    #region Tutorial

    private void ShowTutorial()
    {
        GameState = GameStates.TUTORIAL;

        TutorialModal.SetActive(true);
    }

    // callback set to close tutorial button
    // editor ref
    public void CloseTutorial()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.AlreadySawCardsGameTutorial, 1);

        // disables tutorial modal
        TutorialModal.SetActive(false);

        // starts game
        StartGame();
    }

    // editor ref
    // go to second tutorial panel
    public void GoToSecondTutorialPanel()
    {
        FirstTutorialPanel.SetActive(false);
        SecondTutorialPanel.SetActive(true);
    }

    #endregion

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
        SetPausePanelTexts();
    }

    private void SetPausePanelTexts()
    {
        if (GameState == GameStates.GAME_ENDED)
        {
			// Check if player has surpassed current record
			int record = PlayerPrefs.GetInt(PlayerPrefsKeys.CardsGameRecord);

			// if player has surpassed record, set a new one with FoundObjects value
			if (Score > record)
			{
				PlayerPrefs.SetInt(PlayerPrefsKeys.CardsGameRecord, Score);
				RecordText.text = "Novo Recorde!";
				PointsText.text = "Pontuação: " + Score;
			}
			else
			{
				RecordText.text = "Recorde: " + record.ToString();
				PointsText.text = "Pontuação: " + Score;
			}

			PlayBtnImage.sprite = RestartIconSprite;
            BottomText.text = "Jogo Finalizado";
        }
        else
        {
			int record = PlayerPrefs.GetInt(PlayerPrefsKeys.CardsGameRecord);
			RecordText.text = "Recorde: " + record.ToString();

			PlayBtnImage.sprite = PlayIconSprite;
			BottomText.text = Mathf.Floor(Timer).ToString() + " Segundos Restantes";
			PointsText.text = "Pontuação: " + Score;

			PlayBtnImage.sprite = PlayIconSprite;
            BottomText.text = "";
        }
    }

	public void PlayBtnEffect()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
	}
	#endregion
}