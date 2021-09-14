using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterCollider : MonoBehaviour
{
    private UCharacterController charInstance = null;
    //private GameObject roleInstance = null;
    private IbaseANI monControl = null;
    void Start()
    {
 
    }


    //    void OnTriggerStay(Collider other)
    void OnTriggerEnter(Collider other) //需区分碰撞的对象，限制为敌人才有反应
    {
        
        if (other == null)
            return;

        if (charInstance == null)
        {
            charInstance = App.Game.character;
        }

        if (monControl == null)
        {
            monControl = this.transform.GetComponent<IbaseANI>();
        }
        /*
        if (monControl.IsDie() == false)
        {
           
            //判断目标是否存活
            //双方的enemy加上对方, 双方显示血条
      
            if (monControl.IsInAttackState() == false)
            {
                monControl.setToAttack(charInstance.roleInstance.transform.position);
            }
           
        }*/
    }

    void OnTriggerExit(Collider other)
    {

        if (other == null)
            return;

        if (charInstance == null)
        {
            charInstance = App.Game.character;
        }

        if (monControl == null)
        {
            monControl = this.transform.GetComponent<IbaseANI>();
        }
        /*
        if (monControl.IsDie() == false)
        {
            if (monControl.IsInAttackState() == true)
            {
                monControl.setToStand();
            }
        }
        */
    }
}
