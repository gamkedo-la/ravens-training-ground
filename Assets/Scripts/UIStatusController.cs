using System.Collections;
using System.Collections.Generic;
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
        while (statusChanges.Count != 0)
        {
            yield return StartCoroutine(damageNumberController.Create(statusChanges[0], gameObject));
            yield return new WaitForSeconds(waitTime);
            statusChanges.RemoveAt(0);  
        }

    }
    public void AddStatusChange(string statusChange)
    {
        statusChanges.Add(statusChange);
    }
}
