using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollide : MonoBehaviour
{
    public Block block;
    public BlockStatus blockStatus;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Block")
        {           
            block = collision.GetComponent<Block>();
            if (block.isDown)
            {
                blockStatus = block.blockColor;
            }
            else
            {
                blockStatus = BlockStatus.NULL;
            }
                
        }
        else if(collision.tag == "Floor")
        {
            blockStatus = BlockStatus.FLOOR;
        }
        else
        {
            blockStatus = BlockStatus.NULL;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    blockStatus = BlockStatus.NULL;
    //}


    private void Awake()
    {
        blockStatus = BlockStatus.NULL;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    void BlockDown()
    {
        if (block)
        {
            block.SendMessage("MoveDown");
        }
    }

    void Destroy()
    {
        if (block)
        {
            block = null;
            blockStatus = BlockStatus.NULL;
        }
    }

}
