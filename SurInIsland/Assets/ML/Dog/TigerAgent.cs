using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerAgent : Agent
{
    [Header("Pig Agent Settings")]
    public float moveSpeed = 1f;
    public float rotateSpeed = 2f;
    public float nostrilWidth = .5f;

    private TigerAcademy agentAcademy;
    private TigerArea agentArea;
    private Rigidbody agentRigidbody;
    private RayPerception rayPerception;

    private int trufflesCollected = 0;

    [SerializeField]
    private Animator anim;

    private bool isAttack = false;
    private bool isWalk = false;
    /// Initialize the agent
    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentAcademy = FindObjectOfType<TigerAcademy>();
        agentArea = transform.parent.GetComponent<TigerArea>();
        agentRigidbody = GetComponent<Rigidbody>();
        rayPerception = GetComponent<RayPerception>();

        ////////////////////////////////////////////
        anim = GetComponent<Animator>();
    }

    /// Collect all observations that the agent will use to make decisions
    public override void CollectObservations()
    {

        // Add raycast perception observations for stumps and walls
        float rayDistance = 20f;
        float[] rayAngles = { 90f };                // 볼 각도
        string[] detectableObjects = { "stump", "wall" };               // 레이캐스트를 이용하여 태그가 stump나 wall인 것을 감지
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

        // Sniff for truffles
        AddVectorObs(GetNostrilStereo());

        // Add velocity observation
        Vector3 localVelocity = transform.InverseTransformDirection(agentRigidbody.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)       // 호랑이의 움직임 코드
    {
        // 회전 액션 결정
        float rotateAmount = 0;
        if (vectorAction[1] == 1)
        {
            anim.SetBool("Walking", isWalk);

            Debug.Log("forward");
            rotateAmount = -rotateSpeed;
        }
        else if (vectorAction[1] == 2)
        {
            anim.SetBool("Walking", isWalk);

            rotateAmount = rotateSpeed;
        }

        Vector3 rotateVector = transform.up * rotateAmount;
        agentRigidbody.MoveRotation(Quaternion.Euler(agentRigidbody.rotation.eulerAngles + rotateVector * rotateSpeed));

        // 움직임 액션 결정
        float moveAmount = 0;
        if (vectorAction[0] == 1)           // 앞으로  w의 트리거 
        {
            isWalk = true;
            anim.SetBool("Walking", isWalk);

            moveAmount = moveSpeed;
        }
        else if (vectorAction[0] == 2)
        {
            isWalk = true;
            anim.SetBool("Walking", isWalk);
            moveAmount = moveSpeed * -.5f;  // 뒤로 가는것은 천천히
        }

        // Apply the movement
        Vector3 moveVector = transform.forward * moveAmount;
        agentRigidbody.AddForce(moveVector * moveSpeed, ForceMode.VelocityChange);

        // Determine state
        if (GetCumulativeReward() <= -5f)
        {
            // Reward is too negative, give up
            Done();

            // Indicate failure with the ground material
            StartCoroutine(agentArea.SwapGroundMaterial(success: false));

            // Reset
            agentArea.ResetArea();
        }
        else if (trufflesCollected >= agentArea.GetSmellyObjects().Count)
        {
            // All truffles collected, success!
            Done();

            // Indicate success with the ground material
            StartCoroutine(agentArea.SwapGroundMaterial(success: true));

            // Reset
            agentArea.ResetArea();
        }
        else
        {
            // Encourage movement with a tiny time penalty and pdate the score text display
            AddReward(-.001f);
            agentArea.UpdateScore(GetCumulativeReward());
        }
    }

    /// Reset the agent
    public override void AgentReset()
    {
        // Reset velocity
        agentRigidbody.velocity = Vector3.zero;

        // Reset number of truffles collected
        trufflesCollected = 0;

        isAttack = false;
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
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Pig") || (collision.gameObject.CompareTag("Rabbit"))))             // truffle에서 item으로 수정
        {
            // attack

            CollectTruffle();
            isAttack = true;
            //Destroy(collision.gameObject);
            anim.SetBool("Attack", isAttack);
            //anim.SetTrigger("Hit");
        }
        else if (collision.gameObject.CompareTag("stump") || collision.gameObject.CompareTag("building"))
        {
            AddReward(-.01f);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        isAttack = false;
    }

    private void CollectTruffle()               
    {
        trufflesCollected++;

        // Reward and update the score text display
        AddReward(1f);
        agentArea.UpdateScore(GetCumulativeReward());
    }
}
