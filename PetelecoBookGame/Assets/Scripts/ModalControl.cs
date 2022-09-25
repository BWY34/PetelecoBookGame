// System
using System;
// Unity
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
	public class ModalControl
	{
		#region Members

		// Blur background img
		private Image BlurImg;

		// Modal obj
		private GameObject ConfigModal;

		// UI position control
		private RectTransform ConfigModalRect;

		// Timers for lerps
		float LerpTimer1 = 0.0f;

		// Control interval
		float ControlInterval = 0.5f;

		// Config animations control
		bool OpenModal = false;
		bool IsModalOpen = false;
		bool InTransitionModal = false;

		// Actions
		public Action OnClose;
		public Action OnOpen;

		#endregion

		public ModalControl(Image blurImg, GameObject configModal, float time)
		{
			BlurImg = blurImg;
			ConfigModal = configModal;
			ControlInterval = time;

			ConfigModalRect = ConfigModal.GetComponent<RectTransform>();

			// Deactivate modal objects
			ConfigModal.SetActive(false);
			BlurImg.gameObject.SetActive(false);

			// Recolor
			BlurImg.color = new Color(0f, 0f, 0f, 0f);

			// Reposition
			ConfigModalRect.anchoredPosition = new Vector3(0f, -1300f, 0f);
		}

		public void Update()
		{
			if (OpenModal && (!IsModalOpen))
			{
				float t = Mathf.Min(1.0f, (1f / ControlInterval) * LerpTimer1);

				if (InTransitionModal)
				{
					// Interpolates bg alpha
					var color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.4f, t));
					BlurImg.color = color;

					// Translate Button to screen
					var pos = ConfigModalRect.anchoredPosition;
					pos.y = Mathf.Lerp(-1300f, 0f, t);
					ConfigModalRect.anchoredPosition = pos;

					if (LerpTimer1 > ControlInterval)
					{
						IsModalOpen = true;
						InTransitionModal = false;

						// Callback
						OnOpen?.Invoke();
					}

					LerpTimer1 += Time.deltaTime;
				}
				else
				{
					LerpTimer1 = 0.0f;
					InTransitionModal = true;
				}
			}

			if ((!OpenModal) && IsModalOpen)
			{
				float t = Mathf.Min(1.0f, (1f / ControlInterval) * LerpTimer1);

				if (InTransitionModal)
				{
					// Interpolates bg alpha
					var color = new Color(0f, 0f, 0f, Mathf.Lerp(0, 0.4f, 1 - t));
					BlurImg.color = color;

					var pos = ConfigModalRect.anchoredPosition;
					pos.y = Mathf.Lerp(0f, -1300f, t);
					ConfigModalRect.anchoredPosition = pos;

					if (LerpTimer1 > ControlInterval)
					{
						IsModalOpen = false;
						InTransitionModal = false;

						ConfigModal.SetActive(false);
						BlurImg.gameObject.SetActive(false);

						// Callback
						OnClose?.Invoke();
					}

					LerpTimer1 += Time.deltaTime;
				}
				else
				{
					LerpTimer1 = 0.0f;
					InTransitionModal = true;
				}
			}
		}

		public void Open()
		{
			if (!InTransitionModal)
			{
				ConfigModal.SetActive(true);
				BlurImg.gameObject.SetActive(true);
				OpenModal = true;
			}
		}

		public void Close()
		{
			if (!InTransitionModal)
			{
				OpenModal = false;
			}
		}
	}
}
