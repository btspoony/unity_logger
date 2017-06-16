using UnityEngine;
using System.Collections;

public class TestLogger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Logger.Log("This is TEST");
		Logger.Log("This is TEST Log Info", "Gameplay");
		Logger.LogWarning("This is TEST Log Warning", "Gameplay");
		Logger.LogError("This is TEST Log Error", "Gameplay");
		Logger.Assert( false, "hello world" );
	}
}
