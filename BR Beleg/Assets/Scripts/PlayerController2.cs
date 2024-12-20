using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private int desiredLane = 1; // 0-left, 1-middle, 2-right

    public float forwardSpeed;
    public float maxSpeed;
    public float laneDistance = 4f; // Distance between 2 lanes

    public float jumpForce;
    public float gravity = -20f;
    public Animator anim;
    public bool isGrounded; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
        {
            return;
        }

        //Geschwindigkeit erh�hen
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }


        isGrounded = controller.isGrounded;
        anim.SetBool("isGrounded",controller.isGrounded);
        // Forward movement
        direction.z = forwardSpeed;

        // Gravity and jumping
        if (controller.isGrounded)
        {
            direction.y = -1; // Keep the player on the ground
            if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeDown) // Jump
            {
                
                Jump();
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime; // Apply gravity
        }

        // Lane switching
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight) // Move right
        {
            desiredLane++;
            if (desiredLane > 2) desiredLane = 2; // Stay within bounds
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft) // Move left
        {
            desiredLane--;
            if (desiredLane < 0) desiredLane = 0; // Stay within bounds
        }

        // Calculate target position for lane switching
        float targetX = (desiredLane - 1) * laneDistance; // Calculate the target X position
        float deltaX = targetX - transform.position.x;

        // Smoothly move towards the target lane
        direction.x = deltaX * 10f; // Control the speed of movement between lanes


        if (Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown) // Slide
        {

            Slide();
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
        {
            return;
        }
        // Apply movement using the CharacterController
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        StartCoroutine(resetJumpState(1.0f));
        direction.y = jumpForce;
        anim.SetBool("jumpPressed",true);
        // anim.SetTrigger("Jump"); // Trigger jump animation
    }

    private void Slide()
    {
        StartCoroutine(HeightPingPong(2f, 0.75f, 1.0f));
        if (isGrounded) anim.SetTrigger("slide");
    }

    //um durch den Colider durchzurollen
    private IEnumerator HeightPingPong(float startHeight, float midHeight, float duration)
    {
        float halfDuration = duration / 2f;
        float elapsedTime = 0f;

        // Interpolate from startHeight to midHeight
        while (elapsedTime < halfDuration)
        {
            controller.height = Mathf.Lerp(startHeight, midHeight, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the height is exactly midHeight
        controller.height = midHeight;

        elapsedTime = 0f;

        // Interpolate from midHeight back to startHeight
        while (elapsedTime < halfDuration)
        {
            controller.height = Mathf.Lerp(midHeight, startHeight, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the height is exactly startHeight
        controller.height = startHeight;
    }
    
    private IEnumerator resetJumpState(float duration)
    {
        yield return new WaitForSeconds(duration);
        anim.SetBool("jumpPressed",false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
}
