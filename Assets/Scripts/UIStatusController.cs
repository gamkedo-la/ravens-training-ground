using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIStatusController : MonoBehaviour
{
    [SerializeField] DamageNumberController damageNumberController;
    public float waitTime = .5f;
    public List<string> statusChanges = new List<string>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(ShowStatusEffects());
        }
    }
    public IEnumerator ShowStatusEffects()
    {
        if (statusChanges.Count != 0)
        {
            while (statusChanges.Count > 0)
            {
                yield return StartCoroutine(damageNumberController.Create(statusChanges[0], gameObject));

                print(statusChanges[0]);
                statusChanges.RemoveAt(0);
                
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
    public void AddStatusChange(string statusChange)
    {
        statusChanges.Add(statusChange);
    }
}
