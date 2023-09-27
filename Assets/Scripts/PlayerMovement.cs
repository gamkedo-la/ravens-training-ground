using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;

    public Animator anim;
    public CharacterController controller;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    GameObject cameraEffect;
    bool cantMove;
    GameObject generate;

    public string battleSceneToLoad = "BattleScene";

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

        generate = GameObject.Find("Generator");

        if (!generate.transform.GetChild(1).gameObject.activeSelf)
        {
            foreach (Transform child in generate.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        this.gameObject.transform.position = GameManager.player;
        CreatePlayersInOverworld();
        cameraEffect = GameObject.Find("CameraEffect");

        yield return new WaitForSeconds(1);
        this.GetComponent<CapsuleCollider>().enabled = true;
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (!cantMove)
        {
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
        for (int i = 0; i < GameManager.inCurrentParty.Count; i++)
        {
            print(GameManager.inCurrentParty[i]);
        }

        if (GameManager.inCurrentParty.Count > 1)
            for (int i = 1; i < GameManager.inCurrentParty.Count; i++)
                Instantiate(Resources.Load<GameObject>("Overworld/" + GameManager.inCurrentParty[i]), this.transform.position + nearbyLocations[i], this.transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FrontTrigger")
        {
            GameManager.initiativeSiezedByEnemy = true;
            cantMove = true;

            for (int i = 0; i < cameraEffect.transform.childCount; i++)
            {
                cameraEffect.transform.GetChild(i).gameObject.SetActive(true);
            }

            StartCoroutine(Waiting(1.5f));
        }

        if (other.tag == "Enemy")
        {
            print("here");
            if (other.GetComponent<SphereCollider>().GetType() == typeof(SphereCollider))
            {
                print("front");
                other.gameObject.GetComponent<RoamingMonster>().FrontEntered();
            }
            if (other.GetComponent<CapsuleCollider>().GetType() == typeof(CapsuleCollider))
            {
                print("rear");
                other.gameObject.GetComponent<RoamingMonster>().RearEntered();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
            {
                print("rear");
                other.gameObject.GetComponent<RoamingMonster>().RearEntered();
            }
        }
    }

    IEnumerator Waiting(float loadTime)
    {
        PreserveLocationOfOverworld();
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene("BattleScene");
    }

    public void PreserveLocationOfOverworld()
    {
        GameManager.player = this.transform.position;
        print(GameManager.player);

        for (int i = 0; i < playersInParty.Count; i++)
        {
            GameManager.party[i] = this.transform.position + nearbyLocations[i];
            print(GameManager.party[i]);
        }

        SceneManager.LoadScene(battleSceneToLoad);
    }
}
