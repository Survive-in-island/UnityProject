using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected float ChaseTime;              // 총 추격 시간
    protected float currentChaseTime;       // 계산
    [SerializeField]
    protected float ChaseDelayTime;         // 추격 딜레이

    public void Chase(Vector3 _targetPos)
    {
        isChasing = true;

        destination = _targetPos;     // 목적지
        applySpeed = runSpeed;
        //nav.speed = runSpeed;
        //nav.SetDestination(destination);
        
        //currentTime = runTime;
        //isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);

        destination = new Vector3(_targetPos.x, 0f, _targetPos.z).normalized; // 맞는지 확인 필요 
    }

    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);

        if (!isDead)
            Chase(_targetPos);
    }
}
