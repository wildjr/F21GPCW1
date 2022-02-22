using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    public float runSpeed;
    private GameObject player;
    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform);
        rb.velocity = transform.forward * runSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("celebrate");
            runSpeed = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            animator.SetBool("isDead", true);
            rb.isKinematic = true;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.enabled = false;
        }
    }

    public void StartCelebrating()
    {
        animator.SetTrigger("celebrate");
        runSpeed = 0;
    }
}
