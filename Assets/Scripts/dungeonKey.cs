using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dungeonKey : MonoBehaviour {
    public string key;

    private GameObject player = null;

    void Update() {
        if (player && Input.GetKeyDown(KeyCode.Space)) {
            player.AddComponent<dungeonKeyCarry>().key = key;
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
