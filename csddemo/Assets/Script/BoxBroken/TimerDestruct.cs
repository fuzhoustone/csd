using System.Linq;
//using Assets.Script.Engine;
using UnityEngine;

public class TimerDestruct : MonoBehaviour
{
    public float m_timer = 1;
    private float m_remainTime = float.MaxValue;

    private bool m_disabled = true;
    public GameObject oldObj;
 //   public GameObject newObj;
    public GameObject parAct;



    public float Speed { get; set; }

    public TimerDestruct()
    {
        Speed = 1;
    }

    void Start()
    {
    }

    public void setInit() {
        oldObj.SetActive(true);
      //  newObj.SetActive(false);
        parAct.SetActive(false);
    }

    public void setEnable()
    {
        if (m_disabled == false) {
            return ;
        }

        m_disabled = false;
        /*
        if (m_timer <= 0)
        {
            // Util.EditorDebugModeLog("LifeTime is less then zero");
            //GameObjectPool.Destroy
            //GameObject.Destroy(gameObject);
            
            m_disabled = true;
            return;
        }
        */
        m_remainTime = m_timer;
        m_disabled = false;

        parAct.SetActive(true);
    }

    void OnDisable()
    {
        m_remainTime = m_timer;
        m_disabled = true;
    }

    void Update()
    {
        if (m_disabled == true) return;

        //m_remainTime -= Engine.Instance.DeltaTime*Speed;
        m_remainTime -= Time.deltaTime;
        if (m_remainTime <= 0)
        {
            //GameObjectPool.Destroy(gameObject);
            //GameObject.Destroy(gameObject);
            m_disabled = true;
            oldObj.SetActive(false);
        //    newObj.SetActive(true);
        }
    }
}
