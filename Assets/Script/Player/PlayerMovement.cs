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
    // Move
    private Rigidbody playerRB;
    [SerializeField, Range(0, 50)] private float moveSpeed;

    // Animator
    private Animator playerAnimator;

    // Animation State
    private PlayerAnimatorState playerState;
    private string[] stateStrings;

    // JoyStick
    private JoyStickMovement joystick;

    // GET/SET
    public Animator PlayerAnimator => playerAnimator;
    public Rigidbody PlayerRB => playerRB;

    public void InitializePlayerMovement()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();

        playerState = PlayerAnimatorState.IDLE;
        stateStrings = new string[4];
        stateStrings[(int)PlayerAnimatorState.IDLE] = "IDLE";
        stateStrings[(int)PlayerAnimatorState.WALK] = "WALK";
        stateStrings[(int)PlayerAnimatorState.DAMEGE] = "DAMEGE";
        stateStrings[(int)PlayerAnimatorState.ATTACK] = "ATTACK";

        joystick = PlayerManager.Instance.JoyStickMovement;
    }

    private void Update()
    {
        if (joystick.IsMoveing)
        {
            playerRB.velocity = new Vector3(joystick.JoyVec.x, 0, joystick.JoyVec.y) * moveSpeed;

            playerRB.rotation = Quaternion.LookRotation(new Vector3(
                joystick.JoyVec.x, 0, joystick.JoyVec.y));
        }
    }

    #region Animation
    public void IdlePlayerAnimation()
    {
        playerRB.velocity = Vector3.zero;
        ChangeAnimationState(PlayerAnimatorState.IDLE);
    }

    public void WalkPlayerAnimation()
    {
        ChangeAnimationState(PlayerAnimatorState.WALK);
    }

    public void ChangeAnimationState(PlayerAnimatorState state)
    {
        PlayerAnimator.SetBool("IDLE", false);
        PlayerAnimator.SetBool("WALK", false);
        PlayerAnimator.SetBool("ATTACK", false);

        playerState = state;

        PlayerAnimator.SetBool(stateStrings[(int)state], true);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("NextStage"))
        {
            StageManager.Instance.NextStage();
        }
    }
}
