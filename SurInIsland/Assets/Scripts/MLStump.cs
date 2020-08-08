using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLStump : MonoBehaviour
{
    [SerializeField]
    private int hp;

    private bool isDead;

    [SerializeField]
    private GameObject go_log_item_prefab; // 죽으면 나올 아이템
    [SerializeField]
    private GameObject stumpParent;

    // Start is called before the first frame update
    void Start()
    {
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
                isDead = true;
                Dead();
                return;
            }

        }
    }

    private void Dead()
    {
        Instantiate(go_log_item_prefab, transform.position, Quaternion.identity);

        Destroy(stumpParent, 2);

    }


}
