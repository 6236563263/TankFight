using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank")
        {
            LaserBeam lb = other.gameObject.GetComponent<LaserBeam>();
            lb.enabled = true;
            TankAttackABC ta = other.gameObject.GetComponent<TankAttackABC>();
            ta.LaserOn();
            gameObject.SetActive(false);
        }
        
    }
}
