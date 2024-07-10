using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> monsterListInROOM;              // �ش� ���� ���� ����Ʈ
    [SerializeField] private Transform playerSpawnPoint;    // �÷��̾� ����� ���� ��ġ
    [SerializeField] private Transform enemyParent;

    [Header("Room State")]
    public bool playerInROOM;     // �÷��̾� ���� ����
    public bool isClearROOM;      // Ŭ���� ����
    public RoomType roomType;     // �� Ÿ��

    private void Start()
    {
        
    }

    private void Update()
    {
        CheckRoomState();
    }
    private void CheckRoomState()
    {
        // �÷��̾� �濡 ��������
        if (playerInROOM)
        {
            // Ŭ���� �����ʾ�����
            if (!isClearROOM)
            {
                // ���� �濡 ���� ���Ͱ� ������
                if (monsterListInROOM.Count == 0)
                {
                    // ���� Ŭ���� ���·� ��ȯ
                    isClearROOM = true;
                }
            }
        }
    }
    public void JoinPlayer(ref Room currentRoom)
    {
        PlayerManager.Instance.playerTransform.position = playerSpawnPoint.position;
        PlayerManager.Instance.PlayerTargeting.CurrentRoomData = this;
        currentRoom = this;

        playerInROOM = true;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        monsterListInROOM.Remove(enemy);
    }

    public void SpawnEnemy()
    {

    }
}
