using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] public string animalName; // 동물의 이름
    [SerializeField] protected int hp; // 동물의 체력

    [SerializeField] protected float walkSpeed; // 걷기 스피드
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float turningSpeed;
    protected float applySpeed;

    [SerializeField]
    private GameObject go_meat_item_prefab; // 부서지면 나올 아이템

    protected Vector3 destination; // 방향

    // 상태변수
    protected bool isAction; // 행동중인지 아닌지 판별
    protected bool isWalking; // 걷는지 안 걷는지 판별
    protected bool isRunning;
    public bool isDead;
    protected bool isChasing; // 추격중인지 판별

    [SerializeField] protected float walkTime; // 걷기 시간
    [SerializeField] protected float waitTime; // 대기 시간
    [SerializeField] protected float runTime;
    protected float currentTime;

    // 필요한 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;

    //protected AudioSource theAudio;
    [SerializeField]
    protected AudioSource theAudio;
    [SerializeField] protected AudioClip[] sound_normal;
    [SerializeField] protected AudioClip sound_hurt;
    [SerializeField] protected AudioClip sound_dead;
    protected NavMeshAgent nav;
    protected FieldOfViewAngle theViewAngle;


    [SerializeField]
    private Item item_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        theViewAngle = GetComponent<FieldOfViewAngle>();
        //theAudio.GetComponent<AudioSource>();
        //nav = GetComponent<NavMeshAgent>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    protected virtual void Update()
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
            //nav.SetDestination(transform.position + destination * 5f);
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            turningSpeed = Random.Range(0.01f, 0.05f);
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, destination.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }


    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing)
                ResetAnim(); // 다음 랜덤 행동 개시
        }
    }

    protected virtual void ResetAnim()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        applySpeed = walkSpeed;
        //nav.speed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        //nav.ResetPath();
        destination.Set(0f, Random.Range(0f, 360f), 0f);
        //destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        //nav.speed = walkSpeed;

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

            PlaySE(sound_hurt);

            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(sound_dead);
        isWalking = false;
        isRunning = false;
        isDead = true;

        this.gameObject.tag = "Untagged";

        ////////////////////////////
        Instantiate(go_meat_item_prefab, this.transform.position, Quaternion.identity);
        ////////////////

        anim.SetTrigger("Dead");
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3);
        PlaySE(sound_normal[_random]);
    }

    public Item GetItem()
    {
        Destroy(this.gameObject, 3f);
        return item_Prefab;
    }

}
