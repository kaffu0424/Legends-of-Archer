using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void InitManager()
    {
        // �÷��̾ ���۹����� �̵� �� �ʱ�ȭ
        StageManager.Instance.StartRoom.JoinPlayer(ref StageManager.Instance.currentRoom);
    }
}
