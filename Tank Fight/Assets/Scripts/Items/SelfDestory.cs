using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    public GameObject item;
    public void Off()
    {
        Destroy(item);
    }
}
