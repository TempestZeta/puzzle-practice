using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StageDate stageDate;
    public GameObject selectI;
    public int GameSize;
    public NewBlock[] LeadBlock;
    public NewBlock[][] newBlocks;
    public Index selectIndex;
    public bool isAni;

    // 점수계산
    public ScoreChecker scChecker;

    int stageNum;
    Index noneSelect;  
    Index saveIndex;

    // 터치 조작시 정보 담는 구조체
    Touch touch;
    Vector2 touchPos;

    public BlockChecker blockChecker;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(800, 1280, true);
        stageNum = 0;
        isAni = false;
        noneSelect.x = 99;
        noneSelect.y = 99;
        saveIndex = noneSelect;
        selectIndex = noneSelect;

        newBlocks = new NewBlock[GameSize][];
    }

    private void Start()
    {
        for (int i = 0; i < GameSize; i++)
        {
            newBlocks[i] = new NewBlock[6];
            for (int j = 0; j < GameSize; j++)
            {
                newBlocks[i][j] = LeadBlock[i + j * GameSize];
                newBlocks[i][j].ChangeBlockColor(stageDate.stageInfo[stageNum][i + j * GameSize]);

                newBlocks[i][j].SetIndex(i, j);
                newBlocks[i][j].selectBlock = SelectIndex;
            }
        }

        scChecker.GameStart(stageNum);

        blockChecker.HorizonCheck(newBlocks);
        blockChecker.VerticalCheck(newBlocks);
        blockChecker.BlockBreak();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!isAni)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit2D hit;
            //터치 시
            if (Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity, 1 << 8);
                if (hit)
                {
                    NewBlock block = hit.collider.GetComponent<NewBlock>();
                    if (block.kind != BlockKind.PURPPLE)
                    {
                        block.OnClick();
                    }
                }
            }
            //놓을 시
            else if (Input.GetMouseButtonUp(0))
            {

                hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity, 1 << 8);

            }
        }

#endif
#if UNITY_ANDROID

        if (!isAni)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) // 터치 시작했을 때 pos를 받는다
                {
                    touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    Ray tRay = Camera.main.ScreenPointToRay(touchPos);
                    RaycastHit2D tHit;

                    tHit = Physics2D.Raycast(touchPos, Vector3.forward, Mathf.Infinity, 1 << 8);

                    if (tHit)
                    {
                        NewBlock block = tHit.collider.GetComponent<NewBlock>();
                        if (block.kind != BlockKind.PURPPLE)
                        {
                            block.OnClick();
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    TouchChangeBlock(CheckTouchDirection());
                }
            }
        }
#endif
        

        if (scChecker.GameOverCheck() && !isAni) // 게임 오버 체크
        {
            if (scChecker.isClear)
            {
                stageNum++;
                if (stageNum < 5)
                {
                    // 클리어
                    StageRestart();
                }
            }
            else
            {
                // 제한시간 종료
            }
        }

    }



    public void SelectIndex(Index index)
    {
        saveIndex = selectIndex;
        selectIndex = index;

        selectI.transform.position = newBlocks[selectIndex.x][selectIndex.y].transform.position;
      
        if (saveIndex.x == selectIndex.x && saveIndex.y == selectIndex.y)
        {
            saveIndex = noneSelect;
            selectIndex = noneSelect;
        }
        else if(saveIndex.x + 1 == selectIndex.x && saveIndex.y == selectIndex.y)
        {
            SwapIndex();
        }
        else if (saveIndex.x - 1 == selectIndex.x && saveIndex.y == selectIndex.y)
        {
            SwapIndex();
        }
        else if (saveIndex.x == selectIndex.x && saveIndex.y + 1 == selectIndex.y)
        {
            SwapIndex();
        }
        else if (saveIndex.x == selectIndex.x && saveIndex.y - 1 == selectIndex.y)
        {
            SwapIndex();
        }
        else
        {
            selectIndex = index;
            saveIndex = index;
        }
    }

    public void SwapIndex()
    {
        NewBlock block = newBlocks[saveIndex.x][saveIndex.y];
        Vector3 pos = newBlocks[saveIndex.x][saveIndex.y].transform.position;
        Index index = newBlocks[saveIndex.x][saveIndex.y].index;

        iTween.MoveTo(block.gameObject, newBlocks[selectIndex.x][selectIndex.y].transform.position, 0.2f);
        iTween.MoveTo(newBlocks[selectIndex.x][selectIndex.y].gameObject, block.transform.position, 0.2f);

        newBlocks[saveIndex.x][saveIndex.y].index = newBlocks[selectIndex.x][selectIndex.y].index;
        newBlocks[selectIndex.x][selectIndex.y].index = index;

        newBlocks[saveIndex.x][saveIndex.y] = newBlocks[selectIndex.x][selectIndex.y];
        newBlocks[selectIndex.x][selectIndex.y] = block;

        saveIndex = noneSelect;
        selectIndex = noneSelect;

        blockChecker.HorizonCheck(newBlocks);
        blockChecker.VerticalCheck(newBlocks);
        blockChecker.BlockBreak();
    }

    public void SawpBlock(Index Index1, Index Index2)
    {
        // 바뀌는 블록 (미선택)
        NewBlock sBlock = newBlocks[Index2.x][Index2.y];
        // 바뀌는 블록의 인덱스 저장
        Index index = sBlock.index;

        newBlocks[Index2.x][Index2.y].index = newBlocks[Index1.x][Index1.y].index;
        newBlocks[Index1.x][Index1.y].index = index;

        newBlocks[Index2.x][Index2.y] = newBlocks[Index1.x][Index1.y];
        newBlocks[Index1.x][Index1.y] = sBlock;
    }

    // 터치 조작 시 방향 체크
    int CheckTouchDirection()
    {
        Vector2 v = touch.deltaPosition.normalized;

        float x = v.x; //touch.deltaPosition.x;
        float y = v.y; //touch.deltaPosition.y;

        if (Mathf.Abs(x) > 0.5f && Mathf.Abs(y) > 0.5f) return -1;

        if (x >= 0.5f)
        {
            if (y <= 0.2f && y >= -0.2f)
            {
                return 0;
            }
        }
        else if (x <= -0.5f)
        {
            if (y <= 0.2f && y >= -0.2f)
            {
                return 1;
            }
        }
        if (y >= 0.5f)
        {
            if (x <= 0.2f && x >= -0.2f)
            {
                return 2;
            }
        }
        else if (y <= -0.5f)
        {
            return 3;
        }
        return -1;
    }

    void TouchChangeBlock(int dir)
    {
        if (saveIndex.x == 99 || saveIndex.y == 99) return;

        switch (dir)
        {
            case 0: //R
                if(selectIndex.x < GameSize - 1)
                {
                    Index tempIndex = selectIndex;
                    tempIndex.x++;
                    if (newBlocks[tempIndex.x][tempIndex.y].kind != BlockKind.PURPPLE)
                    {
                        SwapBlock(tempIndex);
                    }
                   
                }
                break;
            case 1: //L 
                if (selectIndex.x > 0)
                {
                    Index tempIndex = selectIndex;
                    tempIndex.x--;

                    if (newBlocks[tempIndex.x][tempIndex.y].kind != BlockKind.PURPPLE)
                    {
                        SwapBlock(tempIndex);
                    }
                }
                break;
            case 2: //T
                if (selectIndex.y < GameSize - 1)
                {
                    Index tempIndex = selectIndex;
                    tempIndex.y++;
                    if (newBlocks[tempIndex.x][tempIndex.y].kind != BlockKind.PURPPLE)
                    {
                        SwapBlock(tempIndex);
                    }
                }
                break;
            case 3: //B
                if (selectIndex.y > 0)
                {
                    Index tempIndex = selectIndex;
                    tempIndex.y--;
                    if (newBlocks[tempIndex.x][tempIndex.y].kind != BlockKind.PURPPLE)
                    {
                        SwapBlock(tempIndex);
                    }
                }
                break;
            default:
                break;
        }
    }

    void SwapBlock(Index tempIndex)
    {
        NewBlock block = newBlocks[tempIndex.x][tempIndex.y];
        Vector3 pos = newBlocks[tempIndex.x][tempIndex.y].transform.position;
        Index index = block.index;

        iTween.MoveTo(block.gameObject, newBlocks[selectIndex.x][selectIndex.y].transform.position, 0.2f);
        iTween.MoveTo(newBlocks[selectIndex.x][selectIndex.y].gameObject, block.transform.position, 0.2f);

        newBlocks[tempIndex.x][tempIndex.y].index = newBlocks[selectIndex.x][selectIndex.y].index;
        newBlocks[selectIndex.x][selectIndex.y].index = index;

        newBlocks[tempIndex.x][tempIndex.y] = newBlocks[selectIndex.x][selectIndex.y];
        newBlocks[selectIndex.x][selectIndex.y] = block;

        selectIndex = noneSelect;

        blockChecker.HorizonCheck(newBlocks);
        blockChecker.VerticalCheck(newBlocks);
        blockChecker.BlockBreak();
    }

    void StageRestart()
    {
        Debug.Log(stageNum);

        for(int i = 0; i < GameSize; i++)
        {
            for (int j = 0; j < GameSize; j++)
            {
                //newBlocks[i][j] = LeadBlock[i + j * GameSize];
                newBlocks[i][j].ChangeBlockColor(stageDate.stageInfo[stageNum][i + j * GameSize]);

                //newBlocks[i][j].SetIndex(i, j);
                //newBlocks[i][j].selectBlock = SelectIndex;
            }
        }

        scChecker.GameStart(stageNum);

        blockChecker.HorizonCheck(newBlocks);
        blockChecker.VerticalCheck(newBlocks);
        blockChecker.BlockBreak();
    }

}