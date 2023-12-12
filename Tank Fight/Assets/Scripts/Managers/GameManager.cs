using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Tank[] tanks;
    public Items[] items;
    public GameObject shellPrefab;
    public GameObject tankPrefab;
    public GameObject laserPrefab;
    public GameObject wrenchPrefab;
    public GameObject petrolPrefab;
    public FollowTarget followTarget;
    public TankManager tankManager;
    public Text messageText;
    public float gameStartDelay = 2f;
    public float gameEndDelay = 3f;
    public int gameRoundCount = 3; //��Ҫʤ���Ļغ�����
    public Button restartButton;
    public Button quitButton;
    private int roundCount = 0;
    private bool airRaid = false;
    private float countTime = 0;
    void Start()
    {
#if UNITY_EDITOR
        gameRoundCount = 3;
#else
        gameRoundCount = OpeningManager.Instance.gameCount;
#endif
        showUIButtons(false);
        spawnTanks();
        StartCoroutine(GameLoop());
    }
    void Update()
    {
        countTime += Time.deltaTime;
        if (airRaid)
        {
            if (countTime >= Time.deltaTime * 15)
            {
                AirRaid();
                countTime = 0;
            }
        }
    }
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(SetGameMode());
        yield return StartCoroutine(SetSpawnPoints());
        yield return StartCoroutine(RoundStart());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnd());
    }
    private IEnumerator SetGameMode()
    {
        HighLight(false, null);
        followTarget.ObseveAll(true);
        if(roundCount!=0&&roundCount%2==0)
        {
            ResetGameMode();
            ChangeGameMode();
            yield return new WaitForSeconds(2);
        }
    }
    private IEnumerator SetSpawnPoints()
    {
        SetUpItems();
        messageText.text = "Select Your Spawn Point";
        EnableSpawnPoint(true);
        yield return new WaitUntil(IsSpawnPointChosen);
        EnableSpawnPoint(false);
        followTarget.ObseveAll(false);
    }
    private IEnumerator RoundStart()
    {
        initializeCamera();
        roundCount++;
        showUIButtons(false);
        messageText.text = "Round " + roundCount;
        tankManager.ResetTanks();
        tankManager.EnableTanks(false);     //ʹ̹�˲����ƶ�(ȡ�������ƶ���Ѫ���͹����ű����)
        HighLight(false, null);
        yield return new WaitForSeconds(gameStartDelay / 2);
        messageText.text = "Start!";
        yield return new WaitForSeconds(gameStartDelay / 2);
    }
    private IEnumerator RoundPlaying()
    {
        tankManager.EnableTanks(true);
        messageText.text = "";
        yield return new WaitUntil(tankManager.IsOneTankLeft);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator RoundEnd()
    {

        tankManager.EnableTanks(false);
        tankManager.StayTanks();

        Tank roundWinner = tankManager.GetRoundWinner();
        if (roundWinner != null)
        {
            roundWinner.IncreaseRoundWinnerCount();     //δ����TankManager��ֱ���޸�Tank
        }
        Tank gameWinner = tankManager.GetGameWinner();
        if (gameWinner != null)     //����Ϸʤ���߾ͽ���
        {
            HighLight(true, gameWinner.GetTankGameObject());
            messageText.text = "Player " + roundWinner.GetTankPlayerNumber() + " Win!";  //δ����TankManager��ֱ�ӵ���Tank���
            showUIButtons(true);
        }
        else if (roundWinner != null)   //û����Ϸʤ�������б���ʤ���߾ͼ���
        {
            HighLight(true, roundWinner.GetTankGameObject());
            messageText.text = "Player " + roundWinner.GetTankPlayerNumber() + " win this round!" + "\n" + tanks[0].GetWinNumber() + " - " + tanks[1].GetWinNumber(); //δ����TankManager��ֱ��ֱ�ӵ���Tank���
            yield return new WaitForSeconds(gameEndDelay);
            RemoveItems();
            HideTanks();
            StartCoroutine(GameLoop());
        }
        else  //��û�о���ƽ��
        {
            messageText.text = "Draw";
            yield return new WaitForSeconds(gameEndDelay);
            RemoveItems();
            HideTanks();
            StartCoroutine(GameLoop());
        }
    }
    void spawnTanks()
    {
        tankManager = new TankManager(gameRoundCount);
        for (int i = 0; i < tanks.Length; i++)
        {
            Tank tank = tanks[i];
            GameObject tankGameObject = Instantiate(tankPrefab, tank.spawnPoint.position, Quaternion.identity);
            tankGameObject.SetActive(false);    //�Ȳ���ʾ̹�ˣ�ֻ����tank
            tank.SetUp(tankGameObject, i + 1);
            tankManager.AddTank(tank);
        }
    }
    void HideTanks()
    {
        foreach (Tank tank in tanks)
        {
            tank.GetTankGameObject().SetActive(false);
        }
    }
    private void initializeCamera()
    {
        Transform[] targets = tankManager.GetTanksTransforms();

        followTarget.SetTanksTransforms(targets);
        followTarget.GetOffset();
    }
    private void showUIButtons(bool show)
    {
        restartButton.gameObject.SetActive(show);
        quitButton.gameObject.SetActive(show);
    }
    private void HighLight(bool boo, GameObject tankGameObject)
    {
        followTarget.Zoom(boo, tankGameObject);
        tankManager.Light(boo);
    }
    private bool IsSpawnPointChosen()
    {
        int count = 0;
        foreach (Tank tank in tanks)
        {
            TankSpawnMovement tsm = tank.spawnPoint.GetComponent<TankSpawnMovement>();
            if (!tsm.IsChosen())
            {
                count++;
            }
        }
        return (count == 0);
    }
    private void EnableSpawnPoint(bool state)
    {
        foreach (Tank tank in tanks)
        {
            TankSpawnMovement tsm = tank.spawnPoint.GetComponent<TankSpawnMovement>();
            tsm.gameObject.SetActive(state);
            if (!state)
            {
                tsm.Resetchosen();
            }
        }
    }
    private void SetUpItems()
    {
        foreach (Items item in items)
        {
            int which = Random.Range(1, 4); //1-3
            GameObject it = null;
            if (which == 1)
            {
                it = Instantiate(laserPrefab, item.spawnpoint.position, Quaternion.identity);
            }
            else if (which == 2)
            {
                it = Instantiate(wrenchPrefab, item.spawnpoint.position, Quaternion.identity);
            }
            else if(which == 3)
            {
                it = Instantiate(petrolPrefab, item.spawnpoint.position, Quaternion.identity);
            }
            item.item = it;
        }
    }
    private void RemoveItems()
    {
        foreach (Items item in items)
        {
            item.item.GetComponent<SelfDestory>().Off();
        }
    }
    private void ChangeGameMode()
    {
        int random = Random.Range(1, 5); //1-4
        if (random == 1)
        {
            tankManager.SetShellBounce(true);
            messageText.text = "Gamemode:\nShell Bounce!";
        }
        else if (random == 2)
        {
            tankManager.SetDamageRate(0.5f);
            messageText.text = "Gamemode:\nHalf Damage!";
        }
        else if (random == 3)
        {
            airRaid = true;
            messageText.text = "Gamemode:\nAir Raid!";
        }
        else if (random == 4)
        {
            tankManager.SetMustmove(true);
            messageText.text = "Gamemode:\nMove or Die!";
        }
    }
    private void ResetGameMode()
    {
        tankManager.SetMustmove(false);
        tankManager.SetShellBounce(false);
        tankManager.SetDamageRate(1f);
        airRaid = false;
    }
    private void AirRaid()
    {
        float x = Random.Range(-42.5f, 42.5f);
        float y = 60f;
        float z = Random.Range(-42.5f, 42.5f);
        Vector3 position = new Vector3(x, y, z);
        GameObject shell = Instantiate(shellPrefab,position, Quaternion.identity);
        shell.GetComponent<ShellExplosion>().explosionRadius = 4;
        shell.transform.localScale = new Vector3(2, 2, 2);
    }
}