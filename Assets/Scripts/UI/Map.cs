using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Constant;
using GameFSM;
using cfg.Config;
using SingleCase;

public class Map : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    //�ƶ����
    public Camera gameCamera;
    public GameObject selectUnit;
    public GameObject sumTowers;
    public GameObject sumPaces;
    public Transform sumTroops;
    public MainUI mainUI;

    //�������Ϻ�
    public Material yellowMaterials;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material purpleMaterial;
    public Material redMaterial;
    public Material grayMaterial;

    private bool _ifScale = false;
    private Vector3 _cachePos;
    private bool _isMoving = false;

    private readonly Vector3 CAMERA_SCALE_POS = new Vector3(51, 91, -49);

    void Start()
    {
        selectUnit.SetActive(false);
        Init();
    }

    private void Awake()
    {
        Debug.Log("Map Awake");
        Watcher.On<int  , int , int>(EventCmd.Move_Cmd, OnTroopsMove);
        Watcher.On<int, TowerStatus, int>(EventCmd.Tower_Status_Cmd, OnTowerStatusTurn);
        Watcher.On<GraphPosConfig>(EventCmd.Game_Config_inited, OnTowerInit);
        Watcher.On<int , int>(EventCmd.Map_Troops_Delete, OnTroopsDelete);
    }

    private void OnDestroy()
    {
        Debug.Log("Map OnDestroy");
        Watcher.Off<int, int , int>(EventCmd.Move_Cmd , OnTroopsMove);
        Watcher.Off<int, TowerStatus, int>(EventCmd.Tower_Status_Cmd, OnTowerStatusTurn);
        Watcher.Off<GraphPosConfig>(EventCmd.Game_Config_inited, OnTowerInit);
        Watcher.Off<int, int>(EventCmd.Map_Troops_Delete, OnTroopsDelete);
    }

    private void Init()
    {
        gameCamera.transform.position = new Vector3(25, 22, -11.8f);


    }

    // Update is called once per frame
    void Update()
    {
        OnCameraMove();
    }

    //������ƶ�
    private void OnCameraMove()
    {
        
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
			HandleTouchInput();
#else
            HandleMouseInput();
#endif
        return;
        //����
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_ifScale)
            {
                _ifScale = true;
                _cachePos = gameCamera.transform.position;
                gameCamera.transform.position = CAMERA_SCALE_POS;
            }
            else
            {
                _ifScale = false;
                gameCamera.transform.position = _cachePos;
                _cachePos = new Vector3();
            }
        }

        if (_ifScale) return;

        if (Input.GetKeyDown(KeyCode.W) && gameCamera.transform.position.z < -11.8f)
        {
            gameCamera.transform.Translate(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.S) && gameCamera.transform.position.z > -88f)
        {
            gameCamera. transform.Translate(Vector3.down);
        }
        if (Input.GetKeyDown(KeyCode.A) && gameCamera.transform.position.x >25f)
        {
            gameCamera.transform.Translate(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.D) && gameCamera.transform.position.x < 75f)
        {
            gameCamera.transform.Translate(Vector3.right);
        }

        //�����
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("���������");
            //OnPlayerMove(clickPos);
        }
    }

    //��ͼ���
    public void OnMapPointClick()
    {
        Debug.Log("��һ�µ��������");

    }

    //��ɫ��ͼ�ƶ� no use
    private void OnPlayerMove(Vector3 targetPos)
    {
        if (!selectUnit || _isMoving) return;
        _isMoving = true;
        Vector3 curPos = selectUnit.transform.position;

        //y���ǿ��Բ��Ƚϵ�
        while(Mathf.Abs(curPos.x - targetPos.x) > 0.5 || Mathf.Abs(curPos.y - targetPos.y) > 0.5 || Mathf.Abs(curPos.z - targetPos.z) > 0.5)
        {
            Vector3 localPos = selectUnit.transform.position;
            if (Mathf.Abs(localPos.x - targetPos.x) > Mathf.Abs(localPos.z - targetPos.z))
            {
                if(localPos.x > targetPos.x)
                {
                    float tc = 1f;
                    DOTween.To(() => tc, a => tc = a, 1f, 1f).OnComplete(() =>
                    {
                        selectUnit.transform.Translate(Vector3.left);
                    });
                }
                else
                {
                    float tc = 1f;
                    DOTween.To(() => tc, a => tc = a, 1f, 1f).OnComplete(() =>
                    {
                        selectUnit.transform.Translate(Vector3.right);
                    });
                }
            } else
            {
                if (localPos.z > targetPos.z)
                {
                    float tc = 1f;
                    DOTween.To(() => tc, a => tc = a, 1f, 1f).OnComplete(() =>
                    {
                        selectUnit.transform.Translate(Vector3.down);
                    });
                }
                else
                {
                    float tc = 1f;
                    DOTween.To(() => tc, a => tc = a, 1f, 1f).OnComplete(() =>
                    {
                        selectUnit.transform.Translate(Vector3.up);
                    });
                }
            }

            ReachCheck(localPos , targetPos);
        }
    }

    //no use
    private void ReachCheck(Vector3 pp , Vector3 tp)
    {
        bool ifReach = Mathf.Abs(pp.x - tp.x) <= 0.5 || Mathf.Abs(pp.y - tp.y) <= 0.5 || Mathf.Abs(pp.z - tp.z) <= 0.5;
        if (ifReach)
        {

            _isMoving = false;
        }
    }



    //ѡ�нڵ�
    public void OnTowerClick()
    {
        Debug.Log("�ֶ����");
    }

    public void OnPointerDown(PointerEventData   eventData )
    {
        //�ȹر�
        //return;
        Debug.Log("��Ҫ����Ӧ����" );
        GameObject clickObj = eventData.pointerEnter;

        if (clickObj == null || clickObj.transform.parent == null || clickObj.transform.parent.name != sumTowers.name) {

            return;
        };

        if(clickObj != null)
        {
            //Vector3 startPos = GetTowerPositionById("1");
            //selectUnit.transform.position = startPos;
            //int startIdx = selectUnit.GetComponent<Troops>().targetArea;
            int targetIdx = int.Parse(clickObj.name);

            int troopsId = mainUI.GetComponent<MainUI>().selectId;
            if (troopsId == 0)
            {
                UIManager.ShowToast("��ǰû�����󲿶�");
                return;
            };
            bool ret = Common.gameCtrl.SetTroopsMove(troopsId , targetIdx);
            Debug.Log("�ƶ�������" + ret);
            /**
                List<int> paceList = SingleCase.GraphPoint.Instance().SearchShortestPath(startIdx, targetIdx);

                if (paceList.Count <= 0) return;

                List<Vector3> posList = new List<Vector3> ();
                for(int ii  = 0 ; ii < paceList.Count ; ii++)
                {
                    posList.Add(GetTowerPositionById(paceList[ii].ToString()));
                }
                posList.RemoveAt(0);
                selectUnit.GetComponent<Troops>().MoveTo(posList , targetIdx);

                Debug.Log(eventData.pointerEnter.name);
            */
        }
    }

    public void SetTowerColor(int towerId  , TowerColor c)
    {
        Transform retNode = sumTowers.transform.Find(towerId.ToString());
        switch(c)
        {
            case TowerColor.YELLOW:
                retNode.GetComponent<MeshRenderer>().material = yellowMaterials;
                break;
            case TowerColor.BLUE:
                retNode.GetComponent<MeshRenderer>().material = blueMaterial;
                break;
            case TowerColor.GREEN:
                retNode.GetComponent<MeshRenderer>().material = greenMaterial;
                break;
            case TowerColor.PURPLE:
                retNode.GetComponent<MeshRenderer>().material = purpleMaterial;
                break;
            case TowerColor.RED:
                retNode.GetComponent<MeshRenderer>().material = redMaterial;
                break;
            case TowerColor.GRAY :
            default:
                retNode.GetComponent<MeshRenderer>().material = grayMaterial;
                break;
        }
    }

    //���մ������ƶ����¼�
    private void OnTroopsMove(int camp , int id  ,  int target)
    {
        Transform troop = sumTroops.Find(id.ToString());
        //Ĭ�ϵ�һ���ƶ������λ��
        if(troop == null)
        {

            Vector3 startPos = GetTowerPositionById(ConstData.Move_Command_Arr[camp][0].ToString());
            troop = Instantiate(selectUnit , selectUnit.transform.position, selectUnit.transform.rotation , sumTroops).transform;
            troop.name = id.ToString();
            troop.transform.localPosition = startPos;
            //troop.position = startPos;
            //troop.parent = sumTroops.transform;
            troop.GetComponent<Troops>().InitData(camp , id , true , this);
            troop.GetComponent<Troops>().targetArea = ConstData.Move_Command_Arr[camp][0];
            troop.gameObject.SetActive(true);



            //SetTowerColor(target , (TowerColor)camp);
            //Common.gameCtrl.TurnTroopStatus(id, TroopStatus.FREE);
            //return;
        }

        int startIdx = troop.GetComponent<Troops>().targetArea;
        List<int> paceList = SingleCase.GraphPoint.Instance().SearchShortestPath(startIdx, target);

        if (paceList.Count <= 0)
        {
            Common.gameCtrl.TurnTroopStatus(id, TroopStatus.FREE);
            return;
        };

        List<Vector3> posList = new List<Vector3>();
        for (int ii = 0; ii < paceList.Count; ii++)
        {
            posList.Add(GetTowerPositionById(paceList[ii].ToString()));
        }
        posList.RemoveAt(0);
        troop.GetComponent<Troops>().MoveTo(posList, target);
    }

    //�ڵ�״̬�ı�
    private void OnTowerStatusTurn(int tId , TowerStatus sta , int belongs)
    {
        SetTowerColor(tId, (TowerColor)belongs);
    }

    private void OnTowerInit(GraphPosConfig cfgs)
    {
        foreach(GraphPosCfg cc in cfgs.DataList)
        {
            Transform node = sumTowers.transform.Find(cc.Id.ToString());
            node.Find("name").GetComponent<TextMesh>().text = cc.Name;
            SetTowerColor(cc.Id , (TowerColor)cc.Camp);
        }

        //��Ϸ��ʼ����
        //��ʱ1s
        float tc = 1f;
        DOTween.To(() => tc, a => tc = a, 1f, 1f).OnComplete(() =>
        {
            Common.gameCtrl.Resume();
        });
    }

    //��ȡָ���ڵ�����
    public Vector3 GetTowerPositionById(string Id)
    {
        Transform retNode = sumTowers.transform.Find(Id);
        if (retNode)
        {
            return retNode.transform.localPosition;
        }
        return new Vector3(0, 0, 0);
    }

    //ͨ�������ȡ�ڵ㣬Ҫ���������߶�׼ȷ
    public int GetTowerIdByPosition(Vector3 pos)
    {

        int childCount = sumTowers.transform.childCount;
        for(int ii = 1 ; ii < childCount + 1; ii++ )
        {
            Transform child = sumTowers.transform.Find(ii.ToString());
            if (!child) continue;

            if(child.localPosition == pos)
            {
                return ii;
            }
        }

        //�Ҳ���
        return -1;
    }


    private Vector3 m_prevPosition;
    private int m_minCamYPos = -50;
    private int m_maxCamYPos = 26;
    private float m_cameraScaleVal = 0.1f;
    private int m_minCamXPos = 0;
    private int m_maxCamXPos = 50;

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                m_prevPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 curPosition = touch.position;
                MoveCamera(m_prevPosition, curPosition);
                m_prevPosition = curPosition;
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_prevPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 curMousePosition = Input.mousePosition;
            MoveCamera(m_prevPosition, curMousePosition);
            m_prevPosition = curMousePosition;
        }
    }

    private void MoveCamera(Vector2 prevPosition, Vector2 curPosition)
    {
        //ע�������myCamera.nearClipPlaen��������ʹ�õ���͸�������������Ҫ��zֵ��Ϊ���
        //�������ʹ�õ���������������ܲ���Ҫ���
        Vector3 offset = (gameCamera.ScreenToWorldPoint(new Vector3(curPosition.x, 0.8f, prevPosition.y)) - gameCamera.ScreenToWorldPoint(new Vector3(prevPosition.x, 0.8f , curPosition.y))) ;
        //�����m_cameraScale,��Ϊ�Ҳ����޸�nearClipPlaen��ֵ���ﵽ�ƶ��Ŀ��������Լ��˸��ƶ�����
        Vector3 newPos = new Vector3(transform.localPosition.x + offset.x * m_cameraScaleVal, transform.localPosition.y + offset.y * m_cameraScaleVal, transform.localPosition.z + offset.y * m_cameraScaleVal);
        newPos.z = Mathf.Clamp(newPos.z, m_minCamYPos, m_maxCamYPos);
        newPos.x = Mathf.Clamp(newPos.x, m_minCamXPos, m_maxCamXPos);
        transform.position = new Vector3(newPos.x, transform.position.y , newPos.z);
    }

    private void OnTroopsDelete(int camp , int id)
    {
        Transform delTroop = sumTroops.Find(id.ToString());
        if (delTroop == null) return;
        Destroy(delTroop.gameObject);
    }
}
