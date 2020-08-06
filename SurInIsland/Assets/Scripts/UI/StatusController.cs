using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTreeFPS;

public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;
    private int currentHp;

    // 스테미너
    [SerializeField]
    private int sp;
    private int currentSp;

    // 스테미너 증가량
    [SerializeField]
    private int spIncreaseSpeed;
    // HP증가량
    [SerializeField]
    private int hpIncreaseSpeed;


    // 스테미너 회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    // HP 회복 딜레이
    [SerializeField]
    private int hpRechargeTime;
    private int currentHpRechargeTime;

    // 스테미너 감소 여부
    private bool spUsed;
    private bool hpUsed;

    // 방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // 배고픔이 줄어드는 속도
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;


    // 목마름이 줄어드는 속도
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족감이 줄어드는 속도
    [SerializeField]
    private int satisfyDecreaseTime;
    private int currentSatisfyDecreaseTime;

    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
        currentHungry = hungry;
        currentSatisfy = satisfy;
        currentThirsty = thirsty;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        Satisfy();

        SPRechargeTime();
        SPRecover();
        HPRechargeTime();
        HPRecover();

        GaugeUpdate();
    }

    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void HPRechargeTime()
    {
        if (hpUsed)
        {
            if (currentHpRechargeTime < hpRechargeTime)
                currentHpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void HPRecover()
    {
        if (hungry > 70)
        {
            currentHp += hpIncreaseSpeed;
        }
    }

    private void SPRecover()
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다.");
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다.");
    }

    private void Satisfy()
    {
        if (currentSatisfy > 0)
        {
            if (currentSatisfyDecreaseTime <= satisfyDecreaseTime)
                currentSatisfyDecreaseTime++;
            else
            {
                currentSatisfy--;
                currentSatisfyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("만족감 수치가 0이 되었습니다.");
    }
    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentDp / dp;
        images_Gauge[DP].fillAmount = (float)currentSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
        {
            currentHp += _count;
            currentHp += _count;
        }

        else
            currentHp = hp;
    }

    public void DecreaseHP(int _count)
    {
        if(currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("캐릭터 피가 0이 되었습니다.");
    }

    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("캐릭터 방어력이 0이 되었습니다.");
    }

    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry = -_count;
    }

    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = -0;
        else
            currentThirsty -= _count;
    }


    public void IncreaseSatisfy(int _count)
    {
        if (currentSatisfy + _count < satisfy)
            currentSatisfy += _count;
        else
            currentSatisfy = satisfy;
    }

    public void DecreaseSatisfy(int _count)
    {
        if (currentSatisfy - _count < 0)
            currentSatisfy = -0;
        else
            currentSatisfy -= _count;
    }

    public void DecreaseStamina(int _coont)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _coont > 0)
        {
            currentSp -= _coont;
        }
        else
            currentSp = 0;
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
