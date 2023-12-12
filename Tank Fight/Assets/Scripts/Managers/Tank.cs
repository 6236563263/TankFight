using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

[Serializable]
public class Tank
{
    public Color skinColor;
    public Transform spawnPoint;
    private GameObject tankGameObject;
    private int roundWinnerCount = 0; //胜利回合总数

    private void SetTankColor(Color color)
    {
        MeshRenderer[] renderers = tankGameObject.GetComponentsInChildren<MeshRenderer>(); 
        foreach(MeshRenderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
    private void SetTankPlayerNumber(int playerNumber)
    {
        TankMovementABC tm = tankGameObject.GetComponent<TankMovementABC>();
        tm.SetPlayerNumber(playerNumber);
        TankAttackABC ta = tankGameObject.GetComponent<TankAttackABC>();
        ta.SetPlayerNumber(playerNumber);
    }
    public void SetUp(GameObject tankGameObject,int playerNumber)
    {
        this.tankGameObject = tankGameObject;
        SetTankColor(this.skinColor);
        SetTankPlayerNumber(playerNumber);
    }
    public GameObject GetTankGameObject()
    {
        return tankGameObject;
    }
    public int GetRoundWinnerCount()
    {
        return roundWinnerCount;
    }
    public int GetTankPlayerNumber()
    {
        int number;
        TankMovementABC tm = tankGameObject.GetComponent<TankMovementABC>();
        number=tm.GetPlayerNumber();
        return number;
    }
    public int GetWinNumber()
    {
        return roundWinnerCount;
    }
    public void IncreaseRoundWinnerCount()
    {
        roundWinnerCount++;
    }
    public void Enable(bool enable)
    {
        TankAttackABC ta = this.tankGameObject.GetComponent<TankAttackABC>();
        ta.enabled = enable;
        TankMovementABC tm = this.tankGameObject.GetComponent<TankMovementABC>();
        tm.enabled = enable;
        TankHpABC th = this.tankGameObject.GetComponent<TankHpABC>();
        th.enabled = enable;
    }
    public void Reset() 
    {
        tankGameObject.SetActive(true);
        tankGameObject.transform.position = spawnPoint.position;
        tankGameObject.transform.rotation = spawnPoint.rotation;
        TankHpABC th = this.tankGameObject.GetComponent<TankHpABC>();
        th.ResetHealth();
        TankAttackABC ta = this.tankGameObject.GetComponent<TankAttackABC>();
        ta.ResetCharge();
        ta.ReloadAmmo();
        ta.dmgUp = 0f;
        TankMovementABC tm = this.tankGameObject.GetComponent<TankMovementABC>();
        tm.speed = 8f;
    }
    public void Stay()
    {
        TankMovementABC tm = this.tankGameObject.GetComponent<TankMovementABC>();
        tm.ResetMovement();
    }
    public void Bounce(bool boo)
    {
        TankAttackABC ta = this.tankGameObject.GetComponent<TankAttackABC>();
        ta.bounce = boo;
    }
    public void damageRate(float rate)
    {
        TankHpABC th = this.tankGameObject.GetComponent<TankHpABC>();
        th.damageRate = rate;
    }
    public void mustMove(bool boo)
    {
        TankHpABC th = this.tankGameObject.GetComponent<TankHpABC>();
        th.mustMove = boo;
    }
}

