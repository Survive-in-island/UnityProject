using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;   // 시야각 120도
    [SerializeField] private float viewDistance; // 시야거리 10미터
    [SerializeField] private LayerMask targetMask; // 타켓 마스크 (플레이어)

    private Pig thePig;

    void Start()
    {
        thePig = GetComponent<Pig>();
    }

    // Update is called once per frame
    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));       // 삼각함수를 이용해서 벡터값 반환
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player" || _targetTf.name == "Tiger")      // 나중에 호랑이나 이런것도 추가해줄 것
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if(_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")    // 호랑이 추가하기!
                        {     
                            Debug.Log("플레이어가 돼지 시야 내에 있음");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }
            }
        }
    }
}
