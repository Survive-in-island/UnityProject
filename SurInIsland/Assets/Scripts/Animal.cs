using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; // 동물의 이름
    [SerializeField] protected int hp; // 동물의 체력

    [SerializeField] protected float walkSpeed; // 걷기 스피드
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float turningSpeed;
    protected float applySpeed;

    protected Vector3 direction; // 방향

    // 상태변수
    protected bool isAction; // 행동중인지 아닌지 판별
    protected bool isWalking; // 걷는지 안 걷는지 판별
    protected bool isRunning;
    protected bool isDead;

    [SerializeField] protected float walkTime; // 걷기 시간
    [SerializeField] protected float waitTime; // 대기 시간
    [SerializeField] protected float runTime;
    protected float currentTime;

    // 필요한 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    //protected NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        //theAudio.GetComponent<AudioSource>();
        //nav = GetComponent<NavMeshAgent>();
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
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            turningSpeed = Random.Range(0.01f, 0.05f);
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }


    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                ResetAnim(); // 다음 랜덤 행동 개시
        }
    }

    protected virtual void ResetAnim()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);

        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("걷기");
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
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

            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        //PlaySE(sound_pig_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;

        this.gameObject.tag = "Untagged";

        //Instantiate(go_meat_row_item_prefab, )
        anim.SetTrigger("Dead");
    }
}
