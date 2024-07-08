using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> monsterListInROOM;    // �ش� ���� ���� ����Ʈ

    [SerializeField] private bool playerInROOM;      // �÷��̾� ���� ����
    [SerializeField] private bool isClearROOM;       // Ŭ���� ����


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
