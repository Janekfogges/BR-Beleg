using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private int desiredLane = 1; //0-links, 1-mitte, 2-rechts

    public float forwardSpeed;
    public float laneDistance = 4; //Distanz zwischen 2 Lanes 

    public float jumpForce;
    public float Gravity = -20;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.z = forwardSpeed;

        //wenn man auf dem Boden ist kann man springen, sonst könnte man immer wieder von der Luft aus springen
        if(controller.isGrounded)
        {
            direction.y = -1;
            //Pfeli oben = springen
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }else
        {
            direction.y += Gravity * Time.deltaTime; //spieler fällt

        }


        //inputs checken und dann Lanes wechseln
        //rechte maus nach rechts wechseln
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if(desiredLane == 3) {
                desiredLane = 2;
            }
        }
        //linke maus nach links wechseln
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        //Berechnen wo man in der Zukunft ist
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        } else if(desiredLane == 2) {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.fixedDeltaTime);


    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);

    }

    private void Jump()
    {
        direction.y = jumpForce;
    }
}
