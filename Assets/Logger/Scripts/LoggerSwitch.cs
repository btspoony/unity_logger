#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

public class LoggerSwitch : MonoBehaviour
{
	[System.Serializable]
	public class ItemTag
	{
		public string tag;
		public Logger.LogLevelDefault logLevel = Logger.LogLevelDefault.INFO;
	}

	[Tooltip("Tags")]
	public ItemTag[] tags;
	
	void OnValidate()
	{
		foreach(var item in tags)
		{
			Logger.SetLogLevel( item.tag, item.logLevel );
		}
	}
}