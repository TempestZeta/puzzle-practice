using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockStatus
{
    NULL = -1,
    RED = 0,
    BLUE,
    GREEN,
    WHITE,
    FLOOR
}

public delegate void RemovePooler(Block gameObject, int row);

// 블록 개별 
public class Block : MonoBehaviour
{

    public RemovePooler removePooler;
    // 블록 색깔
    public BlockStatus blockColor;
    // 스프라이트
    SpriteRenderer renderer;
    Rigidbody2D rigidbody2D;
    // 현재 움직이는 블록인지
    public bool isMove;
    // 파괴되었는지
    public bool isDestroy;
    // 생성되어서 내려오는 중인지
    public bool isDown;
    // 몇 번째 열인지
    public int currRow;

    public bool isSelect;
    // 인접한 블록 (R, L, B, T)
   public BlockCollide[] collBlocks;


    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        ChangeBlockColor();
        isSelect = false;
    }


    // 블록 색 변경 함수
    public void ChangeBlockColor()
    {
        switch (Random.Range(0,4))
        {
            case 0:
                renderer.color = Color.red;
                blockColor = BlockStatus.RED;
                break;
            case 1:
                renderer.color = Color.green;
                blockColor = BlockStatus.GREEN;
                break;
            case 2:
                renderer.color = Color.blue;
                blockColor = BlockStatus.BLUE;
                break;
            case 3:
                renderer.color = Color.white;
                blockColor = BlockStatus.WHITE;
                break;
        }

        isDown = false;
        isDestroy = false;
    }

    private void Update()
    {
        if (rigidbody2D.velocity.sqrMagnitude <= 0.01f * 0.01f)
        {
            isDown = true;
        }
        else
        {
            isDown = false;
        }

        if (isDown && !isSelect)
        {
            HorizonColor();
            VerticalColor();

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            HorizonColor();
            VerticalColor();
        }
    }

  
    public void HorizonColor()
    {
        if (collBlocks[0].blockStatus == blockColor && collBlocks[1].blockStatus == blockColor)
        {
            removePooler(this, currRow);
            removePooler(collBlocks[0].block, currRow - 1);
            removePooler(collBlocks[1].block, currRow + 1);
        }
    }

    public void VerticalColor()
    {
        if (collBlocks[2].blockStatus == blockColor && collBlocks[3].blockStatus == blockColor)
        {
            removePooler(this, currRow);
            removePooler(collBlocks[2].block, currRow);
            removePooler(collBlocks[3].block, currRow);
        }
    }

    public void SendDestroy(Block block)
    {
        for(int i = 0; i < collBlocks.Length; i++)
        {
            collBlocks[i].SendMessage("Destroy");
        }
        collBlocks[3].SendMessage("BlockDown");
    }


    

}
