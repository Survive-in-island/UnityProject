using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLDogSpawn : MonoBehaviour
{
    // 출현할 위치를 담을 배열
    public Transform[] points;
    // 프리팹을 저장할 변수
    public GameObject mlDog;
    // 생성할 주기
    public float mlDogCreateTime = 5.0f;
    // 최대 생성 개수
    public int maxMLDog = 4;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("MLDogSpawn").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMLDog());
        }
    }

    IEnumerator CreateMLDog()
    {
        while (!isGameOver)
        {
            // 현재 생성된 돌 개수 산출
            int itemCount = (int)GameObject.FindGameObjectsWithTag("DogKnight").Length;      // 나중에 수정

            // 돌의 최대 생성 개수보다 작을때만 돌 생성
            if (itemCount < maxMLDog)
            {
                // 돌 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(mlDogCreateTime);

                // 불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);

                // 돌의 동적 생성
                Instantiate(mlDog, points[idx].position, points[idx].rotation);

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
