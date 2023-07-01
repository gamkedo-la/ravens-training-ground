using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowInOverworld : MonoBehaviour
{
    NavMeshAgent nav;
    public Animator anim;
    Transform player;
    bool startFollow;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        StartCoroutine(WaitForFrame());
    }

    IEnumerator WaitForFrame()
    {
        yield return new WaitForEndOfFrame();
        player = GameObject.Find("OverworldPlayer").transform;
        startFollow = true;
    }

    void Update()
    {
        if (startFollow)
        {
            float dist = Vector3.Distance(player.position, transform.position);

            if (dist > nav.stoppingDistance)
            {
                anim.GetComponent<Animator>().SetBool("isRunning", true);

                nav.SetDestination(player.position);
                transform.LookAt(player.transform);
                // transform.rotation *= Quaternion.Euler(0, 90, 0);            
            }
            else
            {
                anim.GetComponent<Animator>().SetBool("isRunning", false);
            }
        }
    }
}
