using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    public Image load1;//ͼƬ������
    public Slider load2;//Slider������
    public Text valueText;//�ı�ֵ	
    float timer = 0f;//��0��ʼ	



    public void SetValue(float value)
    {
        //load1.fillAmount = value;
        load2.value = value;
        //int a = (int)(value * 100);
        //valueText.text = a + "%";
    }
}
