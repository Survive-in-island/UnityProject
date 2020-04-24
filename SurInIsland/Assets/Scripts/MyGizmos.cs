using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type { NORMAL, ITEM, ANIMAL, ROCK }
    private const string animalPointFile = "Animal";
    private const string itemPointFile = "Item";
    private const string rockPointFile = "Rock";


    public Type type = Type.ANIMAL;

    public Color _color = Color.yellow;
    public float _radius = 0.5f;

    // 기즈모 색상 설정
    private void OnDrawGizmos()
    {    

        if(type == Type.NORMAL)
        {
            // 기즈모 색상 설정
            Gizmos.color = _color;
            // 구체 모양의 기즈모 생성, 인자는 (생성 위지, 반지름)
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else if(type == Type.ANIMAL)
        {
            Gizmos.color = _color;
            // 이미지 파일 표시
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, animalPointFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        else if (type == Type.ITEM)
        {
            Gizmos.color = _color;
            // 이미지 파일 표시
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, itemPointFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        else if (type == Type.ROCK)
        {
            Gizmos.color = _color;
            // 이미지 파일 표시
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, rockPointFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
