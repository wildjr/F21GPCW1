using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    public float runSpeed;
    private GameObject player;
    private Rigidbody rb;
    private Animator animator;

    private int health = 1;
    private enum AIState {AttackPlayer, CollectHealth};
    private AIState currentState;
    private float lastCheckedState;
    private GameObject currentTarget = null;
    public int reevaluateEveryXSeconds;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentState = AIState.AttackPlayer;
        lastCheckedState = Time.time;
        currentTarget = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            this.transform.LookAt(currentTarget.transform);
            rb.velocity = new Vector3(transform.forward.x * runSpeed, 0, transform.forward.z * runSpeed);
        }

        if (Time.time - lastCheckedState > reevaluateEveryXSeconds || currentTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject[] healthOrbs = GameObject.FindGameObjectsWithTag("Health");

            if (healthOrbs.Length == 0)
            {
                currentState = AIState.AttackPlayer;
                currentTarget = player;
                lastCheckedState = Time.time;
            }
            else
            {
                //Find closest health orb
                float lowestDistance = Vector3.Distance(healthOrbs[0].transform.position, this.transform.position);
                GameObject orbTarget = healthOrbs[0];

                foreach(GameObject h in healthOrbs)
                {
                    float currentDistance = Vector3.Distance(h.transform.position, this.transform.position);

                    if (currentDistance < lowestDistance)
                    {
                        lowestDistance = currentDistance;
                        orbTarget = h;
                    }
                }

                float orbScore = Random.Range(0.0f, 1.0f) * 100/(health * lowestDistance);
                float playerScore = Random.Range(0.0f, 1.0f) * health * 100/ (Vector3.Distance(player.transform.position, this.transform.position));

                if (orbScore > playerScore)
                {
                    currentState = AIState.CollectHealth;
                    currentTarget = orbTarget;
                    lastCheckedState = Time.time;
                }
                else
                {
                    currentState = AIState.AttackPlayer;
                    currentTarget = player;
                    lastCheckedState = Time.time;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("celebrate");
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            health--;

            if (health <= 0)
            {
                animator.SetBool("isDead", true);
                rb.isKinematic = true;
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.enabled = false;
            }
            else
            {
                animator.SetTrigger("isHit");
            }
        }
        if (other.gameObject.tag == "Health")
        {
            health++;
            Destroy(other.gameObject);
        }
    }

    public void StartCelebrating()
    {
        animator.SetTrigger("celebrate");
        runSpeed = 0;
    }
}
