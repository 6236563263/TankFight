using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform []players;
    private bool zoom=false;
    private bool observe = false;
    private float x=0f,y=0f;
    private float cameraSize;
    private Camera ca;
    private Vector3 offset;
    private Vector3 zoomPosition;
    private Vector3 distance;
    void Start()
    {
        ca = this.GetComponent<Camera>();
    }
    void Update()
    {
        if (zoom)
        {
            transform.position = GetCenterPoint() + distance * Mathf.Sqrt(y) / 10 + offset;
            if (y <= 100)
            {
                y = y + Time.deltaTime * 135;
            }
            ca.orthographicSize = cameraSize / x;
            if (ca.orthographicSize < 5)
            {
                ca.orthographicSize = 5;
            }
            x = x + Time.deltaTime * 0.7f;
        }
        else if(observe)
        {
            transform.position = new Vector3(0, 0, 0);
            ca.orthographicSize = 30;
        }
        else
        {
            transform.position = GetCenterPoint() + offset;
            ca.orthographicSize = GetReqScreenSize();
        }
    }
    Vector3 GetCenterPoint()
    {
        Vector3 point = new Vector3();
        foreach (Transform player in players)
        {
            point = point + player.position;
        }
        point = point / players.Length;
        return point;
    }
    float GetReqScreenSize()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 centerPointAtCamera = transform.InverseTransformPoint(centerPoint);
        float maxSize = 0;
        foreach(Transform player in players)
        {
            Vector3 tankPointAtCamera = transform.InverseTransformPoint(player.position);
            Vector3 distance = tankPointAtCamera - centerPointAtCamera;
            maxSize = Mathf.Max(maxSize, Mathf.Abs(distance.y));
            maxSize = Mathf.Max(maxSize, Mathf.Abs(distance.x)/ca.aspect);
        }
        return maxSize+5;
    }
    public void SetTanksTransforms(Transform[] players)
    {
        this.players = players;
    }
    public void GetOffset()
    {
        offset = new Vector3(-109, 103, -82);   //不会程序算，只好自己计算向量了TAT
    }
    public void Zoom(bool boo,GameObject tank)
    {
        zoom = boo;
        if (zoom)
        {
            x = 1;
            y = 0;
            zoomPosition = tank.transform.position;
            distance = zoomPosition -GetCenterPoint();
            cameraSize = ca.orthographicSize;
        }
    }
    public void ObseveAll(bool boo)
    {
        observe = boo;
    }
}
