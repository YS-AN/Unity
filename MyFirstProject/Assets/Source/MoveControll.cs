using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveControll : MonoBehaviour
{
    public Rigidbody player;
    public float speed = 1f;
    private float distanceX;
    private float distanceZ;
    private Vector3 cameramMv;
    // Start is called before the first frame update
    void Start()
    {
        distanceX = this.gameObject.transform.position.x - Camera.main.transform.position.x;
        distanceZ = this.gameObject.transform.position.z - Camera.main.transform.position.z;
        cameramMv = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // = Camera.main.transform.position;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.AddForce(0f, 0f, speed, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            player.AddForce(0f, 0f, -speed, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            player.AddForce(speed, 0f, 0f, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.AddForce(-speed, 0f, 0f, ForceMode.Impulse);
        }


        //player.AddForce(move);

        //move.x = move.x - distance;

       // move = player.position;
        //cameramMv.x = move.x - distance;
        //Camera.main.transform.Translate(cameramMv);
    }

    private void LateUpdate()
    {
        var position = Camera.main.transform.position;
       //*
       if (Input.GetKey(KeyCode.UpArrow))
       {
            position.z = player.position.z - distanceZ;
       }
       else if (Input.GetKey(KeyCode.DownArrow))
       {
            position.z = player.position.z - distanceZ;
        }
       else if (Input.GetKey(KeyCode.RightArrow))
       {
            position.x = player.position.x - distanceX;
        }
       else if (Input.GetKey(KeyCode.LeftArrow))
       {
            position.x = player.position.x - distanceX;
        }

        Camera.main.transform.position = position;
       //*/

        //Camera.main.transform.Translate(cameramMv);


    }
}
