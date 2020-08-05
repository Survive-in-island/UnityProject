using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLAnimal : MonoBehaviour
{
    [SerializeField]
    private int hp;

    // 사운드 
    AudioSource theAudio;
    [SerializeField]
    private AudioClip[] sound_pig_normal;
    [SerializeField]
    private AudioClip sound_pig_hurt;
    [SerializeField]
    private AudioClip sound_pig_dead;

    private bool isDead;

    [SerializeField]
    private GameObject go_meat_item_prefab; // 죽으면 나올 아이템
    [SerializeField]
    private GameObject pigParent;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
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

            PlaySE(sound_pig_hurt);
        }
    }

    private void Dead()
    {

        PlaySE(sound_pig_dead);

        ///
        // 수정해야될수도
        Destroy(pigParent, 2);
        
        Instantiate(go_meat_item_prefab, pigParent.transform.position, Quaternion.identity);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3);
        PlaySE(sound_pig_normal[_random]);
    }

}
