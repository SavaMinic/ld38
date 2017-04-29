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

	public Button startGameButton;
	public Button continueButton;
	public Button restartGameButton;

	public Button helpButton;
	public Button aboutButton;
	public Button closeHelpAboutButton;

	#endregion

	void Awake()
	{
		Instance = this;
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
}
