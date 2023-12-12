using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrol : MonoBehaviour
{
    public float dmg_ = 5f;
    public float speed_ = 1.5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank")
        {
            other.GetComponent<TankAttackABC>().dmgUp += dmg_;
            other.GetComponent<TankMovementABC>().speed += speed_;
            gameObject.SetActive(false);
        }
    }
}
