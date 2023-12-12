using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class TankAttackABC : MonoBehaviour
{
    public GameObject shellPrefab;
    public float fireForce = 10f;
    public float maxForce = 20f;
    public float minForce = 10f;
    public Transform firePoint;
    public Slider aimSilder;
    public int playerNumber = 1;
    public AudioSource audioSource;
    public AudioClip fireClip;
    public AudioClip loadPowerClip;
    public float reloadTime = 1.5f;
    public float dmgUp = 0;
    public bool bounce = false;

    static float maxAngular = -7f;
    static float angular;
    public TextMeshProUGUI ammoText;
    private Quaternion rotation;
    private float loadingTime = 0.6f;
    private bool shootable = true;
    private int maxAmmo = 6;
    private int leftAmmo = 6;
    private Quaternion textRotation;
    private bool isLaserOn=false;
    private bool shot = false;

    private void Awake()
    {
        textRotation = ammoText.transform.rotation;
    }
    void Update()
    {
        ammoText.transform.rotation = textRotation;
        if (Input.GetButtonDown("Reload"+playerNumber)&&!isLaserOn)
        {
            StartCoroutine(Reload());
        }
        if (leftAmmo > 0&& !isLaserOn)
        {
            if (Input.GetButtonDown("Fire" + playerNumber))
            {
                shootable = true;
                fireForce = minForce;
                angular = maxAngular; rotation = Quaternion.Euler(angular, 0, 0);
            }
            if (Input.GetButton("Fire" + playerNumber) && shootable)
            {
                if (audioSource.clip != loadPowerClip || !audioSource.isPlaying)
                {
                    audioSource.clip = loadPowerClip;
                    audioSource.Play();
                }
                fireForce = fireForce + Time.deltaTime * (maxForce - minForce) / loadingTime;
                if (angular <= -2)
                {
                    angular = angular + Time.deltaTime / loadingTime * 10f;
                    rotation = Quaternion.Euler(angular, 0, 0);
                }
                if (fireForce >= maxForce)
                {
                    shootable = false;
                }
            }
            else if (Input.GetButtonUp("Fire" + playerNumber))
            {
                fire();
                shootable = false;
                fireForce = minForce;
            }
            ShowAmmoCount();
            UpdateSlider();
        }
        else if(isLaserOn)
        {
            ResetAimSlider();
            if (Input.GetButtonDown("Fire" + playerNumber))  //…‰ª˜
            {
                LaserBeam lb = GetComponent<LaserBeam>();
                lb.ShootLaser();
                shot = true;
            }
            if(Input.GetButton("Fire" + playerNumber)&&shot)  //«–ªªœ‘ æ
            {
                ShowAmmoCount();
            }
            if(Input.GetButtonUp("Fire" + playerNumber)&&shot)
            {
                isLaserOn = false;
            }
        }
    }
    void UpdateSlider()
    {
        float value = (fireForce - minForce) / (maxForce - minForce);
        aimSilder.value = value;
    }
    void fire()
    {
        audioSource.clip = fireClip;
        audioSource.Play();
        GameObject shell = Instantiate(shellPrefab, firePoint.position, firePoint.rotation * rotation);
        shell.GetComponent<Rigidbody>().velocity = shell.transform.forward * fireForce;
        shell.GetComponent<ShellExplosion>().maxDamage += dmgUp;
        shell.GetComponent<ShellExplosion>().minDamage += dmgUp;
        shell.GetComponent<ShellExplosion>().SetBounce(bounce);
        leftAmmo--;
    }
    private IEnumerator Reload()
    {
        leftAmmo = 0;
        aimSilder.value = 0;
        ammoText.text = "Reloading...";
        yield return new WaitForSeconds(reloadTime);
        ReloadAmmo();
    }
    public void ReloadAmmo()
    {
        ammoText.transform.rotation = textRotation;
        leftAmmo = maxAmmo;
        ammoText.text = "°Ò°Ò°Ò°Ò°Ò°Ò";
    }
    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }
    public void ResetAimSlider()
    {
        aimSilder.value = 0;
    }
    public void ResetCharge()
    {
        ResetAimSlider();
        audioSource.clip = null;
        fireForce = minForce;
    }
    public void LaserOn()
    {
        ammoText.text = "Laser!";
        shot = false;
        isLaserOn = true;
    }
    public void ShowAmmoCount()
    {
        switch (leftAmmo)
        {
            case 6: ammoText.text = "°Ò°Ò°Ò°Ò°Ò°Ò"; break;
            case 5: ammoText.text = "°Ò°Ò°Ò°Ò°Ò"; break;
            case 4: ammoText.text = "°Ò°Ò°Ò°Ò"; break;
            case 3: ammoText.text = "°Ò°Ò°Ò"; break;
            case 2: ammoText.text = "°Ò°Ò"; break;
            case 1: ammoText.text = "°Ò"; break;
            default: ammoText.text = "Out of Ammo"; break;
        }
    }
}
