using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : Singleton<RouletteManager>
{
    [Header("public")]
    [SerializeField] Sprite[] skillSprites;

    [Header("Roulette")]
    [SerializeField] private GameObject rouletteUI;     // 룰렛 UI
    [SerializeField] private GameObject roulettePlate;  // 돌림판
    [SerializeField] private Transform needle;          // 화살표

    [SerializeField] Image[] displaySlots;

    private List<int> startList;
    private List<int> resultIndexList;

    int itemCount = 6;

    [Header("SlotMachine")]
    [SerializeField] private GameObject slotMachineUI;      // 슬롯머신 UI
    [SerializeField] private SlotMachineItem[] slotItems;
    protected override void InitManager()
    {
        startList = new List<int>();
        resultIndexList = new List<int>();

        // RouletteSpin();
        // StartCoroutine(SlotMachineSpin());
    }

    #region 룰렛
    private void RouletteSpin()
    {
        for(int i = 0; i < itemCount; i++) 
        { 
            startList.Add(i);
        }

        for(int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, startList.Count);

            resultIndexList.Add(startList[randomIndex]);

            displaySlots[i].sprite = skillSprites[startList[randomIndex]];

            startList.RemoveAt(randomIndex);
        }

        StartCoroutine(StartRoulette());
    }

    IEnumerator StartRoulette()
    {
        yield return new WaitForSeconds(2f);
        float randomSpd = Random.Range(1.0f, 5.0f);
        float rotateSpeed = 100f * randomSpd;

        while(true)
        {
            yield return null;

            if (rotateSpeed <= 0.01f)
                break;

            rotateSpeed = Mathf.Lerp(rotateSpeed, 0, Time.deltaTime * 2f);
            roulettePlate.transform.Rotate(0, 0, rotateSpeed);

        }
        yield return new WaitForSeconds(1f);
        Result();

        yield return new WaitForSeconds(1f);
        rouletteUI.SetActive(false);
    }

    private void Result()
    {
        int closetIndex = -1;
        float closetDistance = 500f;
        float currentDistance = 0f;

        for(int i = 0; i <= itemCount; i++)
        {
            currentDistance = Vector2.Distance(displaySlots[i].transform.position, needle.position);
            
            if(closetDistance > currentDistance)
            {
                closetDistance = currentDistance;
                closetIndex = i;
            }
        }

        // 예외처리
        if(closetDistance == -1)
            return;

        displaySlots[itemCount].sprite = displaySlots[closetIndex].sprite;
    }
    #endregion

    #region 슬롯머신
    public IEnumerator SlotMachineSpin()
    {

        // 슬롯머신 활성화
        slotMachineUI.SetActive(true);  

        // 슬롯 이미지 초기화
        foreach(SlotMachineItem item in slotItems)
        {
            int spriteIndex = Random.Range(0, skillSprites.Length);
            item.InitializeSlot(spriteIndex, ref skillSprites);
        }
        yield return new WaitForSeconds(0.5f);

        // 돌리기
        foreach(SlotMachineItem item in slotItems)
        {
            StartCoroutine(item.Spin());
        }
    }

    public void ChoiceSlotSkill(int skillIndex)
    {
        Debug.Log(skillIndex + " 번 스킬이 선택됨");
    }
    #endregion
}
