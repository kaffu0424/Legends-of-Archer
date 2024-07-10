using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> monsterListInROOM;              // �ش� ���� ���� ����Ʈ
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private Transform playerSpawnPoint;    // �÷��̾� ����� ���� ��ġ
    [SerializeField] private Transform enemyParent;

    [Header("Room State")]
    public bool playerInROOM;     // �÷��̾� ���� ����
    public bool isClearROOM;      // Ŭ���� ����
    public RoomType roomType;     // �� Ÿ��

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
        // ��Ŭ���� ���·� ��ȯ
        isClearROOM = false;

        // ���� ����
        SpawnEnemy();

        // �÷��̾� �̵� �� �ʱ�ȭ
        PlayerManager.Instance.playerTransform.position = playerSpawnPoint.position;
        PlayerManager.Instance.PlayerTargeting.CurrentRoomData = this;
        currentRoom = this;

        // �÷��̾� ���� ���·� ��ȯ
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
