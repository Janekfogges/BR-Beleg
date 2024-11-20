using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public Transform target; //Player
    public Vector3 offset; //zw Camera und Player

    void Start()
    {
        //Abstand der Kamera zum Player berechnen
        offset = transform.position - target.position;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //Kamera folgt Spieler
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.deltaTime);


    }
}
