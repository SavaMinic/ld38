using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControl : MonoBehaviour
{

	void Start()
	{
		UiManager.Instance.Debug();
		// we dont need this
		Destroy(gameObject);
	}
}
