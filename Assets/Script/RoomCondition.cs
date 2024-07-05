using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    [SerializeField] private List<GameObject> MonsterListInROOM;

    public bool playerInROOM;
    public bool isClearRoom;
    void Start()
    {
        MonsterListInROOM = new List<GameObject>();

        playerInROOM = false;
        isClearRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInROOM)
        {
            if(MonsterListInROOM.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInROOM = true;

            PlayerTargeting.Instance.MonsterListInROOM = new List<GameObject>(MonsterListInROOM);
        }

        if(other.CompareTag("Enemy"))
        {
            MonsterListInROOM.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInROOM = false;
        }

        if(other.CompareTag("Enemy"))
        {
            MonsterListInROOM.Remove(other.gameObject);
        }
    }
}
