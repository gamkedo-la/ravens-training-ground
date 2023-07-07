using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewMusicAsset", menuName = "Advanced Music Asset")]
public class MusicAssetAdvanced : MusicAsset {
	public Mux mux;

	public override void Play() {
		MusicManager.instance.PlayMux(mux);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(MusicAssetAdvanced))]
[CanEditMultipleObjects]
public class MusicAssetAdvancedEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (!EditorApplication.isPlaying) return;

		if (GUILayout.Button("Play Mux")) {
			(target as MusicAssetAdvanced).Play();
		}
	}
}
#endif