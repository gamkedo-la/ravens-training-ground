using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewMusicAssetSimple", menuName = "Simple Music Asset")]
public class MusicAssetBasic : MusicAsset {
	[SerializeField] private AudioClip musicClip;
	private Mux mux = new Mux();

	public override void Play() {
		if (musicClip == null) return;

		if (mux.tracks.Count == 0) AssignClip();

		MusicManager.Instance.PlayMux(mux);
	}

	private void AssignClip() {
		MuxTrack track = new MuxTrack();
		track.audioclips.Add(musicClip);
		track.endTime = (double)musicClip.samples / (double)musicClip.frequency;

		mux.tracks.Add(track);
	}

	void OnValidate() {
		mux.tracks.Clear();
		if (musicClip != null) AssignClip();
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(MusicAssetBasic))]
[CanEditMultipleObjects]
public class MusicAssetBasicEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (!EditorApplication.isPlaying) return;

		if (GUILayout.Button("Play Mux")) {
			(target as MusicAssetBasic).Play();
		}
	}
}
#endif