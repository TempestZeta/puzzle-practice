using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDate : MonoBehaviour
{
    public int stageNum;
    public bool[][] stageInfo;


    private void Awake()
    {
        stageInfo = new bool[stageNum][];
       
        stageInfo[0] = new bool[36]
        {false,false,false,false,false,false,
        false,false,false,false,false,false,
        false,false,false,false,false,false,
        false,false,false,false,false,false,
        false,false,false,false,false,false,
        false,false,false,false,false,false};

        stageInfo[1] = new bool[36]
        {false,false,false,false,false,false,
        false,false,false,false,false,true,
        true,false,false,false,false,false,
        false,false,false,false,false,true,
        true,false,false,false,false,false,
        false,false,false,false,false,false};

        stageInfo[2] = new bool[36]
        {false,false,false,false,false,false,
        false,false,false,false,false,false,
        false,false,true,true,false,false,
        false,false,true,true,false,false,
        false,false,false,false,false,false,
        false,false,false,false,false,false};

        stageInfo[3] = new bool[36]
        {true,false,false,false,false,true,
        true,false,false,false,false,true,
        true,false,false,false,false,true,
        true,false,false,false,false,true,
        true,false,false,false,false,true,
        true,false,false,false,false,true};

        stageInfo[4] = new bool[36]
        {true,true,false,false,true,true,
        false,false,false,false,false,false,
        false,false,false,false,false,false,
        true,false,false,false,true,true,
        true,false,false,false,true,true,
        true,false,false,false,true,true};



    }
}
