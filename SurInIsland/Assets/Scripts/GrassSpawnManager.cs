using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawnManager : MonoBehaviour
{
    // 풀이 출현할 위치를 담을 배열
    public Transform[] points;
    // 풀 프리팹을 저장할 변수
    public GameObject grass;
    // 풀을 생성할 주기
    public float grassCreateTime = 5.0f;
    // 풀 최대 생성 개수
    public int maxGrass = 20;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("GrassSpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateTree());
        }
    }

    IEnumerator CreateTree()
    {
        while (!isGameOver)
        {
            // 현재 생성된 풀 개수 산출
            int itemCount = (int)GameObject.FindGameObjectsWithTag("Grass").Length;      // 나중에 수정

            // 풀의 최대 생성 개수보다 작을때만 나무 생성
            if (itemCount < maxGrass)
            {
                // 풀 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(grassCreateTime);

                // 불규칙적인 위치 산출

                int idx = Random.Range(1, points.Length);

                // 풀의 동적 생성
                Instantiate(grass, points[idx].position, points[idx].rotation);

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
