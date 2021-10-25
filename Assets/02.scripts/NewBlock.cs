using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SelectBlock(Index index);
[System.Serializable]
public struct Index
{
    public int x;
    public int y;
}
public enum BlockKind
{
    NULL = -1,
    RED = 0,
    BLUE,
    GREEN,
    WHITE,
    PURPPLE
}



public class NewBlock : MonoBehaviour
{
    public Index index;
    public BlockKind kind;
    public SelectBlock selectBlock;
    public Vector2 vector = new Vector3(0.1f, 0.1f);
    // 파괴 리스트에 들어갔는지 여부
    public bool isDestroy;

    SpriteRenderer renderer;

    // 블록 이미지
    public Sprite[] blockImg;

    private void Awake()
    {
        isDestroy = false;
        renderer = GetComponent<SpriteRenderer>();
        //ChangeBlockColor();
    }

    public void ChageBlock()
    {
        ChangeBlockColor();
    }

    public void ChangeBlockColor(bool isPupple = false)
    {
        if (isPupple)
        {
            renderer.sprite = blockImg[4];
            kind = BlockKind.PURPPLE;
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    //renderer.color = Color.red;
                    renderer.sprite = blockImg[0];
                    kind = BlockKind.RED;
                    break;
                case 1:
                    renderer.sprite = blockImg[1];
                    kind = BlockKind.GREEN;
                    break;
                case 2:
                    renderer.sprite = blockImg[2];
                    kind = BlockKind.BLUE;
                    break;
                case 3:
                    renderer.sprite = blockImg[3];
                    kind = BlockKind.WHITE;
                    break;
            }
        }
    }

    public void SetIndex(int x,int y)
    {
        index.x = x;
        index.y = y;
    }

    public void OnClick()
    {     
        selectBlock(index);
    }

    public void ChangeBlock()
    {
        iTween.ScaleFrom(gameObject, vector,0.3f);
        iTween.FadeTo(gameObject,0.0f,0.3f);
    }

    public void UpToBlock(Vector3 vec)
    {
        iTween.FadeTo(gameObject, 1.0f, 0.3f);
        gameObject.transform.position = vec;
    }
    public void DownToBlock(Vector3 vec)
    {
        iTween.MoveTo(gameObject,vec,0.2f);

        isDestroy = false;
    }
    public void DownToChangeBlock(Vector3 vec)
    {
        iTween.MoveTo(gameObject, vec, 0.1f);
        ChangeBlockColor();
        isDestroy = false;
    }
}
