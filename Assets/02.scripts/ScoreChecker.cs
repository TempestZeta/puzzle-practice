using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChecker : MonoBehaviour
{
    [System.Serializable]
    public struct StageClear
    {
        public float time;
        public int score;
    }

    public StageClear[] stageInfo;

    public int whatStage; // 지금 몇 스테이지?

    public Text timeText;
    public Text scoreText;

    float limitTime; // 제한 시간 출력용
    public int getScore; // 플레이어가 획득한 점수
    public int currScore = 0;

    public bool isOver; // 게임 끝났나
    public bool isClear; // 게임 깼나

    private void LateUpdate()
    {
        if (!isOver)
        {
            limitTime -= Time.deltaTime;
            timeText.text = "TIME : " + (int)limitTime;
        }

        scoreText.text = "SCORE : " + getScore;
    }

    public void GameStart(int stage) // 제한 시간 받아오기
    {
        whatStage = stage;
        limitTime = stageInfo[whatStage].time;
        currScore = 0;
        isOver = false;
        isClear = false;
    }

    public void PlusScore(int value) // 점수 더하기
    {
        currScore += value;
        getScore += value;
    }

    public bool GameOverCheck()
    {
        if(limitTime <= 0.001f)
        {
            isOver = true;
        }
        else if (currScore >= stageInfo[whatStage].score)
        {
            isOver = true;
            isClear = true;
        }
        return isOver;
    }
    


}
