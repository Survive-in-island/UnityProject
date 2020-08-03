using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLMoveEx : MonoBehaviour
{
    public Rigidbody cube;
    // Start is called before the first frame update
    void Start()
    {
        cube = GetComponent<Rigidbody>();
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            float dir1 = Random.Range(2f, 4f);
            float dir2 = Random.Range(2f, 4f);

            yield return new WaitForSeconds(1f);

            cube.velocity = new Vector3(dir1 * 3, 0, dir2 * 3);
        }
    }
}
