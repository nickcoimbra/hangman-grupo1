//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//
//public class HiddenMenu : MonoBehaviour
//{
//
//    private string[] cheatCode;
//    private int index;
//    private bool cheat;
//    private bl_GameManager wordCreator;
//
//    void OnGUI()
//    {
//        if (cheat)
//        {
//            // draw the hidden menu
//            GUI.Box(new Rect(20, 80, 240, 240), "Cheat Menu :-) ");
//            if (GUI.Button(new Rect(40, 120, 200, 20), "Criar Palavra "))
//            {
//                wordCreator.OpenWordCreatorPanel();
//            }
//            if (GUI.Button(new Rect(40, 270, 200, 20), "Close this menu"))
//            {
//                cheat = false;
//                index = 0;
//            }
//        }
//    }
//
//    void Start()
//    {
//        // The pass Code is "secret", user needs to input 
//        // this word to show the hidden menu
//        // You can change it whatever you want
//        cheatCode = new string[] {
//   "g",
//   "r",
//   "u",
//   "p",
//   "o",
//   "1"
//  };
//        index = 0;
//    }
//
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            Application.Quit();
//        }
//
//        if (Input.anyKeyDown)
//        {
//            if (Input.GetKeyDown(cheatCode[index]))
//            {
//                // right input, check next digit
//                index++;
//            }
//            else
//            {
//                // wrong input, restart from index 0
//                index = 0;
//            }
//        }
//
//        if (index == cheatCode.Length)
//        {
//            // cheat is ok, the hidden menu is now visible :-)
//            cheat = true;
//            index = 0;
//        }
//    }
//}