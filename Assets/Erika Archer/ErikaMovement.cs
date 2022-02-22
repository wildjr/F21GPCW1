using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErikaMovement : MonoBehaviour
{
    public float moveSpeed;
    public float deadZone;
    public float arrowForce;

    public Camera playerCamera;
    public GameObject bow;
    public GameObject arrowPrefab;
    public GameObject arrowInstantiatePoint;

    private Animator animator;
    private Rigidbody rb;
    private GameObject arrowCurrent = null;

    public GameObject[] weapons;
    private enum Weapon { Arrow, Sword };
    private const int numberOfWeapons = 2;
    private Weapon currentWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        animator.SetInteger("currentWeapon", (int)currentWeapon);

        foreach (GameObject g in weapons)
        {
            g.SetActive(false);
        }

        weapons[(int)currentWeapon].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerCamera.transform);
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y + 180, 0);

        Move();
        Weapons();
    }

    void Move()
    {
        int verticalMovement = 0;
        int horizontalMovement = 0;

        if (Input.GetAxis("Vertical") > deadZone)
        {
            verticalMovement = 1;
        }
        else if (Input.GetAxis("Vertical") < deadZone * -1)
        {
            verticalMovement = -1;
        }
        
        if (Input.GetAxis("Horizontal") > deadZone)
        {
            horizontalMovement = 1;
        }
        else if (Input.GetAxis("Horizontal") < deadZone * -1)
        {
            horizontalMovement = -1;
        }

        rb.velocity = new Vector3(playerCamera.transform.forward.x * verticalMovement, 0, playerCamera.transform.forward.z * verticalMovement) * moveSpeed;
        rb.velocity += new Vector3(playerCamera.transform.right.x * horizontalMovement, 0, playerCamera.transform.right.z * horizontalMovement) * moveSpeed;

        if (rb.velocity.magnitude > deadZone)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void Weapons()
    {
        if (Input.GetMouseButtonDown(1) && !animator.GetBool("isAiming"))
        {
            currentWeapon++;
            if ((int)currentWeapon == numberOfWeapons)
            {
                currentWeapon = 0;
            }

            animator.SetInteger("currentWeapon", (int)currentWeapon);

            foreach (GameObject g in weapons)
            {
                g.SetActive(false);
            }

            weapons[(int)currentWeapon].SetActive(true);
        }

        switch(currentWeapon)
        {
            case Weapon.Arrow:
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        arrowCurrent.transform.parent = null;
                        animator.SetTrigger("shoot");
                        Rigidbody arrowrb = arrowCurrent.GetComponentInChildren<Rigidbody>();
                        arrowrb.isKinematic = false;
                        arrowrb.useGravity = true;
                        arrowrb.AddForce(new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z) * arrowForce);
                        arrowCurrent.GetComponentInChildren<TrailRenderer>().emitting = true;
                        arrowCurrent = null;
                    }
                    else
                    {
                        animator.ResetTrigger("shoot");
                    }

                    if (Input.GetMouseButton(0))
                    {
                        animator.SetBool("isAiming", true);

                        if (arrowCurrent == null)
                        {
                            arrowCurrent = Instantiate(arrowPrefab, arrowInstantiatePoint.transform.position, arrowInstantiatePoint.transform.rotation, arrowInstantiatePoint.transform);
                        }
                    }
                    else
                    {
                        animator.SetBool("isAiming", false);

                        if (arrowCurrent != null)
                            Destroy(arrowCurrent);
                    }
                }
                break;
            case Weapon.Sword:
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        animator.SetTrigger("shoot");
                    }
                    else
                    {
                        animator.ResetTrigger("shoot");
                    }
                }
                break;
            default:
                Debug.LogError("Unknown weapon state " + currentWeapon + " " + currentWeapon.ToString());
                break;
        }
        
    }

    //TODO: Find a better place for this
    static void EditorPause()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
        #endif
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            animator.SetBool("isDead", true);

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                g.SendMessage("StartCelebrating");
            }

            this.enabled = false;
        }
    }
}
