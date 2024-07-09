using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void InitManager()
    {
        // 플레이어를 시작방으로 이동 및 초기화
        StageManager.Instance.StartRoom.JoinPlayer(ref StageManager.Instance.currentRoom);
    }
}
