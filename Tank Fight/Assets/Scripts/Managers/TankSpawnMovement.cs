using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpawnMovement : MonoBehaviour
{
    public int playerNumber = 1;
    public GameObject TankSpawnPoint;
    public Rigidbody rb;

    private float verticalValue;
    private float horizontalValue;
    private bool chosen = false;
    private Vector3 position;
    void Update()
    {
        if (!chosen)
        {
            verticalValue = Input.GetAxis("Vertical" + playerNumber) * 10f;
            horizontalValue = Input.GetAxis("Horizontal" + playerNumber) * 10f;
            rb.velocity = new Vector3 (horizontalValue+ verticalValue, 0f, -horizontalValue+ verticalValue);
            //Vector3 location = TankSpawnPoint.transform.position; 
            //TankSpawnPoint.transform.position = new Vector3(location.x + horizontalValue, location.y, location.z + verticalValue);
            //会穿墙，换了方法
        }
        else
        {
            transform.position = position;
        }
        if (Input.GetButtonDown("Fire" + playerNumber))
        {
            rb.velocity = Vector3.zero;
            position = transform.position;
            GetComponentInChildren<Light>().enabled = true;
            chosen = true;
        }
    }
    public bool IsChosen()
    {
        return chosen;
    }
    public void Resetchosen()
    {
        chosen=false;
        GetComponentInChildren<Light>().enabled = false;
    }
}
