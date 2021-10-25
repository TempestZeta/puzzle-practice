//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerControl : MonoBehaviour
//{
//    public GameObject ChangeBlock;
//    bool isSelect;
//    Block selectBlock;
//    Vector2 savePos;
//    // Start is called before the first frame update
//    private void Awake()
//    {
//        ChangeBlock.SetActive(false);
//        isSelect = false;
//    }
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {

//#if UNITY_EDITOR
        
//        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        Ray ray = Camera.main.ScreenPointToRay(pos); 
//        RaycastHit2D hit;        
//        //터치 시
//        if (Input.GetMouseButtonDown(0))
//        {
//            hit = Physics2D.Raycast(pos, Vector3.forward,Mathf.Infinity,1<<8);
//            if (hit)
//            {
//                isSelect = true;
//                selectBlock = hit.collider.GetComponent<Block>();
//                selectBlock.isSelect = true;
//                selectBlock.GetComponent<Collider2D>().enabled = false;
//                savePos = selectBlock.transform.position;
//                ChangeBlock.transform.position = savePos;
//                ChangeBlock.SetActive(true);                
//            }
//        }
//        //이동 시
//        else if (Input.GetMouseButton(0))
//        {
//            if(isSelect)
//            selectBlock.transform.position = pos;
//        }
//        //놓을 시
//        else if (Input.GetMouseButtonUp(0))
//        {
//            if (isSelect)
//            {
//                isSelect = false;
//                hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity, 1 << 8);
//                selectBlock.GetComponent<Collider2D>().enabled = true;
//                if (hit)
//                {
//                    if (hit.collider.tag == "Block")
//                    {
//                        ChangeBlock.SetActive(false);
//                        Block block = hit.collider.GetComponent<Block>();
//                        int row = block.currRow;
//                        block.currRow = selectBlock.currRow;
//                        selectBlock.currRow = row;
//                        selectBlock.transform.position = hit.transform.position;
//                        hit.collider.transform.position = savePos;
//                        selectBlock.isSelect = false;
//                    }
//                }
//                else
//                {
//                    ChangeBlock.SetActive(false);
//                    selectBlock.transform.position = savePos;
//                    selectBlock.isSelect = false;
//                }
//            }
            
//        }
        
//#endif
//#if UNITY_ANDROID

//        if (Input.touchCount == 1)
//        {
//            Vector2 tPos = Input.GetTouch(0).position;
//            Ray tRay = Camera.main.ScreenPointToRay(tPos);   // 터치한 좌표 레이로 바꾸어
//            RaycastHit2D tHit;    // 정보 저장할 구조체 만들고            
            
//            //터치 시
//            if (Input.GetTouch(0).phase == TouchPhase.Began)
//            {
//                tHit = Physics2D.Raycast(tPos, Vector3.forward, Mathf.Infinity, 1 << 8);

//                if (tHit)
//                {
//                    if (tHit.collider.tag == "Block")
//                    {
//                        selectBlock = tHit.collider.GetComponent<Block>();
//                        savePos = selectBlock.transform.position;
//                    }
//                }
//            }
//            //이동 시
//            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
//            {
//                selectBlock.transform.position = tPos;
//            }
//            //놓을 시
//            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
//            {
//                tHit = Physics2D.Raycast(tPos, Vector3.forward, Mathf.Infinity, 1 << 8);

//                if (tHit)
//                {
//                    if (tHit.collider.tag == "Block")
//                    {
//                        selectBlock.transform.position = tHit.transform.position;
//                        tHit.transform.position = savePos;
//                    }
//                    else
//                    {
//                        selectBlock.transform.position = savePos;
//                    }
//                }
//            }
//        }
//#endif
//    }
//}
