using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class Dog : MonoBehaviour
{
    [SerializeField]
    private int hp;

    private bool isDead = false;
    private bool isAttack = false;

    private Animator anim;

    [SerializeField]
    private GameObject go_meat_item_prefab; // 죽으면 나올 아이템
    [SerializeField]
    private GameObject dogParent;

    //[SerializeField]
    public StatusController playerStat;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerStat = GetComponent<StatusController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(int _dmg)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                //isDead = true;

                Dead();
                return;
            }

            anim.SetTrigger("Hurt");

        }
    }

    private void Dead()
    {

        ///
        // 수정해야될수도
        isDead = true;

        //anim.SetBool("Dead", isDead);
        anim.SetTrigger("DeadTri");

        Destroy(dogParent, 2);

        Instantiate(go_meat_item_prefab, this.transform.position, Quaternion.identity);

    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Pig") || (collision.gameObject.CompareTag("Rabbit"))))             // truffle에서 item으로 수정
        {
            // attack

            isAttack = true;
            anim.SetBool("Attack", isAttack);

            if (collision.gameObject.tag == "Player")
            {
                collision.transform.GetComponent<StatusController>().DecreaseHP(1);
                //playerStat.DecreaseHP(10);
            }

            //anim.SetTrigger("Hit");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isAttack = false;
    }


}