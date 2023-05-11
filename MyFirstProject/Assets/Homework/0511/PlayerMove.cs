using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody Player;
    public float Power;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"[Before] {Player.name}");
        Player.name = "Change_Player";
        Debug.Log($"[After] {Player.name}");

        Player.AddForce(Vector3.up * Power, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log($"[Update] : {Player.name}");

    }
}
