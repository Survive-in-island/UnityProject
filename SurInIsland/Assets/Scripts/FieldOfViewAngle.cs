using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;   // 시야각 120도
    [SerializeField] private float viewDistance; // 시야거리 10미터
    [SerializeField] private LayerMask targetMask; // 타켓 마스크 (플레이어)

    //private Pig thePig;
    private FPSController thePlayer;
    //[SerializeField]
    //private GameObject player;

    void Start()
    {
        //thePig = GetComponent<Pig>();
        thePlayer = FindObjectOfType<FPSController>();
    }

    public Vector3 GetTargetPos()
    {
        //Debug.Log(thePlayer.transform.position);
        return thePlayer.transform.position;
        //return player.transform.position;
    }



    public bool View()
    {
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
                            //Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            //thePig.Run(_hit.transform.position);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
