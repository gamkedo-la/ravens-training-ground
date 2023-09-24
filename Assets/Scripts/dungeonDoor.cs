using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dungeonDoor : MonoBehaviour {
	public List<string> keys;
    public GameObject textMessageHasKeys;
    public GameObject textMessageNeedsKeys;

    private GameObject player = null;
    private bool isOpen = false;

    void Update() {
        if (isOpen) return;

        if (player && hasKeys()) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                removeKeys();

                Animator doorAnimator = GetComponent<Animator>();
                if (doorAnimator) {
                    doorAnimator.SetTrigger("Open");
                } else {
                    Destroy(gameObject);
                }

                textMessageHasKeys.SetActive(false);
                textMessageNeedsKeys.SetActive(false);

                isOpen = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.gameObject;

            EvaluateKeyText();

            if (hasKeys()) {
                textMessageHasKeys.SetActive(true);
            }
            textMessageNeedsKeys.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            player = null;

            textMessageHasKeys.SetActive(false);
            textMessageNeedsKeys.SetActive(false);
        }
    }

    private void EvaluateKeyText() {
        TMP_Text text = textMessageNeedsKeys.GetComponent<TMP_Text>();
        if (text) {
            Debug.Log("Here");
            string newText = "";

            List<dungeonKeyCarry> carriedKeys = new List<dungeonKeyCarry>(player.GetComponents<dungeonKeyCarry>());
            List<string> keyList = new List<string>();

            foreach (dungeonKeyCarry key in carriedKeys) {
                keyList.Add(key.key);
            }

            foreach (string key in keys) {
                if (keyList.Contains(key)) {
                    newText += $"<s>{key} Key</s>\n";
				} else {
                    newText += $"<b>{key} Key</b>\n";
                }
			}

            text.text = newText;
		}
	}

    private bool hasKeys() {
        if (player == null) return false;

        List<dungeonKeyCarry> carriedKeys = new List<dungeonKeyCarry>(player.GetComponents<dungeonKeyCarry>());
        List<string> keyList = new List<string>();

        if (carriedKeys.Count == 0) return false;

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
