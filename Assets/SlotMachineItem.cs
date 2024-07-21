using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum SpinOrder
{
    First = 20,
    Second = 28,
    Third = 36
}

public class SlotMachineItem : MonoBehaviour
{
    [SerializeField] private SpinOrder spinOrder;       
    [SerializeField] private int skillIndex;            // 해당 슬롯에 당첨된 스킬 Index / 추후 스킬 적용에 사용

    [SerializeField] private List<Image> slotSprite;    // 스킬 이미지

    [SerializeField] private Button choiceButton;       // 스킬 선택 버튼
    [SerializeField] private Transform slotTransform;   // 돌아가는 오브젝트 

    private List<int> spriteIndexs;
    private Vector3 defalutPosition;

    private void Start()
    {
        defalutPosition = slotTransform.localPosition;   
    }

    public void InitializeSlot(int _skillIndex, ref Sprite[] skillSprites)
    {
        spriteIndexs = new List<int>();

        choiceButton.interactable = false;
        choiceButton.onClick.RemoveAllListeners();

        slotSprite[0].sprite = skillSprites[_skillIndex];
        spriteIndexs.Add(_skillIndex);

        for(int i = 1; i < slotSprite.Count-1; i++)
        {
            int randomIndex = Random.Range(0, skillSprites.Length);
            slotSprite[i].sprite = skillSprites[randomIndex];

            spriteIndexs.Add(randomIndex);
        }

        slotSprite[slotSprite.Count - 1].sprite = skillSprites[_skillIndex];
        spriteIndexs.Add(_skillIndex);
    }

    public IEnumerator Spin()
    {
        // 스핀
        for(int i = 0; i < (int)spinOrder * 6; i++)
        {
            slotTransform.localPosition -= new Vector3(0, 50f, 0);
            if (slotTransform.localPosition.y < 50f)
                slotTransform.transform.localPosition = defalutPosition;

            yield return new WaitForSeconds(0.02f);
        }

        // 버튼 활성화
        choiceButton.interactable = true;

        IndexBySpinOrder();
        choiceButton.onClick.AddListener(() => RouletteManager.Instance.ChoiceSlotSkill(skillIndex));
    }

    public void IndexBySpinOrder()
    {
        switch(spinOrder)
        {
            case SpinOrder.First:
                skillIndex = spriteIndexs[1];
                break;
            case SpinOrder.Second:
                skillIndex = spriteIndexs[2];
                break;
            case SpinOrder.Third:
                skillIndex = spriteIndexs[3];
                break;
        }
    }
}
