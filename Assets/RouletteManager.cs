using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : Singleton<RouletteManager>
{
    [SerializeField] private GameObject rouletteUI;     // ∑Í∑ø UI
    [SerializeField] private GameObject roulettePlate;  // µπ∏≤∆«
    [SerializeField] private Transform needle;          // »≠ªÏ«•

    [SerializeField] Sprite[] skillSprites;
    [SerializeField] Image[] displaySlots;

    private List<int> startList;
    private List<int> resultIndexList;

    int itemCount = 6;

    protected override void InitManager()
    {
        startList = new List<int>();
        resultIndexList = new List<int>();

        RouletteReset();
    }

    private void RouletteReset()
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

        if(closetDistance == -1)
        {
            Debug.Log(" something is wrong! ");
            return;
        }
        displaySlots[itemCount].sprite = displaySlots[closetIndex].sprite;
    }
}
