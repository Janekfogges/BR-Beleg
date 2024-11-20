using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private int desiredLane = 1; // 0-left, 1-middle, 2-right

    public float forwardSpeed = 10f;
    public float laneDistance = 4f; // Distance between 2 lanes

    public float jumpForce;
    public float gravity = -20f;

    public Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Forward movement
        direction.z = forwardSpeed;

        // Gravity and jumping
        if (controller.isGrounded)
        {
            direction.y = -1; // Keep the player on the ground
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Jump
            {
                Jump();
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime; // Apply gravity
        }

        // Lane switching
        if (Input.GetKeyDown(KeyCode.RightArrow)) // Move right
        {
            desiredLane++;
            if (desiredLane > 2) desiredLane = 2; // Stay within bounds
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // Move left
        {
            desiredLane--;
            if (desiredLane < 0) desiredLane = 0; // Stay within bounds
        }

        // Calculate target position for lane switching
        float targetX = (desiredLane - 1) * laneDistance; // Calculate the target X position
        float deltaX = targetX - transform.position.x;

        // Smoothly move towards the target lane
        direction.x = deltaX * 10f; // Control the speed of movement between lanes

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
        // Apply movement using the CharacterController
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        anim.SetTrigger("Jump"); // Trigger jump animation
    }

    private void Slide()
    {
        // Start Slide animation
        anim.SetTrigger("Slide");

        // Option 1: Automatisch nach Slide zurück zum Run
        // Nichts weiter notwendig, da der Animator automatisch wechselt, wenn die Exit Time erreicht ist.

        // Option 2: Manuell zurück zum Run (z. B. mit einem Bool)
        StartCoroutine(EndSlide());
    }

    private IEnumerator EndSlide()
    {
        // Warte, bis die Slide-Dauer vorbei ist
        yield return new WaitForSeconds(1.0f); // Zeit der Animation

        // Setze das isRunning-Flag
        anim.SetBool("isRunning", true);
    }
}
