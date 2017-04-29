using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Static

	public static GameManager Instance { get; private set; }

	#endregion

	public enum GameState
	{
		Menu,
		Playing,
		Won,
		Lost
	}

	public int MaxScore { get; private set; }
	public int CurrentScore { get; private set; }

	public GameState State { get; private set; }

	public bool IsPlaying { get { return State == GameState.Playing; } }
	public bool IsMenu { get { return State == GameState.Menu; } }

	void Awake()
	{
		Instance = this;
		MaxScore = FindObjectsOfType<Grenade>().Length;
		CurrentScore = 0;
		State = GameState.Playing;
	}

	public void GranadeFound()
	{
		CurrentScore++;
		UiManager.Instance.RefreshScore();
		if (CurrentScore == MaxScore)
		{
			WinGame();
		}
	}

	public void WinGame()
	{
		State = GameState.Won;
	}
}
