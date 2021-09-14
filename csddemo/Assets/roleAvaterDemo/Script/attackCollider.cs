using UnityEngine;
using System.Collections;

public class attackCollider : MonoBehaviour {
    public UCharacterController charInstance = null;
    public attcakStartEnd pAttackClass = null;

    void Start() {
       /*
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
        */
    }

    void OnTriggerStay(Collider other)
    {
        /*
        if (other == null)
            return;

        if (pAttackClass != null)
        {
            woodsetbreak wd = other.gameObject.GetComponent<woodsetbreak>();
            if (wd != null)
                //需要判断当前是否为攻击状态
            {
                if ((wd.isBroken != true) &&(pAttackClass.isInAttack))
                {
                    wd.isBroken = true; //开启破坏
                }
            }
        }
        */
    }

}
