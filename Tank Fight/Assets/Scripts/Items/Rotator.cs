using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed=60f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
    }
}
