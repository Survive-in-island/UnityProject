using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    // 아이템이 출현할 위치를 담을 배열
    public Transform[] points;

    // 아이템 프리팹을 저장할 변수             // 여러개가 필요
    [SerializeField]
    public GameObject[] item;

    // 아이템을 생성할 주기
    public float itemCreatTime = 5.0f;
    // 아이템 최대 생성 개수
    public int maxItem = 20;                 

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("ItemSpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateItem());
        }
    }

    IEnumerator CreateItem()
    {
        while (!isGameOver)
        {
            // 현재 생성된 아이템 개수 산출
            int itemCount = (int)GameObject.FindGameObjectsWithTag("Item").Length;      // 나중에 수정

            // 아이템의 최대 생성 개수보다 작을때만 아이템 생성
            if (itemCount < maxItem)
            {
                // 동물 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(itemCreatTime);

                // 불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);

                // 아이템의 동적 생성
                //Instantiate(item, points[idx].position, points[idx].rotation);
                RandomItem(idx);

            }

            else
                yield return null;
        }
    }

    private void RandomItem(int idx)
    {

        int _random = Random.Range(0, 3);       // 구급상자, 물, 총알 일단 이렇게 3개

        if (_random == 0)
            CreateMedkit(idx);
        else if (_random == 1)
            CreateBottle(idx);
        else if (_random == 2)
            CreateAmmo(idx);
    }

    private void CreateMedkit(int idx)
    {
        Instantiate(item[0], points[idx].position, points[idx].rotation);
    }

    private void CreateBottle(int idx)
    {
        Instantiate(item[1], points[idx].position, points[idx].rotation);

    }

    private void CreateAmmo(int idx)
    {
        Instantiate(item[2], points[idx].position, points[idx].rotation);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
