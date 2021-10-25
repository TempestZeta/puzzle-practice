using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTouchCtr : MonoBehaviour
{

    public Touch touch;
    Vector2 touchPos;
    bool firstTouch;

    NewBlock selectBlock;
    NewBlock changeBlock;
    Transform savePos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Began) // 터치 시작했을 때 pos를 받는다
            {
                touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit2D hit;

                hit = Physics2D.Raycast(touchPos, Vector3.forward, Mathf.Infinity, 1 << 8);

                if (hit)
                {
                    hit.collider.GetComponent<NewBlock>().OnClick();
                }
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                Vector2 v = touch.deltaPosition.normalized;

                Debug.Log("X : " + v.x);
                Debug.Log("Y : " + v.y);

                CheckTouchDirection();

            }

            else if(touch.phase == TouchPhase.Ended)// 터치 뗐을 때
            {
                touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit2D hit;

                hit = Physics2D.Raycast(touchPos, Vector3.forward, Mathf.Infinity, 1 << 8);

                if (hit)
                {
                    hit.collider.GetComponent<NewBlock>().OnClick();
                }
            }
        }
    }

    void CheckTouchDirection()
    {
        Index index = selectBlock.index;

        float x = touch.deltaPosition.x;
        float y = touch.deltaPosition.y;

        if (Mathf.Abs(x) > 0.5f && Mathf.Abs(y) > 0.5f) return;

        if(x >= 0.5f)
        {
            if(y <= 0.2f && y >= -0.2f)
            {
                Debug.Log("Right");
                transform.Translate(Vector3.right);
            }
        }
        else if(x <= -0.5f)
        {
            if (y <= 0.2f && y >= -0.2f)
            {
                Debug.Log("Left");
                transform.Translate(-Vector3.right);
            }
        }

        if (y >= 0.5f)
        {
            if (x <= 0.2f && x >= -0.2f)
            {
                Debug.Log("Up");
                transform.Translate(Vector3.up);
            }
        }
        else if (y <= -0.5f)
        {
            if (x <= 0.2f && x >= -0.2f)
            {
                Debug.Log("Down");
                transform.Translate(-Vector3.up);
            }
        }
    }


    

   

}
