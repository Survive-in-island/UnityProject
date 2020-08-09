using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLAnimalSpawn : MonoBehaviour
{
    // ML동물이 출현할 위치를 담을 배열
    public Transform[] points;
    // ML동물 프리팹을 저장할 변수
    public GameObject mlAnimal;
    // ML동물을 생성할 주기
    public float mlAnimalCreateTime = 5.0f;
    // 최대 생성 개수
    public int maxMLAnimal = 7;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("MLPigSpawn").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMLAnimal());
        }
    }

    IEnumerator CreateMLAnimal()
    {
        while (!isGameOver)
        {
            // 현재 생성된 돌 개수 산출
            int itemCount = (int)GameObject.FindGameObjectsWithTag("MLAnimal").Length;      // 나중에 수정

            // 최대 생성 개수보다 작을때만 돌 생성
            if (itemCount < maxMLAnimal)
            {
                // 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(mlAnimalCreateTime);

                // 불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);

                // 동적 생성
                Instantiate(mlAnimal, points[idx].position, points[idx].rotation);

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
