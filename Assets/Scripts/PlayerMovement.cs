using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;

    public Animator anim;
    public CharacterController controller;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    List<GameObject> playersInParty = new List<GameObject>();
    int totalPlayerLevel;

    public List<Vector3> nearbyLocations = new List<Vector3>();

    private void Start()
    {
        StartCoroutine(WaitForFrame());

    }
    IEnumerator WaitForFrame()
    {
        yield return new WaitForEndOfFrame();
        CreatePlayersInOverworld();
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            anim.SetBool("isRunning", true);

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        
        else
            anim.SetBool("isRunning", false);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CalculatePartyLevel();
        }
    }
    void CalculatePartyLevel()
    {
        for (int i = 0; i < GameManager.inCurrentParty.Count; i++)
        {
            playersInParty.Add(Resources.Load<GameObject>(GameManager.inCurrentParty[i]));
            totalPlayerLevel += playersInParty[i].GetComponent<Unit>().CurrentLevel;
        }
        GameManager.avgPlayerLevelPerPlayerInParty = totalPlayerLevel / GameManager.inCurrentParty.Count;
    }

    void CreatePlayersInOverworld()
    {
        print(GameManager.inCurrentParty.Count);
        for (int i = 0; i < GameManager.inCurrentParty.Count; i++)
        {
            if (i == 0)
                print("skip this");
            else
                Instantiate(Resources.Load<GameObject>("Overworld/" + GameManager.inCurrentParty[i]), this.transform.position + nearbyLocations[i], this.transform.rotation);
        }
    }
}
