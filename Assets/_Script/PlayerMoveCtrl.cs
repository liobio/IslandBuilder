using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCtrl : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public Transform cameraTarget;
    public float speed = 5;
    public float angularSpeed = 10;
    public float jumpForce = 1;
    // Start is called before the first frame update
    void Start()
    {
    }
    public bool isJump = false;
    public Vector3 forward = Vector3.forward;
    private void Update()
    {

        cameraTarget.position = new Vector3(transform.position.x, cameraTarget.position.y, transform.position.z);
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            transform.forward = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            animator.SetTrigger("Jump");
        }
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(speed * h, rb.velocity.y, speed * v);


    }
    public void AnimaJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }
    public void StopJump()
    {
        isJump = false;
    }
}
