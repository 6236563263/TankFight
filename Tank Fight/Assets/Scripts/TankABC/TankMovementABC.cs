using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovementABC : MonoBehaviour //加ABC为了区分资源提供的移动脚本
{
    public float speed = 8f;
    public float angularSpeed = 30f;
    public int playerNumber = 1;

    private float moveValue;
    private float turnValue;

    public Rigidbody rb;
    public AudioClip idleClip;
    public AudioClip movingClip;
    public AudioSource engineAudioSource;

    void Update()
    {
        moveValue = Input.GetAxis("Vertical" + playerNumber);
        turnValue = Input.GetAxis("Horizontal" + playerNumber);
    }
    void FixedUpdate()
    {
        move();
        turn();
        engineAudio();
    }
    private void engineAudio()
    {
        if(!Mathf.Approximately(moveValue,0)||!Mathf.Approximately(turnValue,0))
        {
            if(engineAudioSource.clip == idleClip)
            {
                engineAudioSource.clip = movingClip;
                engineAudioSource.Play();
            }
            else if (engineAudioSource.clip == movingClip)
            {
                engineAudioSource.clip = idleClip;
                engineAudioSource.Play();
            }
        }
    }
    private void move()
    {
        /*Vector3 position = transform.forward * moveValue * speed * Time.deltaTime;
        rb.MovePosition(rb.position + position);*/
        rb.velocity = transform.forward * moveValue * speed;
    }
    private void turn()
    { 
        /*float turn = turnValue * angularSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, turn, 0);
        rb.MoveRotation(rb.rotation * rotation);*/
        rb.angularVelocity = transform.up * turnValue * angularSpeed;
    }

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }
    public void ResetMovement()
    {
        rb.velocity = new Vector3(0,0,0);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }
    public int GetPlayerNumber()
    {
        return this.playerNumber;
    }
}
