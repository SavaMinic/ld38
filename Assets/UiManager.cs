using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

	#region Static

	public static UiManager Instance { get; private set; }

	#endregion

	#region Fields

	public RectTransform gameUI;
	public Text scoreLabel;

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
}
