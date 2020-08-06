using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ChameleonAgent : Agent
{
    [Tooltip("The area this agent belongs to")]
    public ChameleonArea chameleonArea;

    [Tooltip("The chameleon mesh that the color-changing material is on")]
    public GameObject chameleonMeshObject;

    private Material chameleonMaterial;
    private float colorHoldTime = 0f;

    public override void AgentReset()
    {
        chameleonArea.ResetArea();
        SetRandomColor();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // 색을 조정, + 0 -
        float redAction = ((int)vectorAction[0] - 1) * chameleonArea.ChameleonAcademy.colorChangeMagnitude;
        float greenAction = ((int)vectorAction[1] - 1) * chameleonArea.ChameleonAcademy.colorChangeMagnitude;
        float blueAction = ((int)vectorAction[2] - 1) * chameleonArea.ChameleonAcademy.colorChangeMagnitude;

        // Set the agent color
        chameleonMaterial.color = new Color(Mathf.Clamp01(chameleonMaterial.color.r + redAction),
                                        Mathf.Clamp01(chameleonMaterial.color.g + greenAction),
                                        Mathf.Clamp01(chameleonMaterial.color.b + blueAction));

        if (colorHoldTime > 0f)
        {
            float rewardMultiplier = 0.7f;
            if (redAction + greenAction + blueAction == 0f)
            {
                rewardMultiplier = 1f;
            }

            // positive 보상
            AddReward(rewardMultiplier / agentParameters.maxStep);
        }
        else
        {
            // negativa 보상
            AddReward(-1f / agentParameters.maxStep);
        }
    }

    private void Start()
    {
        Debug.Assert(agentParameters.maxStep > 0, "Agent max step should be higher than 0.");
        chameleonMaterial = chameleonMeshObject.GetComponent<MeshRenderer>().material;
        StartCoroutine("FixUp");
        SetRandomColor();
    }

    //private void FixedUpdate()
    //{
    //    // Determine the correct color threshold
    //    float correctThreshold = 1 - chameleonArea.ChameleonAcademy.resetParameters["coloraccuracy"];

    //    // Calculate the average color difference
    //    Vector3 colorDifference = ColorDifference(chameleonMaterial.color, chameleonArea.PlatformColor);
    //    float averageDifference = (colorDifference.x + colorDifference.y + colorDifference.z) / 3f;

    //    if (averageDifference < correctThreshold)
    //    {
    //        // Color is close enough, add to the colorHoldTime
    //        colorHoldTime += Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        // Color not close enough, reset the colorHoldTime
    //        colorHoldTime = 0;
    //    }
    //}

    IEnumerator FixUp()
    {
        // Determine the correct color threshold
        float correctThreshold = 1 - chameleonArea.ChameleonAcademy.resetParameters["coloraccuracy"];

        // Calculate the average color difference
        Vector3 colorDifference = ColorDifference(chameleonMaterial.color, chameleonArea.PlatformColor);
        float averageDifference = (colorDifference.x + colorDifference.y + colorDifference.z) / 3f;

        if (averageDifference < correctThreshold)
        {
            // Color is close enough, add to the colorHoldTime
            colorHoldTime += Time.fixedDeltaTime;
        }
        else
        {
            // Color not close enough, reset the colorHoldTime
            colorHoldTime = 0;
        }

        yield return new WaitForSeconds(10f);
    }


    private Vector3 ColorDifference(Color a, Color b)
    {
        float redDifference = Mathf.Abs(a.r - b.r);
        float greenDifference = Mathf.Abs(a.g - b.g);
        float blueDifference = Mathf.Abs(a.b - b.b);

        return new Vector3(redDifference, greenDifference, blueDifference);
    }

    private void SetRandomColor()
    {
        Color randomColor = UnityEngine.Random.ColorHSV();
        chameleonMaterial.color = new Color(randomColor.r, randomColor.g, randomColor.b);
    }
}
