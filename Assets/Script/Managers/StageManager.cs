using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    START,
    NORMAL,
    ANGEL,
    BOSS
}

[System.Serializable]
public class StageLists
{
    public List<Room> rooms;
}

public class StageManager : Singleton<StageManager>
{
    [Header("Stage data")]
    public Room currentRoom;

    [Header("Room Datas")]
    [SerializeField] private Room startRoom;
    [SerializeField] private Room angleRoom;
    [SerializeField] private Room bossRoom;

    [SerializeField] private List<StageLists> stages;
    [SerializeField] private int stageIndex;            // ���� �������� ( 1, 2 )

    private int roomIndex;

    // 10 �������� ���� stages�� �ε����� �÷��� ���� ������� �ٲٱ�
    private int stageLEVEL => stageIndex / 10;

    // GET / SET
    public Room StartRoom => startRoom;
    public Room AngleRoom => angleRoom;
    public Room BossRoom => bossRoom;

    protected override void InitManager()
    {
        // ���������� 0���� �ʱ�ȭ
        stageIndex = 0;
    }

    public void NextStage()
    {
        if (!currentRoom.isClearROOM)
            return;

        // ���� ��ġ�� ���۹��϶�
        if(currentRoom.roomType == RoomType.START)
            stageIndex = 0;

        // ���۹��� �ƴҶ�
        else
        {
            // ���� ����������
            stageIndex++;

            // ���� �������� + 1�� 10��° ( Boss �� )
            if((stageIndex + 1) % 10 == 0 )
            {
                bossRoom.JoinPlayer(ref currentRoom);
                return;
            }
            // ���� �������� + 1 �� 5��° ( angel �� )
            else if((stageIndex + 1) % 5 == 0)
            {
                angleRoom.JoinPlayer(ref currentRoom);
                return;
            }

        }

        // ���� ���� �÷��̾� ������¸� false
        currentRoom.playerInROOM = false;

        // ���� ��ȣ�� ������ ����
        roomIndex = Random.Range(0, stages[stageLEVEL].rooms.Count);
        stages[stageLEVEL].rooms[roomIndex].JoinPlayer(ref currentRoom);
    }

    // 0 1 2 3 �Ϲ�       
    // 4 ����             
    // 5 6 7 8 �Ϲ�       
    // 9 ����             
}
