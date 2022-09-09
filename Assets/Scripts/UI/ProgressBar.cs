using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    public Image load1;//图片进度条
    public Slider load2;//Slider进度条
    public Text valueText;//文本值	
    float timer = 0f;//从0开始	



    public void SetValue(float value)
    {
        //load1.fillAmount = value;
        load2.value = value;
        //int a = (int)(value * 100);
        //valueText.text = a + "%";
    }
}
