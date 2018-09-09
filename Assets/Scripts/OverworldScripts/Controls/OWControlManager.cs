using UnityEngine;

/// <summary>
/// Singleton class containing all user controls.
/// </summary>
public class OWControlManager
{
	private static OWControlManager instance;

	private OWControlManager() {}

	public static OWControlManager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new OWControlManager();
			}
			return instance;
		}
	}

	/*
	 * All available inputs for the OverWorld.
	 */

	public KeyCode WalkUp;

}
