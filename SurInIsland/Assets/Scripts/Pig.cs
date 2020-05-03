﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    protected override void Update()
    {
        base.Update();

        if (theViewAngle.View() && !isDead)
        {
            Run(theViewAngle.GetTargetPos());
        }
    }

    protected override void ResetAnim()
    {
        base.ResetAnim();
        RandomAction();
    }

    // Start is called before the first frame update


    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4); // 대기,풀뜯기, 두리번, 걷기

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();

    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");

        Debug.Log("두리번");
    }

}
