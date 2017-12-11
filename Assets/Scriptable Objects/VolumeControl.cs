using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class VolumeControl : ScriptableObject {

	[Range(0, 1)]
	public float MasterVolumeScale;
	[Range(0, 1)]
	public float MusicVolumeScale;
	[Range(0, 1)]
	public float SoundFXVolumeScale;

	public bool MasterMuted = false;
}
