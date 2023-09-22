using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dungeonKey : MonoBehaviour {
    public FloatAwayText floatAwayTextPrefab;

    public string key;

    private GameObject player = null;

    void Update() {
        if (player) {
            player.AddComponent<dungeonKeyCarry>().key = key;

            FloatAwayText newText = Instantiate(floatAwayTextPrefab);
            newText.transform.position = transform.position;
            newText.transform.LookAt(Camera.main.transform, Vector3.up);
            newText.AssignText($"Got the {key} Key");

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            player = null;
        }
    }
   
}
