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
    [SerializeField] private int stageIndex;            // 현재 스테이지 ( 1, 2 )

    private int roomIndex;

    // 10 스테이지 마다 stages의 인덱스를 올려서 다음 배경으로 바꾸기
    private int stageLEVEL => stageIndex / 10;

    // GET / SET
    public Room StartRoom => startRoom;
    public Room AngleRoom => angleRoom;
    public Room BossRoom => bossRoom;

    protected override void InitManager()
    {
        // 스테이지를 0으로 초기화
        stageIndex = 0;
    }

    public void NextStage()
    {
        if (!currentRoom.isClearROOM)
            return;

        // 현재 위치가 시작방일때
        if(currentRoom.roomType == RoomType.START)
            stageIndex = 0;

        // 시작방이 아닐때
        else
        {
            // 다음 스테이지로
            stageIndex++;

            // 다음 스테이지 + 1가 10번째 ( Boss 방 )
            if((stageIndex + 1) % 10 == 0 )
            {
                bossRoom.JoinPlayer(ref currentRoom);
                return;
            }
            // 다음 스테이지 + 1 이 5번째 ( angel 방 )
            else if((stageIndex + 1) % 5 == 0)
            {
                angleRoom.JoinPlayer(ref currentRoom);
                return;
            }

        }

        // 기존 방의 플레이어 입장상태를 false
        currentRoom.playerInROOM = false;

        // 랜덤 번호의 방으로 입장
        roomIndex = Random.Range(0, stages[stageLEVEL].rooms.Count);
        stages[stageLEVEL].rooms[roomIndex].JoinPlayer(ref currentRoom);
    }

    // 0 1 2 3 일반       
    // 4 엔젤             
    // 5 6 7 8 일반       
    // 9 보스             
}
