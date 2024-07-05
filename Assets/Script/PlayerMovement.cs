using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimatorState
{
    IDLE,
    WALK,
    DAMEGE,
    ATTACK
}

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("playerMovement");
                    instance = instanceContainer.AddComponent<PlayerMovement>();
                }
            }
            return instance;
        }
    }
    private static PlayerMovement instance;

    private Rigidbody playerRB;
    [SerializeField, Range(0, 50)] private float moveSpeed;

    public Animator playerAnimator;

    private PlayerAnimatorState playerState;
    string[] stateStrings;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        playerState = PlayerAnimatorState.IDLE;
        stateStrings = new string[4];
        stateStrings[(int)PlayerAnimatorState.IDLE] = "IDLE";
        stateStrings[(int)PlayerAnimatorState.WALK] = "WALK";
        stateStrings[(int)PlayerAnimatorState.DAMEGE] = "DAMEGE";
        stateStrings[(int)PlayerAnimatorState.ATTACK] = "ATTACK";
    }

    private void FixedUpdate()
    {
        if (JoyStickMovement.Instance.joyVec.x != 0 || JoyStickMovement.Instance.joyVec.y != 0)
        {
            playerRB.velocity = new Vector3(JoyStickMovement.Instance.joyVec.x * moveSpeed, playerRB.velocity.y, JoyStickMovement.Instance.joyVec.y * moveSpeed);

            playerRB.rotation = Quaternion.LookRotation(new Vector3(
                JoyStickMovement.Instance.joyVec.x ,0 ,JoyStickMovement.Instance.joyVec.y));
        }
    }

    public void IdlePlayer()
    {
        playerRB.velocity = Vector3.zero;
        ChangeState(PlayerAnimatorState.IDLE);
        // setTrigger Idle
    }

    public void WalkPlayer()
    {
        playerAnimator.SetTrigger("WALK");
        ChangeState(PlayerAnimatorState.WALK);
    }

    public void ChangeState(PlayerAnimatorState state)
    {
        playerAnimator.SetBool("IDLE", false);
        playerAnimator.SetBool("WALK", false);
        playerAnimator.SetBool("ATTACK", false);

        playerState = state;
        switch(state)
        {
            case PlayerAnimatorState.IDLE:
                playerAnimator.SetBool("IDLE", true);
                break;

            case PlayerAnimatorState.WALK:
                playerAnimator.SetBool("WALK", true);
                break;
            case PlayerAnimatorState.ATTACK:
                playerAnimator.SetBool("ATTACK", true);
                break;
        }
    }
}
