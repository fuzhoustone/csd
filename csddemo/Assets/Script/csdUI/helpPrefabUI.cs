using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpPrefabUI : MonoBehaviour
{
    // Start is called before the first frame update

    public void UIShow() {
        gameObject.SetActive(true);
    }

    public void UIClose() {
        gameObject.SetActive(false);
        //gameObject.active = false;
    }
}
