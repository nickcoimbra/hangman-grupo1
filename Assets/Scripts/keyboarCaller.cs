using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboarCaller : MonoBehaviour
{
    // Start is called before the first frame update
     void callKeyboard()
    {
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
            Debug.Log("Teclado ADM");
    }
}
