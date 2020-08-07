using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnManager : MonoBehaviour
{
    // 나무가 출현할 위치를 담을 배열
    public Transform[] points;
    // 나무 프리팹을 저장할 변수
    public GameObject tree;
    // 나무를 생성할 주기
    public float treeCreateTime = 5.0f;
    // 나무 최대 생성 개수
    public int maxTree = 20;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("TreeSpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateTree());
        }
    }

    IEnumerator CreateTree()
    {
        while (!isGameOver)
        {
            // 현재 생성된 나무 개수 산출
            int itemCount = (int)GameObject.FindGameObjectsWithTag("Tree").Length;      // 나중에 수정

            // 나무의 최대 생성 개수보다 작을때만 나무 생성
            if (itemCount < maxTree)
            {
                // 돌 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(treeCreateTime);

                // 불규칙적인 위치 산출

                int idx = Random.Range(1, points.Length);

                // 돌의 동적 생성
                Instantiate(tree, points[idx].position, points[idx].rotation);

            }

            else
                yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
