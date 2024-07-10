using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFunction : MonoBehaviour
{
    PlayerTargeting playerTargeting;

    private void Start()
    {
        playerTargeting = PlayerManager.Instance.PlayerTargeting;
    }

    public void Attack()
    {
        playerTargeting.Attack();
    }
}
