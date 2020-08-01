using MLAgents;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PigArea : Area
{
    [Header("Pig Area Objects")]
    public GameObject pigAgent;
    public GameObject ground;
    public Material successMaterial;
    public Material failureMaterial;
    public TextMeshPro scoreText;

    [Header("Prefabs")]
    public GameObject trufflePrefab;
    public GameObject stumpPrefab;

    [HideInInspector]
    public int numTruffles;
    [HideInInspector]
    public int numStumps;
    [HideInInspector]
    public float spawnRange;
    
    private List<GameObject> spawnedTruffles;
    private List<GameObject> spawnedStumps;

    // (position, radius) 
    private List<Tuple<Vector3, float>> occupiedPositions;

    private Renderer groundRenderer;
    private Material groundMaterial;

    private int notGroundLayerMask;

    private void Start()
    {
        groundRenderer = ground.GetComponent<Renderer>();

        groundMaterial = groundRenderer.material;

        notGroundLayerMask = ~LayerMask.GetMask("ground");              ///
    }

    public override void ResetArea()
    {
        occupiedPositions = new List<Tuple<Vector3, float>>();
        ResetAgent();
        ResetTruffles();
        ResetStumps();
    }

    private void FixedUpdate()
    {
        // 돼지가 벗어나지 않게
        Vector3 pigLocalPosition = pigAgent.transform.localPosition;
        if (Mathf.Abs(pigLocalPosition.x) > 13f || Mathf.Abs(pigLocalPosition.z) > 13f)
        {
            Debug.LogWarning("Pig out of the pen!");
            PigAgent pigAgentComponent = pigAgent.GetComponent<PigAgent>();
            pigAgentComponent.SetReward(-5f);
            pigAgentComponent.AgentReset();
            ResetArea();
        }
    }

    public List<GameObject> GetSmellyObjects()
    {
        return spawnedTruffles;
    }


    public IEnumerator SwapGroundMaterial(bool success)
    {
        if (success)
        {
            groundRenderer.material = successMaterial;
        }
        else
        {
            groundRenderer.material = failureMaterial;
        }

        yield return new WaitForSeconds(0.5f);
        groundRenderer.material = groundMaterial;
    }

    public void UpdateScore(float score)                // 학습에서 점수를 얻었는지 
    {
        scoreText.text = score.ToString("0.00");
    }


    private void ResetAgent()
    {
        // 위치와 회전값 리셋 
        RandomlyPlaceObject(pigAgent, spawnRange, 10);
    }


    private void ResetTruffles()                // 랜덤한 위치에 먹이 생성
    {
        if (spawnedTruffles != null)
        {
            // 이전 step에서 남은 truffles 파괴 
            foreach (GameObject spawnedMushroom in spawnedTruffles.ToArray())
            {
                Destroy(spawnedMushroom);
            }
        }

        spawnedTruffles = new List<GameObject>();

        for (int i = 0; i < numTruffles; i++)
        {
            // 새로운 먹이 생성 
            GameObject truffleInstance = Instantiate(trufflePrefab, transform);
            RandomlyPlaceObject(truffleInstance, spawnRange, 50);
            spawnedTruffles.Add(truffleInstance);
        }
    }


    private void ResetStumps()      // 랜덤한 위치에 나무 생성
    {
        if (spawnedStumps != null)
        {
            // 실행 전 나무 파괴 
            foreach (GameObject spawnedTree in spawnedStumps.ToArray())
            {
                Destroy(spawnedTree);
            }
        }

        spawnedStumps = new List<GameObject>();

        for (int i = 0; i < numStumps; i++)
        {
            // 나무 랜덤하게 생성 
            GameObject stumpInstance = Instantiate(stumpPrefab, transform);
            RandomlyPlaceObject(stumpInstance, spawnRange, 50);
            spawnedStumps.Add(stumpInstance);
        }
    }



    private void RandomlyPlaceObject(GameObject objectToPlace, float range, float maxAttempts)
    {
        // 랜덤생성
        objectToPlace.GetComponent<Collider>().enabled = false;

        float testRadius = GetColliderRadius(objectToPlace) * 1.1f;

        objectToPlace.transform.rotation = Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f));

        int attempt = 1;
        while (attempt <= maxAttempts)
        {
            Vector3 randomLocalPosition = new Vector3(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
            randomLocalPosition.Scale(transform.localScale);

            if (CheckIfPositionIsOpen(transform.position + randomLocalPosition, testRadius))
            {
                objectToPlace.transform.localPosition = randomLocalPosition;
                occupiedPositions.Add(new Tuple<Vector3, float>(objectToPlace.transform.position, testRadius));
                break;
            }
            else if (attempt == maxAttempts)
            {
                Debug.LogError(string.Format("{0} couldn't be placed randomly after {1} attempts.", objectToPlace.name, maxAttempts));
                break;
            }

            attempt++;
        }

        objectToPlace.GetComponent<Collider>().enabled = true;
    }


    private static float GetColliderRadius(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();

        Vector3 boundsSize = Vector3.zero; 
        if (col.GetType() == typeof(MeshCollider))
        {
            boundsSize = ((MeshCollider)col).sharedMesh.bounds.size;
        }
        else if (col.GetType() == typeof(BoxCollider))
        {
            boundsSize = col.bounds.size;
        }

        boundsSize.Scale(obj.transform.localScale);
        return Mathf.Max(boundsSize.x, boundsSize.z) / 2f;
    }

    private bool CheckIfPositionIsOpen(Vector3 testPosition, float testRadius)
    {
        foreach (Tuple<Vector3, float> occupied in occupiedPositions)
        {
            Vector3 occupiedPosition = occupied.Item1;
            float occupiedRadius = occupied.Item2;
            if (Vector3.Distance(testPosition, occupiedPosition) - occupiedRadius <= testRadius)
            {
                return false;
            }
        }

        return true;
    }
}
