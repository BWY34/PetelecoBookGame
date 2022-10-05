// System
using System;
// Unity
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Assets
using Assets.Enums;
using Assets.Constants;


public class AudioViewer : MonoBehaviour
{
	#region EditorRefs

	// Background

	[SerializeField]
	private Image SunHaloImg;

	[SerializeField]
	private AudioSource Source;

	[SerializeField]
	private TMP_Text SubtitleField;

	[SerializeField]
	private TMP_Text TitleField;

	[SerializeField]
	private Sprite PauseSprite;

	[SerializeField]
	private Sprite ResumeSprite;

	[SerializeField]
	private Button PauseBtn;

	[SerializeField]
	private Button RestartBtn;

	[SerializeField]
	private Button ReturnBtn;

	[SerializeField]
	private Image StrikeAnim;

	#region AudioClips

	[SerializeField]
	private AudioClip FocusClip;

	[SerializeField]
	private AudioClip CortesyClip;

	[SerializeField]
	private AudioClip DisciplineClip;

	[SerializeField]
	private AudioClip EmpatyClip;

	[SerializeField]
	private AudioClip HistoryClip;

	[SerializeField]
	private AudioClip HonestyClip;

	[SerializeField]
	private AudioClip PatienceClip;

	[SerializeField]
	private AudioClip PersistenceClip;

	[SerializeField]
	private AudioClip RespectClip;

	[SerializeField]
	private AudioClip StrikeClip;

	#endregion

	#region Subtitles

	private string[] Focus = new string[]
	{
		"Ele percebeu ",
		"que o foco � a estrat�gia ",
		"que garante que voc� ",
		"se mantenha empenhado ",
		"em alcan�ar um objetivo, ",
		"e percorra ",
		"o caminho necess�rio ",
		"para chegar at� ",
		"l�."
	};

	private string[] Cortesy = new string[]
	{
		"Em seguida ",
		"eles praticaram a ",
		"virtude da Cortesia. ",
		"Nessa virtude ",
		"Peteleco compreendeu ",
		"que um grande ",
		"Ninja deve ser Cort�s ",
		"e ter ",
		"como princ�pio ajudar ",
		"as pessoas. ",
		"Foi assim que come�ou ",
		"a perceber a grandeza ",
		"dos pequenos ",
		"gestos que podem ser ",
		"praticados no decorrer ",
		"do dia. ",
		"Percebeu ",
		"que atitudes simples, ",
		"como limpar a casa ",
		"e ser prestativo, ",
		"fazem muita diferen�a ",
		"para a disciplina ",
		"e desenvolvimento ",
		"de um Ninja. ",
		"Afinal de contas, ",
		"se quiser salvar ",
		"o Mundo, um grande ",
		"Ninja deve antes manter ",
		"sua pr�pria vida e ",
		"ambiente organizados, ",
		"algo ",
		"que seu Mestre Volantin ",
		"sempre enfatizou. "
	};

	private string[] Discipline = new string[]
	{
		"Foi nesse cen�rio ",
		"que ele desenvolveu ",
		"a virtude da disciplina. ",
		"Peteleco ",
		"aprendeu que a verdadeira ",
		"disciplina consiste ",
		"em realizar ",
		"o que deve ser feito, ",
		"independente ",
		"de sua vontade pr�pria ",
		"no momento. ",
		"Peteleco entendeu ",
		"que a disciplina envolve ",
		"possuir consist�ncia ",
		"e integridade no que ",
		"se faz, e sem ",
		"ela, todo seu ",
		"treinamento e aprendizado ",
		"seriam in�teis ",
		"e ele jamais ",
		"poderia se ",
		"tornar um Mestre ",
		"Ninja. ",
	};

	private string[] Empaty = new string[]
	{
		"Da empatia e ",
		"comunica��o, ",
		"Peteleco aprendeu a import�ncia ",
		"e a forma adequada ",
		"de se comunicar ",
		"com as pessoas, ",
		"e tamb�m de se colocar ",
		"no lugar do outro. ",
		"Compreendeu que, ",
		"para se tornar um grande ",
		"Ninja, precisava saber, ",
		"ao mesmo tempo, ",
		"expressar-se ",
		"e compreender as ",
		"pessoas, e a melhor ",
		"maneira de conseguir ",
		"isto era falando ",
		"suas vontades, ",
		"questionando ",
		"suas d�vidas, argumentando ",
		"suas ideias, ",
		"expondo seus ",
		"sentimentos atrav�s de ",
		"fala, gestos, ",
		"express�es ",
		"e ouvindo as pessoas."
	};

	private string[] History = new string[]
	{
		"O Taekwondo � ",
		"uma arte marcial que ",
		"surgiu na Cor�ia, ",
		"na �poca em que a regi�o ",
		"era dividida em Tr�s ",
		"Reinos. Um antigo ",
		"rei da Dinastia ",
		"Silla ",
		"resolveu treinar ",
		"um grupo de guerreiros que ",
		"dominasse o combate corpo a corpo, ",
		"e, assim, ",
		"foi criada a elite de ",
		"ex�mios lutadores ",
		"Hwa Rang Do, ",
		"semelhantes aos samurais ",
		"japoneses. ",
		"Durante muito tempo, ",
		"essa arte foi considerada ",
		"proibida de ser ",
		"praticada, ",
		"mas na d�cada de ",
		"1950 ",
		"o general Choi Hong-hi ",
		"resolveu ",
		"unir conhecimentos ",
		"de v�rias artes marciais, ",
		"criando ",
		"o Taekwondo, que significa ",
		"\"caminho ",
		"dos p�s e das m�os ",
		"atrav�s da mente\". ",
		"Hoje considerado ",
		"um esporte, ",
		"o Taekwondo ensina ",
		"sobre disciplina, ",
		"eleva��o da espiritualidade ",
		"atrav�s ",
		"de conhecimentos filos�ficos, ",
		"e desenvolvimento ",
		"do corpo ",
		"e da mente. ",
		"Nas gradua��es de faixas ",
		"(que mostram a ",
		"eleva��o ",
		"f�sica ",
		"e espiritual do indiv�duo), ",
		"temos ",
		"11 faixas, sendo ",
		"a branca a primeira, ",
		"e a preta, o �ltimo n�vel."
	};

	private string[] Honesty = new string[]
	{
		"Peteleco aprendeu ",
		"que honestidade � a ",
		"virtude de abrir o seu ",
		"cora��o a outras pessoas, ",
		"comunicando sempre a verdade, ",
		"por mais que essa atitude ",
		"possa ser dolorosa ",
		"ou desagrad�vel ",
		"de ser feita. ",
		"A honestidade � ",
		"uma das maiores ",
		"provas de amizade e ",
		"grandeza espiritual."
	};

	private string[] Patience = new string[]
	{
		"A paci�ncia � ",
		"a virtude da espera cautelosa, ",
		"quando uma pessoa ",
		"tem maturidade ",
		"suficiente ",
		"para compreender que ",
		"tudo acontece no seu ",
		"devido tempo, ",
		"sem a necessidade de ",
		"correr para conseguir ",
		"algo. Foi dessa maneira ",
		"que ele compreendeu ",
		"que �s vezes � ",
		"preciso esperar para ",
		"se ter aquilo que se deseja."
	};

	private string[] Persistence = new string[]
	{
		"Ent�o veio a persist�ncia, ",
		"e ",
		"com ela, Peteleco ",
		"aprendeu a nunca desistir ",
		"daquilo que desejava ",
		"alcan�ar, e ",
		"tamb�m, a acreditar ",
		"na sua capacidade ",
		"de fazer as coisas. ",
		"A persist�ncia ",
		"� a virtude ",
		"da perseveran�a, ",
		"quando uma pessoa ",
		"se mant�m focada ",
		"em seu objetivo ",
		"e n�o desiste. Continua ",
		"lutando, encontrando ",
		"os melhores caminhos ",
		"para passar os obst�culos."
	};

	private string[] Respect = new string[]
	{
		"Peteleco ",
		"compreendeu que cada ",
		"indiv�duo � dotado de ",
		"caracter�sticas e ",
		"aprendizados �nicos, ",
		"e que respeitar ",
		"algu�m � ter reconhecimento ",
		"por quem essa pessoa ",
		"realmente �, ",
		"e suas experi�ncias ",
		"de vida. ",
		"Quando voc� respeita ",
		"outras pessoas, ",
		"est� se abrindo a ",
		"ter uma rela��o de amizade ",
		"e crescimento ",
		"com elas, podendo ",
		"aprender tudo o que ",
		"t�m a ensinar. Afinal ",
		"de contas, ",
		"at� mesmo os maiores ",
		"Mestres Ninjas n�o ",
		"cresceram sozinhos e por ",
		"conta pr�pria, ",
		"precisaram de pessoas ",
		"para auxiliar em sua caminhada."
	};

	private string[] Strike = new string[]
	{
		"Peteleco executou ",
		"seus golpes ",
		"de Taekwondo brilhantemente. ",
		"Iniciando ",
		"com muita concentra��o ",
		"e foco, foi ",
		"capaz de demonstrar ",
		"ao Mestre Volantin ",
		"com maestria ",
		"como estava preparado ",
		"para enfrentar ",
		"os desafios ",
		"a ele passados."
	};

	#endregion

	#endregion

	private string[] Subtitle;

	private bool IsPaused = false;
	private bool IsRestart = false;
	private int LastIndex = -1;

	// Start is called before the first frame update
	void Start()
	{
		AudioManager.Instance.StopAudio(AudioSources.MAIN_LOOP);

		int source = PlayerPrefs.GetInt(PlayerPrefsKeys.AudioSceneSource);

		switch (source)
		{
			case 0:
				Source.clip = FocusClip;
				Subtitle = Focus;
				TitleField.text = "Virtude do Foco";
				UpdateLevel(3);
				break;
			case 1:
				Source.clip = CortesyClip;
				Subtitle = Cortesy;
				TitleField.text = "Virtude do Cortesia";
				UpdateLevel(4);
				break;
			case 2:
				Source.clip = RespectClip;
				Subtitle = Respect;
				TitleField.text = "Virtude do Respeito";
				UpdateLevel(8);
				break;
			case 3:
				Source.clip = PersistenceClip;
				Subtitle = Persistence;
				TitleField.text = "Virtude do Persist�ncia";
				UpdateLevel(9);
				break;
			case 4:
				Source.clip = PatienceClip;
				Subtitle = Patience;
				TitleField.text = "Virtude do Paci�ncia";
				UpdateLevel(10);
				break;
			case 5:
				Source.clip = EmpatyClip;
				Subtitle = Empaty;
				TitleField.text = "Virtude do Empatia";
				UpdateLevel(12);
				break;
			case 6:
				Source.clip = HonestyClip;
				Subtitle = Honesty;
				TitleField.text = "Virtude do Honestidade";
				UpdateLevel(13);
				break;
			case 7:
				Source.clip = DisciplineClip;
				Subtitle = Discipline;
				TitleField.text = "Virtude do Disciplina";
				UpdateLevel(14);
				break;
			case 8:
				StrikeAnim.gameObject.SetActive(true);
				Source.clip = StrikeClip;
				Subtitle = Strike;
				TitleField.text = "Golpe";
				UpdateLevel(15);
				break;
			case 9:
				Source.clip = HistoryClip;
				Subtitle = History;
				TitleField.text = "Hist�ria do Taekwondo";
				UpdateLevel(16);
				break;
		}

		SetupUICallbacks();
		Source.Play();
	}

    // Update is called once per frame
    void Update()
    {
		UpdateAudio();

		// Updates halo background
		UpdateHaloRotation();
	}

	private void UpdateLevel(int level)
	{
		var cur = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);

		if (cur < level)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, level);
		}
	}

	// Rotates the halo around the sun
	private void UpdateHaloRotation()
	{
		const float DegreesPerSecond = 2;

		float dAngle = DegreesPerSecond * Time.deltaTime;

		SunHaloImg.transform.Rotate(new Vector3(0, 0, dAngle));
	}

	void UpdateAudio()
	{
		if (IsRestart)
		{
			SubtitleField.text = "";
			IsRestart = false;
		}

		if (!IsPaused)
		{
			int index = (int)Math.Floor(Source.time);

			if (
				(index >= 0 && index < Subtitle.Length) &&
				(LastIndex != index)
			)
			{
				if (index == 0 && LastIndex != -1)
				{
					return;
				}
				SubtitleField.text += Subtitle[index];
				LastIndex = index;
			}
		}
	}

	void SetupUICallbacks()
	{
		PauseBtn.onClick.AddListener(OnPauseClick);
		RestartBtn.onClick.AddListener(OnRestartClick);
		ReturnBtn.onClick.AddListener(OnReturnClick);
	}

	void CleanUICallbacks()
	{
		PauseBtn.onClick.RemoveListener(OnPauseClick);
		RestartBtn.onClick.RemoveListener(OnRestartClick);
		ReturnBtn.onClick.RemoveListener(OnReturnClick);
	}

	void OnPauseClick()
	{
		if (IsPaused)
		{
			Source.UnPause();
			PauseBtn.GetComponent<Image>().sprite = PauseSprite;
			IsPaused = false;
		}
		else
		{
			Source.Pause();
			PauseBtn.GetComponent<Image>().sprite = ResumeSprite;
			IsPaused = true;
		}
	}

	void OnRestartClick()
	{
		Source.time = 0f;
		Source.Play();

		PauseBtn.GetComponent<Image>().sprite = PauseSprite;
		IsPaused = false;

		LastIndex = -1;
		IsRestart = true;
	}

	void OnReturnClick()
	{
		AudioManager.Instance.PlayAudio(AudioSources.BTN_EFFECT, false);
		CleanUICallbacks();
		SceneManager.LoadSceneAsync((int)GameScenes.MENU);
	}
}
