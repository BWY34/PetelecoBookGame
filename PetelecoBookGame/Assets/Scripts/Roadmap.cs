// Unity
using UnityEngine;
using UnityEngine.UI;
// Assets
using Assets.Enums;
using Assets.Constants;
using UnityEngine.SceneManagement;

public class Roadmap : MonoBehaviour
{
	#region EditorRef

	// Background

	[SerializeField]
	private Image SunHaloImg;

	// Return

	[SerializeField]
	private Button ReturnBtn;

	#endregion

	// Start is called before the first frame update
	void Start()
	{
		SetupUICallbacks();

		var currentLevel = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (currentLevel < 1 || currentLevel > 17)
		{
			currentLevel = 1;
		}

		if (currentLevel == 17)
		{
			return;
		}

		{
			var obj = GameObject.Find("Level" + currentLevel.ToString());
			obj.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
		}

		for (var i = currentLevel + 1; i < 17; ++i)
		{
			var obj = GameObject.Find("Level" + i.ToString());
			obj.GetComponent<Image>().color = new Color(0.58f, 0.58f, 0.58f, 1f);
			obj.GetComponent<Button>().interactable = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Updates halo background
		UpdateHaloRotation();
	}

	// Rotates the halo around the sun
	private void UpdateHaloRotation()
	{
		const float DegreesPerSecond = 2;

		float dAngle = DegreesPerSecond * Time.deltaTime;

		SunHaloImg.transform.Rotate(new Vector3(0, 0, dAngle));
	}

	private void SetupUICallbacks()
	{
		ReturnBtn.onClick.AddListener(OnReturnClick);
	}

	private void CleanUICallbacks()
	{
		ReturnBtn.onClick.RemoveListener(OnReturnClick);
	}

	public void OnLevelClick(int level)
	{
		switch (level)
		{
			case 1:
				SceneManager.LoadSceneAsync((int)GameScenes.CONNECT_POINTS);
				break;
			case 2:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 0);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 3:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 1);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 4:
				SceneManager.LoadSceneAsync((int)GameScenes.FIND_SHADOWS);
				break;
			case 5:
				SceneManager.LoadSceneAsync((int)GameScenes.SEPARATE_TRASH);
				break;
			case 6:
				SceneManager.LoadSceneAsync((int)GameScenes.MEMORY_GAME);
				break;
			case 7:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 2);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 8:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 3);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 9:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 4);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 10:
				SceneManager.LoadSceneAsync((int)GameScenes.FIND_OBJECTS);
				break;
			case 11:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 5);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 12:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 6);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 13:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 7);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 14:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 8);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 15:
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 9);
				SceneManager.LoadSceneAsync((int)GameScenes.AUDIO);
				break;
			case 16:
				SceneManager.LoadSceneAsync((int)GameScenes.CARDS_GAME);
				break;
		}
	}

	void OnReturnClick()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
		CleanUICallbacks();
		SceneManager.LoadSceneAsync((int)GameScenes.MENU);
	}
}
