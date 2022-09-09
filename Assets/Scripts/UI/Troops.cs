using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using GameFSM;
using SingleCase;

public class Troops : MonoBehaviour
{
    public SpriteRenderer img;
    
    private readonly Vector3 startPos = new Vector3(4.77f,0.2f,-4.66f);
    private readonly Vector3 midPos = new Vector3(94, 0.9f, -3);
    private readonly Vector3 mid2Pos = new Vector3(94, 0.9f, -94);
    private readonly Vector3 endPos = new Vector3(3, 0.9f, 94);

    private List<Vector3> posList = new List<Vector3> { };

    private bool ifInitVec = false;
    private Vector3 deltaPos = new Vector3();
    public bool ifNPC { get; private set; }
    public int targetArea { get; set; }
    private float speed = 2.0f;

    private Map mapCtrl;
    public int Id { get; private set; }
    public int belongs { get; private set; }

    private int mileStone = 0;
    // Start is called before the first frame update
    void Start()
    {
        //transform.localPosition = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        //return;

        //float dt = 1.0f / 60.0f;
        //timeCount += dt;
        //if (timeCount < 1.0f) return;
        //timeCount = 0.0f;
        Move();

        //transform.Translate(new Vector3(1 , 0 , -1) * Time.deltaTime , Space.World);
    }

    //初始化id跟外观
    public void InitData(int campid ,  int id , bool ifnpc , Map map)
    {
        belongs = campid;
        Id = id;
        ifNPC = ifnpc;
        mapCtrl = map;

        // transform.Find("img").GetComponent<Sprite>
        ResTools.SetSprite("HeadImg/" + id , img);
    }

    //移动
    private void Move()
    {
        if(posList.Count <= 0)
        {
            mileStone = 0;
        }

        
        if(mileStone == 0)
        {
            return;
        }
        else if ( mileStone == 1 )
        {
            Vector3 curPos = transform.localPosition;
            //抵达节点
            if ( Mathf.Abs(curPos.x - posList[0].x) <= Time.deltaTime && Mathf.Abs(curPos.z - posList[0].z) <= Time.deltaTime)
            {
                Vector3 removePos = posList[0];
                int towerId = mapCtrl.GetTowerIdByPosition(removePos);

                if(Common.gameCtrl.AttackTower(belongs , Id, towerId))
                {
                    posList.RemoveAt(0);
                    ifInitVec = false;
                    
                }

                return;
            }

            if (ifInitVec)
            {
                transform.Translate(deltaPos * Time.deltaTime * speed, Space.Self);
                return;
            };

            

            deltaPos = new Vector3(curPos.x - posList[0].x, 0, curPos.z - posList[0].z ).normalized;

            ifInitVec = true;
        }
    }

    public void OnClick()
    {
        Debug.Log("可以点击");
    }

    public void MoveTo(List<Vector3> list , int target)
    {
        posList.AddRange(list);
        targetArea = target;
        mileStone = 1;
    }
}
