using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterCollider : MonoBehaviour
{
    private UCharacterController charInstance = null;
    //private GameObject roleInstance = null;
    private monsterAniControl monControl = null;
    void Start()
    {
 
    }


    //    void OnTriggerStay(Collider other)
    void OnTriggerEnter(Collider other)
    {
        
        if (other == null)
            return;

        if (charInstance == null)
        {
            charInstance = App.Game.character;
        }

        if (monControl == null)
        {
            monControl = this.transform.GetComponent<monsterAniControl>();
        }

        if (monControl.IsDie() == false)
        {
            if (monControl.IsInAttackState() == false)
            {
                monControl.setMonsterToAttack(charInstance.roleInstance.transform.position);
            }
        }
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
            monControl = this.transform.GetComponent<monsterAniControl>();
        }

        if (monControl.IsDie() == false)
        {
            if (monControl.IsInAttackState() == true)
            {
                monControl.setMonsterToStand();
            }
        }
    }
}
