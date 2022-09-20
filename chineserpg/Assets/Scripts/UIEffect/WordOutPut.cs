using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordOutPut : MonoBehaviour
{
    private const float cfCharsPerSecond = 0.05f;
    public float charsPerSecond;//打字时间间隔
    private string words;//保存需要显示的文字

    private bool isActive = false; //判断是否开始输出
    private bool isShowFinish = false;
    private float timer;//计时器
    private Text myText;//获取身上的test脚本
    private int currentPos = 0;//当前打字位置
    public bool Enabled { get; set; }

    // Use this for initialization


    private void Start()
    {
        Debug.LogWarning("WordOutPut start");
    }

    public void setTextEffectSpeed(float lCharsPerSecond)
    {
        charsPerSecond = lCharsPerSecond;
        //OnEnable();
    }

    public void setContext(string ltext) {
        timer = 0;
        isActive = true;
        isShowFinish = false;
        charsPerSecond = cfCharsPerSecond;
        charsPerSecond = Mathf.Max(0.02f, charsPerSecond); //将最小的出字速度限制为0.02，也可以自行调整
        words = ltext;
        if(myText == null)
            myText = GetComponent<Text>();
        myText.text = "";
    }
    /*
    private void OnEnable()
    {
        Debug.Log("wordoutput:onEnable");
        timer = 0;
        isActive = true;
        isShowFinish = false;
        charsPerSecond = Mathf.Max(0.02f, charsPerSecond); //将最小的出字速度限制为0.02，也可以自行调整
        myText = GetComponent<Text>();
        words = myText.text;
        myText.text = "";//获取Text的文本信息，保存到words中，然后动态更新文本显示内容，实现打字机的效果
    }
    */

    private void OnDisable()
    {
        Debug.Log("wordoutput:OnDisable");
        OnFinish();//当脚本在失活的时候，将数据进行重置
    }
    // Update is called once per frame
    private void Update()
    {
        OnStartWriter();
        //Debug.Log (isActive);
    }
    /// <summary>
    /// 执行打字任务
    /// </summary>
    private void OnStartWriter()
    {

        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)//判断计时器时间是否到达
            {
                timer = 0;
                currentPos++;

                //这里其实还可以做一个改良，可以检测一个input用户输入，如果输入了，则让currentPos = words.Length，这样可以实现按下按键，马上就显示完毕

                myText.text = words.Substring(0, currentPos);//刷新文本显示内容

                if (currentPos >= words.Length)
                {
                    OnFinish();
                    isShowFinish = true; //显示完成
                }
            }

        }
    }
    /// <summary>
    /// 结束打字，初始化数据
    /// </summary>
    private void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        if(isShowFinish == false)
            myText.text = words;
    }
}