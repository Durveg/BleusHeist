using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerStats : ScriptableObject {

	[SerializeField]
	private const int jewelsCarryingDefaultValue = 0;
	public int jewelsCarrying;

	public void ResetStats()
	{
		jewelsCarrying = jewelsCarryingDefaultValue;
	}
}
