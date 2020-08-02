using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField]
    private string animalName;
    [SerializeField]
    private int hp;

    [SerializeField]
    private float walkSpeed;

    // 상태변수
    private bool isWalking;
    [SerializeField]
    private float walkTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
