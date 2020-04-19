using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    [SerializeField] private string animalName; // 동물의 이름
    [SerializeField] private int hp; // 동물의 체력

    [SerializeField] private float walkSpeed; // 걷기 스피드
    [SerializeField] private float runSpeed; // 뛰는 스피드

    private Vector3 direction; // 방향

    // 상태변수
    private bool isAction; // 행동중인지 아닌지 판별
    private bool isWalking; // 걷는지 안 걷는지 판별
    private bool isRunning; // 뛰는지 판별
    private bool isHit; // 맞았는지 판별

    [SerializeField] private float walkTime; // 걷기 시간
    [SerializeField] private float waitTime; // 대기 시간
    [SerializeField] private float runTime; // 뛰는 시간
    private float currentTime;

    // 필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }


    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                ResetAnim(); // 다음 랜덤 행동 개시
        }
    }

    private void ResetAnim()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;

        //applySpeed = walkSpeed;

        anim.SetBool("isWalk", isWalking);
        anim.SetBool("isRun", isRunning);

        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        int _random = Random.Range(0, 3); // 헹동 수정하기

        if (_random == 0)
            Wait();
        else if (_random == 1)
            //Run();
            TryWalk();
        else if (_random == 2)
            TryWalk();

    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }


    private void Run(Vector3 _targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        currentTime = waitTime;
        anim.SetBool("isRun", isRunning);

        Debug.Log("뛰기");
    }

    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("isWalk", isWalking);
        currentTime = walkTime;
        Debug.Log("걷기");
    }

    public void Tiger_Damage(int _dmg, Vector3 _targetPos)
    {

        if (!isHit)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                //Dead();
                return;
            }

            //PlaySE(sound_pig_Hurt);
            anim.SetTrigger("isHit");
            Run(_targetPos);
        }
    }
}
