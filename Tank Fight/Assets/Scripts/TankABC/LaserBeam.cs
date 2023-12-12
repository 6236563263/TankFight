using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private bool shoot = false; //判断是否按下发射键
    public LineRenderer lineRenderer;
    public GameObject explosionPrefab;
    private void Update()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1.644f, transform.position.z);
        Ray ray = new Ray(origin, transform.forward);
        RaycastHit hitInfo;
        lineRenderer.SetPosition(0,origin); //设置起点
        if (Physics.Raycast(ray,out hitInfo))
        {
            //float beamLength = Vector3.Distance(transform.position, hitInfo.point); //计算两点间长度
            lineRenderer.SetPosition(1, hitInfo.point);
            //Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //Debug.Log(hitInfo.collider.gameObject.name);
            if (shoot)
            {
                if (hitInfo.collider.gameObject.tag == "Tank")
                {
                    hitInfo.collider.gameObject.GetComponent<TankHpABC>().TakeDamage(40);
                    GameObject tankExplosionInstance = Instantiate(explosionPrefab);
                    tankExplosionInstance.transform.position = hitInfo.transform.position;
                    ParticleSystem tankExplosionEffect = tankExplosionInstance.GetComponent<ParticleSystem>();
                    tankExplosionEffect.Play();
                }
                shoot = false;
                lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                lineRenderer.SetPosition(1, new Vector3(0,0,0));
                this.enabled = false;
            }
        }
    }
    public void ShootLaser()
    {
        shoot = true;
    }
}

