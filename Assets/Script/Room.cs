using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> monsterListInROOM;              // 해당 방의 몬스터 리스트
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private Transform playerSpawnPoint;    // 플레이어 입장시 스폰 위치
    [SerializeField] private Transform enemyParent;

    [Header("Room State")]
    public bool playerInROOM;     // 플레이어 입장 여부
    public bool isClearROOM;      // 클리어 여부
    public RoomType roomType;     // 방 타입

    private void Update()
    {
        CheckRoomState();
    }
    private void CheckRoomState()
    {
        // 플레이어 방에 있을때만
        if (playerInROOM)
        {
            // 클리어 되지않았을때
            if (!isClearROOM)
            {
                // 현재 방에 남은 몬스터가 없을때
                if (monsterListInROOM.Count == 0)
                {
                    // 방을 클리어 상태로 전환
                    isClearROOM = true;
                }
            }
        }
    }
    public void JoinPlayer(ref Room currentRoom)
    {
        // 미클리어 상태로 전환
        isClearROOM = false;

        // 몬스터 생성
        SpawnEnemy();

        // 플레이어 이동 및 초기화
        PlayerManager.Instance.playerTransform.position = playerSpawnPoint.position;
        PlayerManager.Instance.PlayerTargeting.CurrentRoomData = this;
        currentRoom = this;

        // 플레이어 입장 상태로 전환
        playerInROOM = true;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        monsterListInROOM.Remove(enemy);
    }

    public void SpawnEnemy()
    {
        foreach(Transform spawnPoint in  enemySpawnPoints)
        {
            GameObject enemy = Instantiate(EnemyManager.Instance.GetEnemy(EnemyName.Duck), enemyParent);
            enemy.transform.position = spawnPoint.position;

            monsterListInROOM.Add(enemy);
            enemy.GetComponent<Enemy>().currentRoom = this;
        }

    }
}
