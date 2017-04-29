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

	public Button startGameButton;
	public Button continueButton;
	public Button restartGameButton;

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

	public void OnContinueGameClick()
	{
		menuUi.gameObject.SetActive(false);
		gameUI.gameObject.SetActive(true);
		GameManager.Instance.ContinueGame();
	}
}
