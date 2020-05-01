using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Rabbit : MonoBehaviour
{
    [SerializeField]
    private string animalName;  // 동물 이름
    [SerializeField]
    private int hp; // 동물의 체력

    [SerializeField]
    private float runSpeed; // 뛰는 속도
    [SerializeField] 
    private float walkSpeed; // 걷기 스피드
    private float applySpeed;

    // 상태변수
    private bool isAction; // 행동중인지 아닌지 판별
    private bool isWalking;
    private bool isRunning; // 뛰는지 안뛰는지 판별
    private bool isDead = false;


    [SerializeField]
    private float runTime; // 뛰기 시간
    [SerializeField]
    private float waitTime; // 대기 시간
    private float currentTime;

    private Vector3 direction; // 방향

    // 필요한 컴포넌트
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;

    protected NavMeshAgent nav;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapsedTime();
        }
    }

    private void Move()
    {
        if (isRunning || isWalking)
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }

    private void Rotation()
    {
        if (isRunning || isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
        //Debug.Log("아니 이게 왜 안나오는거야");
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


    private void ResetRabbitAction()
    {
        isRunning = false;
        isAction = true;
        isWalking = false;

        applySpeed = walkSpeed;

        anim.SetBool("isRun", isRunning);

        direction.Set(0f, Random.Range(0f, 360f), 0f);

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

    private void Run()        
    {
        isRunning = true;
        anim.SetBool("isRun", isRunning);
        currentTime = runTime;
        applySpeed = walkSpeed;     // 이거땜에 오류 생기기도
    }

    private void HurtRun(Vector3 _targetPos)          
    {
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;          // 이거땜에 오류 생기기도
        applySpeed = runSpeed;
        isWalking = false;
        isRunning = true;
        anim.SetBool("isRun", isRunning);
        //Debug.Log("뛰기");
        //currentTime = waitTime;
    }

    public void Damage(int _dmg, Vector3 _targetPos)
    {

        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            //PlaySE(sound_pig_Hurt);
            //anim.SetTrigger("isHurt");
            HurtRun(_targetPos);
        }
    }

    private void Dead()
    {
        //PlaySE(sound_pig_Dead);
        isRunning = false;
        isDead = true;

        this.gameObject.tag = "Untagged";

        anim.SetTrigger("isDead");
    }
}
