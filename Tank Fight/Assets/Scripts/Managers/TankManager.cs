using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class TankManager
{
    private ArrayList tanks = new ArrayList();
    private int gameRoundCount = 0; //胜利所需局数
    public TankManager(int gameRoundCount)
    {
        this.gameRoundCount = gameRoundCount;
    }
    public void AddTank(Tank tank) //将坦克存入tank ArrayList
    {
        tanks.Add(tank);
    }
    public Transform[] GetTanksTransforms()
    {
        Transform[] targets = new Transform[tanks.Count];
        for (int i = 0; i < tanks.Count; i++)
        {
            Tank tank = (Tank)tanks[i];
            targets[i] = tank.GetTankGameObject().transform;
        }
        return targets;
    }
    public bool IsOneTankLeft()
    {
        int TanksLeftNumber = 0;
        foreach (Tank tank in tanks)
        {
            if (tank.GetTankGameObject().activeSelf)
            {
                TanksLeftNumber++;
            }
        }
        return (TanksLeftNumber <= 1);
    }
    public Tank GetRoundWinner()
    {
        foreach (Tank tank in tanks)
        {
            if (tank.GetTankGameObject().activeSelf)
            {
                return tank;
            }
        }
        return null;
    }
    public Tank GetGameWinner()
    {
        foreach (Tank tank in tanks)
        {
            if (tank.GetRoundWinnerCount() == gameRoundCount)
            {
                return tank;
            }
        }
        return null;
    }
    public void EnableTanks(bool enable)
    {
        foreach (Tank tank in tanks)
        {
            tank.Enable(enable);
        }
    }
    public void ResetTanks()
    {
        foreach (Tank tank in tanks)
        {
            tank.Reset();
        }
    }
    public void StayTanks()
    {
        foreach (Tank tank in tanks)
        {
            tank.Stay();
        }
    }
    public void Light(bool boo)
    {
        foreach (Tank tank in tanks)
        {
            if (tank.GetTankGameObject().activeSelf)
            {
                tank.GetTankGameObject().GetComponentInChildren<Light>().enabled = boo;
            }
        }
    }
    public void SetShellBounce(bool boo)
    {
        foreach (Tank tank in tanks)
        {
            tank.Bounce(boo);
        }
    }
    public void SetDamageRate(float rate)
    {
        foreach (Tank tank in tanks)
        {
            tank.damageRate(rate);
        }
    }
    public void SetMustmove(bool boo)
    {
        foreach (Tank tank in tanks)
        {
            tank.mustMove(boo);
        }
    }
}
