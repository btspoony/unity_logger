//#define PRODUCTION

using UnityEngine;
using System.Collections.Generic;

public sealed class Logger
{	
	static Logger()
	{
	}
	
	public enum LogCategory{
		EXCEPT = 1,
		ERROR = 2,
		WARN = 4,
		INFO = 8,
	}
	
	public enum LogLevelDefault {
		NONE = 0,
		ERROR = 3,
		WARN = 7,
		INFO = 15,
	}
	
	static Dictionary<string, int> logLevels = new Dictionary<string, int>();
	
	/// <summary>
	/// Sets the log level.
	/// </summary>
	/// <param name="tag">Tag.</param>
	/// <param name="lvl">Lvl.</param>
	public static void SetLogLevel( string tag, LogLevelDefault lvl){
		SetLogLevel( tag, (int)lvl );
	}
	/// <summary>
	/// Sets the log level.
	/// </summary>
	/// <param name="tag">Tag.</param>
	/// <param name="level">Level.</param>
	public static void SetLogLevel( string tag, int level ){
		if( logLevels.ContainsKey( tag ) ){
			logLevels[tag] = level;
		}else{
			logLevels.Add( tag, level );
		}
	}
	
	/// <summary>
	/// Checks the log level.
	/// </summary>
	/// <returns><c>true</c>, if log level was checked, <c>false</c> otherwise.</returns>
	/// <param name="tag">Tag.</param>
	/// <param name="cate">Cate.</param>
	private static bool CheckLogLevel( string tag, LogCategory cate ){
		if( tag == null ){
			return true;
		}
		int logLv;
		bool hasTag = logLevels.TryGetValue( tag, out logLv );
		if( !hasTag ){
			return true;
		}
		return (logLv & (int)cate) != 0;
	}
	
#if PRODUCTION
	class Msg
	{
		public float time;
		public string text;
		public Msg next;
	}

	static readonly int max = 2048;
	static readonly Msg root = new Msg();
	static int count = 0;
	static Msg curr = root;

	static void _Log(string msg, string tag = null)
	{
		curr = curr.next = new Msg{text = msg, time = Time.realtimeSinceStartup};
		if(count == max)
		{
			var head = root.next;
			root.next = head.next;
			head.next = null;
		}
		else ++count;
	}
	#endif 
	public static string Output()
	{
		#if PRODUCTION
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		var msg = root.next;
		while(msg != null)
		{
			builder.AppendLine("[Time]" + msg.time);
			builder.AppendLine(msg.text);
			msg = msg.next;
		}
		return builder.ToString();
		#else
		Debug.LogWarning("Logger.Output just valid at production version!");
		return "";
		#endif
	}
// =============== Logging ====================
	public static void Log(object msg, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.INFO ) )
			return;
#if PRODUCTION
		_Log(msg.ToString(), tag);
#else
		Debug.Log(msg);
#endif
	}
	
	public static void LogError(object msg, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.ERROR ) )
			return;
#if PRODUCTION
		_Log(msg.ToString(), tag);
#else
		Debug.LogError(msg);
#endif
	}
	
	public static void LogException(System.Exception ex, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.EXCEPT ) )
			return;
#if PRODUCTION
		_Log(msg.StackTrace, tag);
#else
		Debug.LogException(ex);
#endif
	}
	
	public static void LogWarning(object msg, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.WARN ) )
			return;
#if PRODUCTION
		_Log(msg.ToString(), tag);
#else
		Debug.LogWarning(msg);
#endif
	}
	
	public static void Log(object msg, Object context, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.INFO ) )
			return;
#if PRODUCTION
		_Log(msg.ToString(), tag);
#else
		Debug.Log(msg, context);
#endif
	}
	
	public static void LogError(object msg, Object context, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.ERROR ) )
			return;
#if PRODUCTION
		_Log(msg.ToString(), tag);
#else
		Debug.LogError(msg, context);
#endif
	}
	
	public static void LogException(System.Exception ex, Object context, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.EXCEPT ) )
			return;
#if PRODUCTION
		_Log(msg.StackTrace, tag);
#else
		Debug.LogException(ex, context);
#endif
	}
	
	public static void LogWarning(object msg, Object context, string tag = null)
	{
		if( !CheckLogLevel( tag, LogCategory.WARN ) )
			return;
#if PRODUCTION
		_Log(msg.ToString(), tag);
#else
		Debug.LogWarning(msg, context);
#endif
	}
// ================== Assert ====================
	public static void Assert( bool condition )
	{
		Assert( condition, string.Empty, true );
	}
	
	public static void Assert( bool condition, string assertString )
	{
		Assert( condition, assertString, true );
	}
	
	public static void Assert( bool condition, string assertString, bool pauseOnFail )
	{
		if( !condition )
		{
			Logger.LogError( "[ASSERTION] Failed! " + assertString );
			if( pauseOnFail )
				Debug.Break();
		}
	}
}
