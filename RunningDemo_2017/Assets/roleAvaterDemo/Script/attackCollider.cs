using UnityEngine;
using System.Collections;

public class attackCollider : MonoBehaviour {
    public UCharacterController charInstance = null;
    public attcakStartEnd pAttackClass = null;

    void Start() {
        //roleInstance = null;
     //   Debug.LogWarning("attackCollider start");
    }

    void OnTriggerEnter(Collider other)
    {

        if (charInstance == null)
        {
            charInstance = App.Game.character;
        }

        if (charInstance != null)
        {
            if (pAttackClass == null)
            {
                pAttackClass = charInstance.roleInstance.GetComponent<attcakStartEnd>();
            }
        }

        if (pAttackClass != null)
        {
            //需要判断当前是否为攻击状态
            if (pAttackClass.isInAttack)
            {
                woodsetbreak wd = other.gameObject.GetComponent<woodsetbreak>();
                if (wd != null)
                {
                    wd.isBroken = true; //开启破坏
                }
            }
        }
        /*
        if (charInstance.isInAttack()) {
                woodsetbreak wd = other.gameObject.GetComponent<woodsetbreak>();
                if (wd != null)
                {
                    wd.isBroken = true;
                }
            }
        }
        */
    }

}
