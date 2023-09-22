using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class FloatAwayText : MonoBehaviour {
	public TMP_Text tmpText;
	public Vector3 floatAwayVector = Vector3.up;
	public float lifeTime = 3f;

	private float countup = 0f;

	private void Start() {
		if (!tmpText) tmpText = GetComponent<TMP_Text>();
	}

	private void Update() {
		transform.position += floatAwayVector * Time.deltaTime;

		countup += Time.deltaTime;
		if (countup >= lifeTime) {
			Destroy(gameObject);
		}
	}

	public void AssignText(string text) {
		tmpText.text = text;
	}
}
