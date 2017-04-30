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

	public Light mainLight;
	public Color startColor;
	public Color endColor;

	public Renderer floorRenderer;
	public Color startFloorColor;
	public Color endFloorColor;

	public Renderer wallMaterial;
	public Color startWallColor;
	public Color endWallColor;

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
		State = GameState.Menu;
		UiManager.Instance.Refresh(true);
		UiManager.Instance.ShowNextHint();
	}

	void Update()
	{
		if (IsPlaying && Input.GetKeyUp(KeyCode.Escape))
		{
			State = GameState.Menu;
			UiManager.Instance.Refresh(false);
			Time.timeScale = 0f;
		}
	}

	public void IncreaseAtmopshere()
	{
		// change atmophere of the game
		var progress = Mathf.Clamp01((CurrentScore + 1) / (float)MaxScore);
		mainLight.color = Color.Lerp(startColor, endColor, progress);
		floorRenderer.material.color = Color.Lerp(startFloorColor, endFloorColor, progress);
		wallMaterial.sharedMaterial.color = Color.Lerp(startWallColor, endWallColor, progress);
	}

	public void GrenadeFound()
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
		UiManager.Instance.ShowWining();
	}

	public void LoseGame()
	{
		State = GameState.Lost;
		UiManager.Instance.ShowLosing();
	}

	public void ContinueGame()
	{
		State = GameState.Playing;
		Time.timeScale = 1f;
	}
}
