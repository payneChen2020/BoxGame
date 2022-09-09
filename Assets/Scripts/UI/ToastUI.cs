using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastUI : MonoBehaviour
{
    // Start is called before the first frame update
    public  Text toastTxt;

    void Start()
    {
    }

    public void Show(string t)
    {
        toastTxt.text = t;
        Destroy(gameObject , 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
