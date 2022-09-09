using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SingleCase;
using cfg.Config;

public class BagUI : MonoBehaviour
{
    public Transform content;
    // Start is called before the first frame update
    public  Transform gHeadTmp;
    public Transform clickQuan;


    private List<Button> troopsHeadList = new List<Button>();
    private Transform upBtn;
    private Transform downBtn;
    private int clickId;

    void Awake()
    {
        transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(OnBackClick);
        upBtn = transform.Find("upBtn");
        downBtn = transform.Find("downBtn");

        upBtn.GetComponent<Button>().onClick.AddListener(OnUpLineClick);
        downBtn.GetComponent<Button>().onClick.AddListener(OnOffLineClick);

        content.DetachChildren();
    }

    void OnEnable()
    {
        InitList();
    }

    private void OnBackClick()
    {
        transform.gameObject.SetActive(false);
    }

    private void InitList()
    {
        List<int> list = GameModel.Instance().generalList;

        int defaultId = 0;

        foreach(int id in list)
        {
            Transform obj= content.Find(id.ToString());
            if (obj == null)
            {
                TroopsCfg cfg = Configs.GetTroopsCfgById(id);
                obj = Instantiate(gHeadTmp);
                ResTools.SetImage("HeadImg/" + id, obj.GetComponent<Image>());
                obj.Find("name").GetComponent<Text>().text = cfg.Name;
                obj.parent = content;
                obj.name = id.ToString();
            }

            bool ifOn = GameModel.Instance().CheckTroopOn(id);
            obj.Find("on").gameObject.SetActive(ifOn);

            obj.gameObject.SetActive(true);
            troopsHeadList.Add(obj.GetComponent<Button>());

            if(defaultId == 0)
            {
                defaultId = id;
            }


        }

        foreach (Button btn in troopsHeadList)
        {
            int id = int.Parse(btn.transform.name);
            btn.onClick.AddListener(delegate ()
            {
                OnHeadClick(id);
            });
        }

        clickId = defaultId;
        clickQuan.parent = content.Find(clickId.ToString());
        clickQuan.localPosition = new Vector3(0, 0, 0);
        bool ret = GameModel.Instance().CheckTroopOn(clickId);
        upBtn.gameObject.SetActive(!ret);
        downBtn.gameObject.SetActive(ret);
    }

    private void OnHeadClick(int id)
    {
        clickId = id;
        Transform btn = content.Find(id.ToString());
        clickQuan.parent = btn;
        clickQuan.localPosition = new Vector3(0, 0, 0);

        bool ifOnline = GameModel.Instance().CheckTroopOn(int.Parse(btn.name));

        upBtn.gameObject.SetActive(!ifOnline);
        downBtn.gameObject.SetActive(ifOnline);
    }

    private void OnUpLineClick()
    {
        bool ret = GameModel.Instance().SetTroopsOn(clickId);
        upBtn.gameObject.SetActive(!ret);
        downBtn.gameObject.SetActive(ret);
        if (!ret)
        {
            UIManager.ShowToast("上阵达到上限");
        } else
        {
            Transform UpFlag = content.Find(clickId.ToString()).Find("on");
            UpFlag.gameObject.SetActive(true);
        }
    }

    private void OnOffLineClick()
    {
        bool ret = GameModel.Instance().SetTroopsOff(clickId);
        upBtn.gameObject.SetActive(ret);
        downBtn.gameObject.SetActive(!ret);
        if (!ret)
        {
            UIManager.ShowToast("未找到改上阵队伍");
        }
        else
        {
            Transform UpFlag = content.Find(clickId.ToString()).Find("on");
            UpFlag.gameObject.SetActive(false);
        }
    }
}
