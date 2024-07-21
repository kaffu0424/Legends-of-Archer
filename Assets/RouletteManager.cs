using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : Singleton<RouletteManager>
{
    public bool onRoulette;

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
        onRoulette = false;

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
        Time.timeScale = 0;
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
    public void SlotMachineSpin()
    {
        onRoulette = true;
        StartCoroutine(MachineSpinCoroutine());
    }

    public IEnumerator MachineSpinCoroutine()
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
        // 0. 공격력 증가1
        // 1. 이속 증가1
        // 2. 최대체력 증가1
        // 3. 공속 증가1
        // 4. 이속증가?1
        // 5. 사선 추가
        // 6. 공속 증가
        // 7. 공속 증가
        // 8. 직선 추가
        // 9. 몹 바운스
        // 10. 추가 공격
        // 11. 벽 바운스

        onRoulette = false;
        Debug.Log(skillIndex + " 번 스킬이 선택됨");

        switch(skillIndex)
        {
            case 0:
                PlayerManager.Instance.PlayerStat.GetDamageBoost(1.2f);
                break;
            case 1:
                PlayerManager.Instance.PlayerStat.GetMoveSpeed(1f);
                break;
            case 2:
                PlayerManager.Instance.GetHPBoost();
                break;
            case 3:
                PlayerManager.Instance.PlayerStat.MultiplyAtkSpeed(1.1f);
                break;
            case 4:
                PlayerManager.Instance.PlayerStat.GetMoveSpeed(1f);
                break;
            case 5:
                PlayerManager.Instance.PlayerStat.skills[(int)SkillName.AddDiagonal]++;
                break;
            case 6:
                PlayerManager.Instance.PlayerStat.MultiplyAtkSpeed(1.11f);
                break;
            case 7:
                PlayerManager.Instance.PlayerStat.MultiplyAtkSpeed(1.12f);
                break;
            case 8:
                PlayerManager.Instance.PlayerStat.skills[(int)SkillName.AddStraight]++;
                break;
            case 9:
                PlayerManager.Instance.PlayerStat.skills[(int)SkillName.EnemyBounce]++;
                break;
            case 10:
                PlayerManager.Instance.PlayerStat.skills[(int)SkillName.MultiAttack]++;
                break;
            case 11:
                PlayerManager.Instance.PlayerStat.skills[(int)SkillName.Bounce]++;
                break;
        }

        slotMachineUI.SetActive(false);
    }
    #endregion
}
