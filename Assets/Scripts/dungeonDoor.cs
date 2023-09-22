using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dungeonDoor : MonoBehaviour {
	public List<string> keys;
    public GameObject textMessage;

    private GameObject player = null;

    void Update() {
        if (player && hasKeys()) {
            textMessage.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                removeKeys();
                Destroy(gameObject);
            }
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

    private bool hasKeys() {
        if (player == null) return false;

        List<dungeonKeyCarry> carriedKeys = new List<dungeonKeyCarry>(player.GetComponents<dungeonKeyCarry>());
        List<string> keyList = new List<string>();

        foreach (dungeonKeyCarry key in carriedKeys) {
            keyList.Add(key.key);
        }

        foreach (string key in keys) {
            if (keyList.Contains(key)) {
                continue;
			}

            return false;
		}

        return true;
	}

    private void removeKeys() {
        if (player == null) return;

        List<dungeonKeyCarry> carriedKeys = new List<dungeonKeyCarry>(player.GetComponents<dungeonKeyCarry>());

        for (int i = 0; i < carriedKeys.Count; i++) {
            if (keys.Contains(carriedKeys[i].key)) {
                Destroy(carriedKeys[i]);
			}
        }
    }

}
