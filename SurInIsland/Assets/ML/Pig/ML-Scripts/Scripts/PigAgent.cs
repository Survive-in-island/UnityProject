﻿using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAgent : Agent
{
    [Header("Pig Agent Settings")]
    public float moveSpeed = 1f;
    public float rotateSpeed = 2f;
    public float nostrilWidth = .5f;

    private PigAcademy agentAcademy;
    private PigArea agentArea;
    private Rigidbody agentRigidbody;
    private RayPerception rayPerception;

    private int trufflesCollected = 0;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentAcademy = FindObjectOfType<PigAcademy>();
        agentArea = transform.parent.GetComponent<PigArea>();
        agentRigidbody = GetComponent<Rigidbody>();
        rayPerception = GetComponent<RayPerception>();
    }

    public override void CollectObservations()
    {

        float rayDistance = 20f;
        float[] rayAngles = { 90f };                // 볼 각도
        string[] detectableObjects = { "stump", "wall" };               // 레이캐스트를 이용하여 태그가 stump나 wall인 것을 감지
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

        // 아이템 감지
        AddVectorObs(GetNostrilStereo());

        Vector3 localVelocity = transform.InverseTransformDirection(agentRigidbody.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)       // 돼지의 움직임 코드
    {
        // 회전 액션 결정
        float rotateAmount = 0;
        if (vectorAction[1] == 1)      
        {
            Debug.Log("forward");
            rotateAmount = -rotateSpeed;
        }
        else if (vectorAction[1] == 2)
        {
            rotateAmount = rotateSpeed;
        }

        Vector3 rotateVector = transform.up * rotateAmount;
        agentRigidbody.MoveRotation(Quaternion.Euler(agentRigidbody.rotation.eulerAngles + rotateVector * rotateSpeed));

        // 움직임 액션 결정
        float moveAmount = 0;
        if (vectorAction[0] == 1)           // 앞으로  w의 트리거 
        {
            moveAmount = moveSpeed;
        }
        else if (vectorAction[0] == 2)
        {
            moveAmount = moveSpeed * -.5f;  // 뒤로 가는것은 천천히
        }

        // Apply the movement
        Vector3 moveVector = transform.forward * moveAmount;
        agentRigidbody.AddForce(moveVector * moveSpeed, ForceMode.VelocityChange);
        //agentRigidbody.AddForce(moveVector * moveSpeed);

        // Determine state
        if (GetCumulativeReward() <= -5f)
        {
            // reward가 너무 낮아서 포기
            Done();

            StartCoroutine(agentArea.SwapGroundMaterial(success: false));

            // Reset
            agentArea.ResetArea();
        }
        else if (trufflesCollected >= agentArea.GetSmellyObjects().Count)
        {
            // 성공
            Done();

            StartCoroutine(agentArea.SwapGroundMaterial(success: true));

            // Reset
            agentArea.ResetArea();
        }
        else
        {
            AddReward(-.001f);
            agentArea.UpdateScore(GetCumulativeReward());
        }
    }

    /// Reset the agent
    public override void AgentReset()
    {
        agentRigidbody.velocity = Vector3.zero;

        trufflesCollected = 0;
    }

    private Vector2 GetNostrilStereo()
    {
        List<GameObject> smellyObjects = agentArea.GetSmellyObjects();
        if (smellyObjects == null)
            return Vector2.zero;

        float leftNostril = 0;
        Vector3 leftNostrilPosition = transform.position - nostrilWidth / 2.0f * transform.right;
        float rightNostril = 0;
        Vector3 rightNostrilPosition = transform.position + nostrilWidth / 2.0f * transform.right;

        foreach (GameObject smellyObject in smellyObjects)
        {
            if (smellyObject != null)
            {
                leftNostril += .8f - .5f * Mathf.Log10(Vector3.Distance(smellyObject.transform.position, leftNostrilPosition));
                rightNostril += .8f - .5f * Mathf.Log10(Vector3.Distance(smellyObject.transform.position, rightNostrilPosition));
            }
        }

        return new Vector2(leftNostril, rightNostril);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            CollectTruffle();
            Destroy(collision.gameObject);
            Debug.Log(trufflesCollected);
        }
        else if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(-.01f);
        }
        else if (collision.gameObject.CompareTag("Water"))
            agentArea.ResetArea();

    }

    private void CollectTruffle()
    {
        trufflesCollected++;

        // Reward and update the score text display
        AddReward(1f);
        agentArea.UpdateScore(GetCumulativeReward());
    }
}
