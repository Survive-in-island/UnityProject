using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSpawnManager : MonoBehaviour
{
    // 야생동물이 출현할 위치를 담을 배열
    public Transform[] points;
    // 야생동물 프리팹을 저장할 변수
    public GameObject animal;
    // 야생동물을 생성할 주기
    public float animalCreatTime = 5.0f;
    // 야생동물 최대 생성 개수
    public int maxAnimal = 5;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("PigSpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateAnimal());
        }
    }

    IEnumerator CreateAnimal()
    {
        while (!isGameOver)
        {
            // 현재 생성된 동물 개수 산출
            int animalCount = (int)GameObject.FindGameObjectsWithTag("WeakAnimal").Length;      // 나중에 수정

            // 동물의 최대 생성 개수보다 작을때만 동물 생성
            if (animalCount < maxAnimal)
            {
                // 동물 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(animalCreatTime);

                // 불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);
                // 동물의 동적 생성
                Instantiate(animal, points[idx].position, points[idx].rotation);
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