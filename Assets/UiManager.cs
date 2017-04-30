using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{

	#region Static

	public static UiManager Instance { get; private set; }

	#endregion

	#region Fields

	public RectTransform gameUI;
	public Text scoreLabel;
	public RectTransform winUI;
	public RectTransform menuUi;
	public RectTransform helpUi;
	public RectTransform aboutUI;
	public RectTransform storyUI;

	public Button startGameButton;
	public Button continueButton;
	public Button restartGameButton;

	public Button helpButton;
	public Button aboutButton;
	public Button closeHelpAboutButton;

	public List<RectTransform> storyUIs;
	private int currentStoryIndex;

	public Text hintText;

	[System.Serializable]
	public class Hint
	{
		public string hintText;
		public float delay;
		public List<KeyCode> keyCodes;
	}

	public List<Hint> hints;
	private Hint activeHint;

	#endregion

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		if (GameManager.Instance.IsPlaying && activeHint != null)
		{
			var finished = false;
			for (int i = 0; i < activeHint.keyCodes.Count; i++)
			{
				if (Input.GetKeyDown(activeHint.keyCodes [i]))
				{
					finished = true;
					break;
				}
			}
			if (finished)
			{
				ShowNextHint();
			}
		}
	}

	public void RefreshScore()
	{
		var gm = GameManager.Instance;
		StartCoroutine(RefreshAnimation(gm.CurrentScore, gm.MaxScore));
	}

	public void Refresh(bool isStart)
	{
		menuUi.gameObject.SetActive(true);
		startGameButton.gameObject.SetActive(isStart);
		continueButton.gameObject.SetActive(!isStart);
		restartGameButton.gameObject.SetActive(!isStart);
		RefreshScore();
	}

	private IEnumerator RefreshAnimation(int currScore, int maxScore)
	{
		Go.to(scoreLabel, .4f, new GoTweenConfig()
			.intProp("fontSize", 72)
			.setIterations(2, GoLoopType.PingPong)
			.setEaseType(GoEaseType.BackInOut)
		);

		yield return new WaitForSeconds(0.3f);
		scoreLabel.text =currScore + "/" + maxScore;
	}

	public void OnCloseGame()
	{
		Application.Quit();
	}

	public void ShowWining()
	{
		winUI.gameObject.SetActive(true);
	}

	public void OnContinueGameClick()
	{
		menuUi.gameObject.SetActive(false);
		gameUI.gameObject.SetActive(true);
		hintText.gameObject.SetActive(activeHint != null);
		GameManager.Instance.ContinueGame();
	}

	public void OnHelpClick()
	{
		helpUi.gameObject.SetActive(true);
		aboutUI.gameObject.SetActive(false);

		helpButton.gameObject.SetActive(false);
		aboutButton.gameObject.SetActive(false);
		closeHelpAboutButton.gameObject.SetActive(true);
	}

	public void OnAboutClick()
	{
		helpUi.gameObject.SetActive(false);
		aboutUI.gameObject.SetActive(true);

		helpButton.gameObject.SetActive(false);
		aboutButton.gameObject.SetActive(false);
		closeHelpAboutButton.gameObject.SetActive(true);
	}

	public void OnCloseAboutHelpClick()
	{
		helpUi.gameObject.SetActive(false);
		aboutUI.gameObject.SetActive(false);

		helpButton.gameObject.SetActive(true);
		aboutButton.gameObject.SetActive(true);
		closeHelpAboutButton.gameObject.SetActive(false);
	}

	public void ShowNextHint()
	{
		if (activeHint != null)
		{
			activeHint = null;
			hintText.gameObject.SetActive(false);
		}
		if (hints.Count > 0)
		{
			var hint = hints[0];
			hints.RemoveAt(0);
			StartCoroutine(ShowHint(hint));
		}

	}

	private IEnumerator ShowHint(Hint hint)
	{
		if (hint.delay > 0)
		{
			yield return new WaitForSeconds(hint.delay);
		}
		activeHint = hint;
		hintText.text = hint.hintText;
		if (GameManager.Instance.IsPlaying)
		{
			hintText.gameObject.SetActive(true);
		}
	}

	public void OnStartQuestClick()
	{
		storyUI.gameObject.SetActive(true);
	}

	public void OnSkipClick()
	{
		currentStoryIndex++;
		if (currentStoryIndex == storyUIs.Count)
		{
			storyUI.gameObject.SetActive(false);
			OnContinueGameClick();
		}
		else
		{
			storyUIs[currentStoryIndex].gameObject.SetActive(true);
		}
	}


}
