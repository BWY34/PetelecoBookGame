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
		"que o foco é a estratégia ",
		"que garante que você ",
		"se mantenha empenhado ",
		"em alcançar um objetivo, ",
		"e percorra ",
		"o caminho necessário ",
		"para chegar até ",
		"lá."
	};

	private string[] Cortesy = new string[]
	{
		"Em seguida ",
		"eles praticaram a ",
		"virtude da Cortesia. ",
		"Nessa virtude ",
		"Peteleco compreendeu ",
		"que um grande ",
		"Ninja deve ser Cortês ",
		"e ter ",
		"como princípio ajudar ",
		"as pessoas. ",
		"Foi assim que começou ",
		"a perceber a grandeza ",
		"dos pequenos ",
		"gestos que podem ser ",
		"praticados no decorrer ",
		"do dia. ",
		"Percebeu ",
		"que atitudes simples, ",
		"como limpar a casa ",
		"e ser prestativo, ",
		"fazem muita diferença ",
		"para a disciplina ",
		"e desenvolvimento ",
		"de um Ninja. ",
		"Afinal de contas, ",
		"se quiser salvar ",
		"o Mundo, um grande ",
		"Ninja deve antes manter ",
		"sua própria vida e ",
		"ambiente organizados, ",
		"algo ",
		"que seu Mestre Volantin ",
		"sempre enfatizou. "
	};

	private string[] Discipline = new string[]
	{
		"Foi nesse cenário ",
		"que ele desenvolveu ",
		"a virtude da disciplina. ",
		"Peteleco ",
		"aprendeu que a verdadeira ",
		"disciplina consiste ",
		"em realizar ",
		"o que deve ser feito, ",
		"independente ",
		"de sua vontade própria ",
		"no momento. ",
		"Peteleco entendeu ",
		"que a disciplina envolve ",
		"possuir consistência ",
		"e integridade no que ",
		"se faz, e sem ",
		"ela, todo seu ",
		"treinamento e aprendizado ",
		"seriam inúteis ",
		"e ele jamais ",
		"poderia se ",
		"tornar um Mestre ",
		"Ninja. ",
	};

	private string[] Empaty = new string[]
	{
		"Da empatia e ",
		"comunicação, ",
		"Peteleco aprendeu a importância ",
		"e a forma adequada ",
		"de se comunicar ",
		"com as pessoas, ",
		"e também de se colocar ",
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
		"suas dúvidas, argumentando ",
		"suas ideias, ",
		"expondo seus ",
		"sentimentos através de ",
		"fala, gestos, ",
		"expressões ",
		"e ouvindo as pessoas."
	};

	private string[] History = new string[]
	{
		"O Taekwondo é ",
		"uma arte marcial que ",
		"surgiu na Coréia, ",
		"na época em que a região ",
		"era dividida em Três ",
		"Reinos. Um antigo ",
		"rei da Dinastia ",
		"Silla ",
		"resolveu treinar ",
		"um grupo de guerreiros que ",
		"dominasse o combate corpo a corpo, ",
		"e, assim, ",
		"foi criada a elite de ",
		"exímios lutadores ",
		"Hwa Rang Do, ",
		"semelhantes aos samurais ",
		"japoneses. ",
		"Durante muito tempo, ",
		"essa arte foi considerada ",
		"proibida de ser ",
		"praticada, ",
		"mas na década de ",
		"1950 ",
		"o general Choi Hong-hi ",
		"resolveu ",
		"unir conhecimentos ",
		"de várias artes marciais, ",
		"criando ",
		"o Taekwondo, que significa ",
		"\"caminho ",
		"dos pés e das mãos ",
		"através da mente\". ",
		"Hoje considerado ",
		"um esporte, ",
		"o Taekwondo ensina ",
		"sobre disciplina, ",
		"elevação da espiritualidade ",
		"através ",
		"de conhecimentos filosóficos, ",
		"e desenvolvimento ",
		"do corpo ",
		"e da mente. ",
		"Nas graduações de faixas ",
		"(que mostram a ",
		"elevação ",
		"física ",
		"e espiritual do indivíduo), ",
		"temos ",
		"11 faixas, sendo ",
		"a branca a primeira, ",
		"e a preta, o último nível."
	};

	private string[] Honesty = new string[]
	{
		"Peteleco aprendeu ",
		"que honestidade é a ",
		"virtude de abrir o seu ",
		"coração a outras pessoas, ",
		"comunicando sempre a verdade, ",
		"por mais que essa atitude ",
		"possa ser dolorosa ",
		"ou desagradável ",
		"de ser feita. ",
		"A honestidade é ",
		"uma das maiores ",
		"provas de amizade e ",
		"grandeza espiritual."
	};

	private string[] Patience = new string[]
	{
		"A paciência é ",
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
		"que às vezes é ",
		"preciso esperar para ",
		"se ter aquilo que se deseja."
	};

	private string[] Persistence = new string[]
	{
		"Então veio a persistência, ",
		"e ",
		"com ela, Peteleco ",
		"aprendeu a nunca desistir ",
		"daquilo que desejava ",
		"alcançar, e ",
		"também, a acreditar ",
		"na sua capacidade ",
		"de fazer as coisas. ",
		"A persistência ",
		"é a virtude ",
		"da perseverança, ",
		"quando uma pessoa ",
		"se mantém focada ",
		"em seu objetivo ",
		"e não desiste. Continua ",
		"lutando, encontrando ",
		"os melhores caminhos ",
		"para passar os obstáculos."
	};

	private string[] Respect = new string[]
	{
		"Peteleco ",
		"compreendeu que cada ",
		"indivíduo é dotado de ",
		"características e ",
		"aprendizados únicos, ",
		"e que respeitar ",
		"alguém é ter reconhecimento ",
		"por quem essa pessoa ",
		"realmente é, ",
		"e suas experiências ",
		"de vida. ",
		"Quando você respeita ",
		"outras pessoas, ",
		"está se abrindo a ",
		"ter uma relação de amizade ",
		"e crescimento ",
		"com elas, podendo ",
		"aprender tudo o que ",
		"têm a ensinar. Afinal ",
		"de contas, ",
		"até mesmo os maiores ",
		"Mestres Ninjas não ",
		"cresceram sozinhos e por ",
		"conta própria, ",
		"precisaram de pessoas ",
		"para auxiliar em sua caminhada."
	};

	private string[] Strike = new string[]
	{
		"Peteleco executou ",
		"seus golpes ",
		"de Taekwondo brilhantemente. ",
		"Iniciando ",
		"com muita concentração ",
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
				TitleField.text = "Virtude do Persistência";
				UpdateLevel(9);
				break;
			case 4:
				Source.clip = PatienceClip;
				Subtitle = Patience;
				TitleField.text = "Virtude do Paciência";
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
				TitleField.text = "História do Taekwondo";
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
