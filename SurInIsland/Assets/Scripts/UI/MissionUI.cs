using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    public bool step1_Survive = false;
    public bool step2_ = false;

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!step1_Survive)
            text.text = "살아남기 위한 총을 찾으시오";

        else if (!step2_)
            text.text = "사냥을 하여 허기를 채우시오";

    }

    public void Test()
    {
        Debug.Log("호출");
    }

    //private void Step1()
    //{
    //    text.text = "살아남기 위한 도구를 찾으시오";
    //}
}
