using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EndTransform
{
    public Transform[] data;

}
public class BlockPooler : MonoBehaviour
{
    public int blockPoolCount;
    public GameObject blockObj;
    public Queue<GameObject> queBlock;
    public Transform[] startTr;
    public EndTransform[] endTr;
    public List<Block> destroyList;
    int createNum;
    int[] stageArr;


    private void Awake()
    {
        queBlock = new Queue<GameObject>();
        destroyList = new List<Block>();
        
        stageArr = new int[6];
        createNum = 0;

        for (int i = 0; i < blockPoolCount; i++)
        {
            GameObject g = Instantiate(blockObj, transform.position, transform.rotation);
            Block block = g.GetComponent<Block>();
            block.removePooler = AddBlock;
            block.ChangeBlockColor();
            g.SetActive(false);
            queBlock.Enqueue(g);

        }
        StartCoroutine(StartGame());
        StartCoroutine(DeleteBlock());
    }

    private void LateUpdate()
    {
        if(destroyList.Count != 0)
        {
            for(int i = 0; i < destroyList.Count; i++)
            {
                createNum++;
                queBlock.Enqueue(destroyList[0].gameObject);
                destroyList[0].gameObject.SetActive(false);
                destroyList.RemoveAt(0);
            }
        }
    }

    IEnumerator DeleteBlock()
    {        
        while (true)
        {
            if (createNum != 0)
            {
                int index = FindNotSix();
        
                if (index != -1)
                {
                    UseBlock(index);
                    stageArr[index]++;
                    createNum--;
                }

            }
            yield return new WaitForSeconds(0.1f);
        }       
    }

    IEnumerator StartGame()
    {
        int count = 0;
        while (count < 36)
        {
            UseBlock(count % 6);
            stageArr[count % 6]++;
            count++;
            
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    //추가 블럭 큐에서 제거
    public void UseBlock(int row)
    {     
        GameObject g = queBlock.Dequeue();
        Block block = g.GetComponent<Block>();
        block.currRow = row;
        block.isDown = false;
        block.isDestroy = false;
        //block.MoveToBlock(endTr[stageArr[row]].data[row].position);
        g.transform.position = startTr[row].position;
        g.SetActive(true);
    }

    //터진 블럭 다시 큐에 추가
    public void AddBlock(Block block, int row)
    {
        if (!block.isDestroy)
        {
            block.isDestroy = true;
            stageArr[block.currRow]--;
            destroyList.Add(block);
        }
    }

    int FindNotSix()
    {
        for(int i = 0; i < stageArr.Length; i++)
        {
            if(stageArr[i] < 6)
            {
                
                return i;
            }
        }
        return -1;
    }
}
