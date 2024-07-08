using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> monsterListInROOM;    // 해당 방의 몬스터 리스트

    [SerializeField] private bool playerInROOM;      // 플레이어 입장 여부
    [SerializeField] private bool isClearROOM;       // 클리어 여부


    void Start()
    {
        //monsterListInROOM = new List<GameObject>();

        playerInROOM = true;
        isClearROOM = false;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerManager.Instance.PlayerTargeting.CurrentRoomData = this;
        }
    }
}
