using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct IndexTransForm
{
    public Transform[] tr;
}

public class BlockChecker : MonoBehaviour
{
    public GameManager gameManager;
    public ScoreChecker scChecker;
    public IndexTransForm[] startTransForm;
    public IndexTransForm[] endTransForm;
    public ComboText comboText;

    bool[][] checkIndex;
    Stack<NewBlock>[] upBlockSaveT;
    List<NewBlock> destroyList;

    // 점수 계산 필요
    int combo = 0;
    

    // Start is called before the first frame update
    void Awake()
    {
        destroyList = new List<NewBlock>();
        checkIndex = new bool[6][];
        upBlockSaveT = new Stack<NewBlock>[6];
        for (int i = 0; i < 6; i++)
        {
            checkIndex[i] = new bool[6];
            upBlockSaveT[i] = new Stack<NewBlock>();
        }
        ReSetCheckIndex();
    }

    public void HorizonCheck(NewBlock[][] stage)
    {
        gameManager.isAni = true;
        BlockKind checkColor;
        int start = 0;
        int count = 0;
        for (int i = 0; i < stage[0].Length; i++)
        {
            checkColor = BlockKind.NULL;

            for (int j = 0; j < stage[i].Length; j++)
            {
                if (checkColor == stage[i][j].kind)
                {
                    if (!(stage[i][j].kind == BlockKind.PURPPLE))
                        count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        for (int k = start; k < count + start; k++)
                        {
                            if (!stage[i][k].isDestroy)
                            {
                                stage[i][k].isDestroy = true;
                                destroyList.Add(stage[i][k]);
                            }
                        }
                        count = 0;
                    }

                    checkColor = stage[i][j].kind;
                    start = j;

                    if (stage[i][j].kind == BlockKind.PURPPLE)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = 1;
                    }
                }
            }

            if (count >= 3)
            {
                for (int k = start; k < count + start; k++)
                {
                    if (!stage[i][k].isDestroy)
                    {
                        stage[i][k].isDestroy = true;
                        destroyList.Add(stage[i][k]);
                    }
                }
            }

            count = 0;
        }
        if (count >= 3)
        {
            for (int k = start; k < count + start; k++)
            {
                if (!stage[stage[0].Length - 1][k].isDestroy)
                {
                    stage[stage[0].Length - 1][k].isDestroy = true;
                    destroyList.Add(stage[stage[0].Length - 1][k]);
                }
            }
        }
    }

    public void VerticalCheck(NewBlock[][] stage)
    {
        BlockKind checkColor;
        int start = 0;
        int count = 0;

        for (int i = 0; i < stage[0].Length; i++)
        {
            checkColor = BlockKind.NULL;

            for (int j = 0; j < stage[i].Length; j++)
            {
                if (checkColor == stage[j][i].kind)
                {
                    if(!(stage[j][i].kind == BlockKind.PURPPLE))
                        count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        for (int k = start; k < count + start; k++)
                        {
                            if (!stage[k][i].isDestroy)
                            {
                                stage[k][i].isDestroy = true;
                                destroyList.Add(stage[k][i]);
                            }
                        }
                        count = 0;
                    }

                    checkColor = stage[j][i].kind;
                    start = j;

                    if (stage[j][i].kind == BlockKind.PURPPLE)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = 1;
                    }
                }
            }
            if (count >= 3)
            {
                for (int k = start; k < count + start; k++)
                {
                    stage[k][i].isDestroy = true;
                    destroyList.Add(stage[k][i]);
                }
            }
            count = 0;

        }

        if (count >= 3)
        {
            for (int k = start; k < count + start; k++)
            {
                if (!stage[k][stage[0].Length - 1].isDestroy)
                {
                    stage[k][stage[0].Length - 1].isDestroy = true;
                    destroyList.Add(stage[k][stage[0].Length - 1]);
                }
            }
        }
    }

    public void BlockBreak()
    {
        int count = destroyList.Count;
        for (int i = 0; i < destroyList.Count; i++)
        {
            checkIndex[destroyList[i].index.x][destroyList[i].index.y] = true;
            //destroyList[i].ChangeBlock();          
        }
        destroyList.Clear();
        
        if(count != 0)
        {
            //위치이동
            StartCoroutine(BlockArrangement());
            combo++;
            comboText.SetComboText(combo);
        }
        else
        {
            gameManager.isAni = false;
            combo = 0;
            comboText.SetComboText(combo);
        }

        int score = (count * 10) * (combo + 1);
        scChecker.PlusScore(score);
    }

    IEnumerator BlockBreakCo()
    {             
        yield return new WaitForSeconds(0.3f);
        HorizonCheck(gameManager.newBlocks);
        VerticalCheck(gameManager.newBlocks);
        BlockBreak();       
        yield break;
    }

    IEnumerator BlockArrangement()
    {
        Index index;
        int count;
        //터지는 애니메이션
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (checkIndex[i][j])
                {              
                    gameManager.newBlocks[i][j].ChangeBlock();
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        //위로 올리는 작업
        for (int i = 0; i < 6; i++)
        {
            count = 5;
            for (int j = 0; j < 6; j++)
            {
                if (checkIndex[i][j])
                {                   
                    //올라간 항목 리스트에 저장
                    upBlockSaveT[i].Push(gameManager.newBlocks[i][j]);
                    //위로 올림
                    gameManager.newBlocks[i][j].UpToBlock(startTransForm[i].tr[count].position);
                    //gameManager.SawpBlock(gameManager.newBlocks[i][j].index,index);
                    count--;
                }             
            }
        }
        //아래로 내리는 작업
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (gameManager.newBlocks[i][j].kind == BlockKind.PURPPLE) continue;
                //위로 올라가지 않았다면
                if (!checkIndex[i][j])
                {
                    for(int k = 0; k < j; k++)
                    {
                        if (checkIndex[i][k])
                        {
                            index.x = i;
                            index.y = k;
                            //비어있는 가장 아래로 이동시킴
                            gameManager.newBlocks[i][j].DownToBlock(endTransForm[i].tr[k].position);
                            //원래 있던 자리 비움
                            checkIndex[i][j] = true;
                            //들어간자리 채움
                            checkIndex[i][k] = false;
                            gameManager.SawpBlock(gameManager.newBlocks[i][j].index, index);
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        //위로 올렸던거 내리는 작업      
        for (int j = 5; j >= 0; j--)
        {
            for (int i = 0; i < 6; i++)
            {
                if (upBlockSaveT[i].Count - 1 == j)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        if (checkIndex[i][k])
                        {
                            index.x = i;
                            index.y = k;
                            //비어있는 가장 아래로 이동시킴
                            upBlockSaveT[i].Peek().DownToChangeBlock(endTransForm[i].tr[k].position);
                            //들어간 자리 채움
                            checkIndex[i][k] = false;
                            gameManager.SawpBlock(upBlockSaveT[i].Peek().index, index);
                            upBlockSaveT[i].Pop();
                            break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        

        ReSetCheckIndex();
        StartCoroutine(BlockBreakCo());
        yield break;
    }

    public void ReSetCheckIndex()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                checkIndex[i][j] = false;
                
            }
        }
    }
    
}
