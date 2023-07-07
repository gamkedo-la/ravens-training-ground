using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public static MusicManager instance;
	[SerializeField] private AudioSource musicSourcePrefab;
	[SerializeField] private MusicAsset startingMusicAsset;

	public bool playOnStart = true;

	const double BufferTime = 0.25;
	const float FadeTime = 0.15f;

	private AudioSource currentSource;
	private AudioClip currentClip;
	private Mux currentMux;
	private MuxTrack currentTrack, nextTrack;
	private double nextStartTime = double.MaxValue;

	void Awake() {
		if (instance != null) {
			if (playOnStart && startingMusicAsset) startingMusicAsset.Play();
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start() {
		if (playOnStart && startingMusicAsset) startingMusicAsset.Play();
	}

	void Update() {
		if (AudioSettings.dspTime > nextStartTime - BufferTime) {
			PlayTrack(nextTrack);
		}
	}

	public void PlayMux(Mux newMux) {
		if (newMux == null || newMux == currentMux) return;

		StopMux((float)BufferTime);
		currentMux = newMux;

		nextStartTime = AudioSettings.dspTime + BufferTime;
		PlayTrack(currentMux.tracks[0]);
	}

	public void StopMux(float delay = 0f) {
		StartCoroutine(WaitAndFadeOutAndStop(currentSource, delay));
		nextStartTime = double.MaxValue;
	}

	private void PlayTrack(MuxTrack newTrack) {
		currentTrack = newTrack;
		currentClip = currentTrack.GetClip();
		nextTrack = currentMux.GetNextTrack(currentTrack);

		double startTime = nextStartTime;
		if (currentTrack.endTime <= 0.0 && currentClip != null) {
			currentTrack.endTime = (double)currentClip.samples / (double)currentClip.frequency;
		}
		nextStartTime = currentTrack.endTime + startTime - nextTrack.startTime;

		if (currentClip == null) return;
		currentSource = Instantiate(musicSourcePrefab, this.transform);
		currentSource.gameObject.name = currentClip.name;
		currentSource.clip = currentClip;
		currentSource.PlayScheduled(startTime);
		Destroy(currentSource.gameObject, currentSource.clip.length + (float)BufferTime);
	}

	IEnumerator WaitAndFadeOutAndStop(AudioSource source, float waitTime, float fadeTime = FadeTime) {
		if (source == null) yield break;

		float startTime = Time.unscaledTime + waitTime;
		float currentTime;
		float startVolume = source.volume;

		yield return new WaitForSecondsRealtime(waitTime);

		while (startTime + fadeTime > Time.unscaledTime) {
			if (source == null) yield break;

			currentTime = Time.unscaledTime - startTime;

			source.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
			yield return null;
		}

		if (source == null) yield break;
		source.Stop();
		Destroy(source.gameObject);
	}
}

[System.Serializable]
public class Mux {
	public List<MuxTrack> tracks;

	public MuxTrack GetNextTrack(MuxTrack currentTrack) {
		string label = currentTrack.GetTransitionTarget().ToLower();
		MuxTrack nextTrack = currentTrack;

		for (int i = 0; i < tracks.Count; i++) {
			if (tracks[i].label.ToLower() == label) {
				nextTrack = tracks[i];
				break;
			}
		}

		return nextTrack;
	}
}

[System.Serializable]
public class MuxTrack {
	public string label = "";
	public List<AudioClip> audioclips = new List<AudioClip>();
	public double startTime, endTime;
	public LabelWeightPair[] trasitionTargets = new LabelWeightPair[0];

	public string GetTransitionTarget() {
		string transition = "";

		float scale = 0f;
		for (int i = 0; i < trasitionTargets.Length; i++) {
			scale += trasitionTargets[i].weight;
		}

		float randomNumber = Random.value * scale;
		float runningTotal = scale;
		for (int i = trasitionTargets.Length-1; i >= 0; i--) {
			runningTotal -= trasitionTargets[i].weight;
			if (randomNumber >= runningTotal) {
				transition = trasitionTargets[i].label;
				break;
			}
		}

		return transition;
	}

	public AudioClip GetClip() {
		if (audioclips.Count == 0) return null;

		int randomIndex = Random.Range(0, audioclips.Count);
		return audioclips[randomIndex];
	}
}

[System.Serializable]
public struct LabelWeightPair {
	public string label;
	public float weight;
}