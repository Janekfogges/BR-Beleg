using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private int desiredLane = 1; // 0-left, 1-middle, 2-right

    public float forwardSpeed;
    public float laneDistance = 4; // Distance between 2 lanes

    public float jumpForce;
    public float Gravity = -20;
    public Animator anim; // Link the Animator component in the Inspector

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.z = forwardSpeed;

        // Check if grounded and handle jumping
        if (controller.isGrounded)
        {
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Jump
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        // Check input for lane switching
        if (Input.GetKeyDown(KeyCode.RightArrow)) // Move Right
        {
            desiredLane++;
            if (desiredLane == 3) desiredLane = 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // Move Left
        {
            desiredLane--;
            if (desiredLane == -1) desiredLane = 0;
        }

        // Calculate target position for lane switching
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.fixedDeltaTime);

        // Update animator parameters
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Speed", forwardSpeed);

        if (Input.GetKeyDown(KeyCode.DownArrow)) // Slide
        {
            Slide();
        }
    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        anim.SetBool("Jump", true); // Trigger jump animation
        StartCoroutine(ResetTrigger("Jump")); // Reset after triggering
    }

    private void Slide()
    {
        anim.SetBool("Slide", true); // Trigger slide animation
        StartCoroutine(ResetTrigger("Slide")); // Reset after triggering
    }

    private IEnumerator ResetTrigger(string triggerName)
    {
        yield return new WaitForSeconds(0.1f); // Allow time for the animation to start
        anim.SetBool(triggerName, false);
    }
}
