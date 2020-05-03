using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigWild : StrongAnimal
{
    protected override void Update()
    {
        base.Update();

        if (theViewAngle.View() && !isDead)
        {
            //Chase(theViewAngle.GetTargetPos());
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
            Debug.Log(theViewAngle.GetTargetPos());
        }
    }

    IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while (currentChaseTime < ChaseTime)
        {
            Chase(theViewAngle.GetTargetPos());
            yield return new WaitForSeconds(ChaseDelayTime);
            currentChaseTime += ChaseDelayTime;
        }

        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);

        // nav.ResetPath();
        destination = theViewAngle.GetTargetPos();
    }
}
