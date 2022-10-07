// System
using System.Linq;
// Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// ZXing
using ZXing;
// Enums
using Assets.Enums;
using Assets.Constants;
using System;

public class QrScanner : MonoBehaviour
{
	[SerializeField]
	private RawImage BgRawImage;

	[SerializeField]
	private AspectRatioFitter Fitter;

	[SerializeField]
	private RectTransform ScanZone;

	[SerializeField]
	private Button ReturnBtn;

	[SerializeField]
	private Text InfoTxt;

	private bool IsInTransition = false;

	private bool HasCamera = false;

	private WebCamTexture CameraTexture;

	private IBarcodeReader BarcodeReader;

	// Start is called before the first frame update
	void Start()
	{
		SetupUiCallbacks();

		SetupCamera();

		BarcodeReader = new BarcodeReader();
	}

	// Update is called once per frame
	void Update()
	{
		if (!HasCamera || IsInTransition)
		{
			// Show error and auth popup
			return;
		}

		UpdateCameraRender();

		string code = DecodeCameraTexture();

		var CurrentLevel = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		switch (code)
		{
			case "0":
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				SceneManager.LoadScene((int)GameScenes.CONNECT_POINTS);
				break;
			case "1":
				if (CurrentLevel < 2)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 0);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "2":
				if (CurrentLevel < 3)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 1);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "5":
				if (CurrentLevel < 4)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				SceneManager.LoadScene((int)GameScenes.FIND_SHADOWS);
				break;
			case "6":
				if (CurrentLevel < 5)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				SceneManager.LoadScene((int)GameScenes.SEPARATE_TRASH);
				break;
			case "4":
				if (CurrentLevel < 6)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				SceneManager.LoadScene((int)GameScenes.MEMORY_GAME);
				break;
			case "8":
				if (CurrentLevel < 7)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 2);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "9":
				if (CurrentLevel < 8)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 3);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "10":
				if (CurrentLevel < 9)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 4);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "11":
				if (CurrentLevel < 10)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				SceneManager.LoadScene((int)GameScenes.FIND_OBJECTS);
				break;
			case "12":
				if (CurrentLevel < 11)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 5);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "13":
				if (CurrentLevel < 12)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 6);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "14":
				if (CurrentLevel < 13)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 7);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "15":
				if (CurrentLevel < 14)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 8);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "16":
				if (CurrentLevel < 15)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				PlayerPrefs.SetInt(PlayerPrefsKeys.AudioSceneSource, 9);
				SceneManager.LoadScene((int)GameScenes.AUDIO);
				break;
			case "17":
				if (CurrentLevel < 16)
				{
					InfoTxt.text = "Jogue os levels anteriores para ver esse.";
					return;
				}
				IsInTransition = true;
				InfoTxt.text = "Carregando o level...";
				SceneManager.LoadScene((int)GameScenes.CARDS_GAME);
				break;
			default:
				break;
		}
	}

	private void SetupUiCallbacks()
	{
		ReturnBtn.onClick.AddListener(OnReturnToMenu);
	}

	private void CleanUiCallbacks()
	{
		ReturnBtn.onClick.RemoveListener(OnReturnToMenu);
	}

	private void OnReturnToMenu()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
		CleanUiCallbacks();
		SceneManager.LoadSceneAsync((int)GameScenes.MENU);
	}

	private void UpdateCameraRender()
	{
		float ratio = (float)CameraTexture.width / (float)CameraTexture.height;

		Fitter.aspectRatio = ratio;

		int orientation = CameraTexture.videoRotationAngle;
		BgRawImage.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
	}

	private string DecodeCameraTexture()
	{
		try
		{
			var result = BarcodeReader.Decode(
				CameraTexture.GetPixels32(),
				CameraTexture.width, CameraTexture.height
			);

			return result == null ? "" : result.Text;
		}
		catch
		{
		}

		return "";
	}

	private void SetupCamera()
	{
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0 || !devices.Any(d => !d.isFrontFacing))
			return;

		var device = devices.First(device => !device.isFrontFacing);

		CameraTexture = new WebCamTexture(
			device.name, (int)ScanZone.rect.width, (int)ScanZone.rect.height
		);

		CameraTexture.Play();
		BgRawImage.texture = CameraTexture;
		HasCamera = true;
	}
}
