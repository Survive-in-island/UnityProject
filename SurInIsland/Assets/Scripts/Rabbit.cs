using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    [SerializeField]
    private string animalName;  // 동물 이름
    [SerializeField]
    private int hp; // 동물의 체력
    [SerializeField]
    private float runSpeed; // 뛰는 속도

    // 상태변수
    private bool isAction; // 행동중인지 아닌지 판별
    private bool isRunning; // 뛰는지 안뛰는지 판별

    [SerializeField]
    private float runTime; // 뛰기 시간
    [SerializeField]
    private float waitTime; // 대기 시간
    private float currentTime;

    //private Vector3 direction; // 방향

    // 필요한 컴포넌트
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime();
    }

    private void ElapsedTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                ResetRabbitAction();
            }
        }
    }


    //public void RabbirRun(Vector3 _targetPos)
    //{
    //    direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

    //    currentTime = runTime;
    //    isRunning = true;
    //    anim.SetBool("isRun", isRunning);
    //}

    private void ResetRabbitAction()
    {
        isRunning = false;
        isAction = true;

        anim.SetBool("isRun", isRunning);

        RandomAction();
    }

    private void RandomAction()
    {

        int _random = Random.Range(0, 2);       // 대기, 뛰기

        if (_random == 0)
            Wait();
        else if(_random == 1)
            Run();
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }

    private void Run()          // 나중에 피격당했을때로 바꿀 것.
    {
        isRunning = true;
        anim.SetBool("isRun", isRunning);
        //Debug.Log("뛰기");
        currentTime = waitTime;
    }
}
