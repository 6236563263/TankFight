using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : MonoBehaviour
{
    public float restoreHp = 30;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank")
        {
            TankHpABC th = other.gameObject.GetComponent<TankHpABC>();
            th.RestoreHealth(restoreHp);
            gameObject.SetActive(false);
        }
    }
}
