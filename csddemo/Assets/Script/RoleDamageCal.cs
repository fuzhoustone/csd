using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DamageCal
{
    public class RoleDamageCal
    {
        private static RoleDamageCal _instance = null;

        public static RoleDamageCal instance {
            get {
                if (_instance == null) {
                    _instance = new RoleDamageCal();
                }

                return _instance;
            }
        }
        //计算伤害
        public int DamageCal(roleProperty AttPro, roleProperty DefPro)
        {
            int damage = 1;
            float tmpAttack = AttPro.attack - DefPro.def;
            if (tmpAttack < 1.0f)
                tmpAttack = 1.0f;
            float tmpRes = attribRestraint(AttPro.element, DefPro.element);
            float res = tmpAttack * tmpRes;

            damage = Mathf.FloorToInt(res); //向下取整

            return damage;
        }

        private const int ciEleNull = 0;
        private const int ciEleFire = 1;
        private const int ciEleWind = 2;
        private const int ciEleWater = 3;
        private const int ciEleEarth = 4;
        private const int ciEleLight = 5;
        private const int ciEleDark = 6;
        private float attribRestraint(int attEle, int defEle) {
            float res = 1.0f;
            if (attEle == ciEleNull) //无属性不受克制关系
                res = 1.0f;
            else if ( ((attEle == ciEleWind) && (defEle == ciEleEarth))  //风克土
                  ||((attEle == ciEleEarth) && (defEle == ciEleWater)) //  土克水，
                  || ((attEle == ciEleWater) && (defEle == ciEleFire)) //水克火，
                  || ((attEle == ciEleFire) && (defEle == ciEleWind)) //火克风
                  || ((attEle == ciEleLight) && (defEle == ciEleLight)) //圣克暗，
                    )
                    res = 1.3f;  //克制伤害+30%
            else if (((defEle == ciEleWind) && (attEle == ciEleEarth))  //风克土
                  || ((defEle == ciEleEarth) && (attEle == ciEleWater)) //  土克水，
                  || ((defEle == ciEleWater) && (attEle == ciEleFire)) //水克火，
                  || ((defEle == ciEleFire) && (attEle == ciEleWind)) //火克风
                  || ((defEle == ciEleLight) && (attEle == ciEleLight)) //圣克暗，
                    )
                res = 0.7f;

            return res;
        }


    }
}
