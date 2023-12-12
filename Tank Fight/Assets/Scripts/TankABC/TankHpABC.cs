using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class TankHpABC : MonoBehaviour
{
    public float hp = 100;
    public Slider healthSlider;
    public GameObject tankExplosion;
    public float damageRate = 1f;
    public bool mustMove = false;
    private float restoredHp = 0;
    private float maxRestoredHp = 0;

    //public AudioSource explosionAudio;

    void Start()
    {
        SetHealth(hp);
    }
    void Update()
    {
        SetHealth(hp);
        RestoreHp();
        if(mustMove)
        {
            if(gameObject.GetComponent<Rigidbody>().velocity == new Vector3(0,0,0))
            {
                DecreaseHp();
            }
        }
        if (hp>100)
        {
            hp = 100;
        }
        if (hp <= 0)
        {
            OnDeath();
        }
    }
    public void TakeDamage(float dmg)
    {
        hp -= dmg*damageRate;
        SetHealth(hp);
        Debug.Log(hp);
        maxRestoredHp = 0;
    }
    void SetHealth(float hp)
    {
        healthSlider.value = hp/100;
    }
    void OnDeath()
    {
        GameObject tankExplosionInstance= Instantiate(tankExplosion);
        tankExplosionInstance.transform.position = transform.position;
        ParticleSystem tankExplosionEffect = tankExplosionInstance.GetComponent<ParticleSystem>();
        tankExplosionEffect.Play();
        Destroy(tankExplosionInstance, tankExplosionEffect.main.duration);
        //explosionAudio.Play(); //生成死亡爆炸特效时直接产生音效，故不再次播放
        gameObject.SetActive(false);
    }
    public void ResetHealth()
    {
        hp = 100;
        SetHealth(hp);
    }
    public void RestoreHealth(float health)
    {
        restoredHp = 0;
        maxRestoredHp = health;
    }
    private void RestoreHp()
    {
        if (restoredHp < maxRestoredHp)
        {
            hp += 10 * Time.deltaTime;
            restoredHp += 10 * Time.deltaTime;
        }
    }
    private void DecreaseHp()
    {
        hp -= 20 * Time.deltaTime;
    }
}
