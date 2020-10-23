//using System.Collections;
//using System.Collections.Generic;
//using System.Security.Permissions;
//using UnityEngine;
//
//public class StoreController : MonoBehaviour
//{
//    public bl_ScoreManager score;
//    public Timer timer;
//    public bl_GameManager trys;
//    // TEMPO
//        public void BuyItem1(int cost)
//        {
//            score.CurrentScore = cost;
//            if (GameInformation.TotalPots >= cost)
//            {
//                GameInformation.TotalPots -= cost;
//                GameInformation.timer.timeRemaing += 30;
//                rewardIndicator.GetComponent<Text>().text = "Completado";
//                rewardIndicator.SetActive(true);
//                rewardIndicator.GetComponent<Animation>().Play();
//            }
//            else
//            {
//                rewardIndicator.GetComponent<Text>().text = "Sem pontos";
//                rewardIndicator.SetActive(true);
//            }
//        }
//        // PONTOS
//        public void BuyItem2(int cost)
//        {
//            score.CurrentScore = cost;
//            if (GameInformation.TotalPots >= cost)
//            {
//                GameInformation.TotalPots -= cost;
//                GameInformation.score.CurrentScore += 1000;
//                rewardIndicator.GetComponent<Text>().text = "Completado";
//                rewardIndicator.SetActive(true);
//                rewardIndicator.GetComponent<Animation>().Play();
//            }
//            else
//            {
//                rewardIndicator.GetComponent<Text>().text = "Sem pontos";
//                rewardIndicator.SetActive(true);
//            }
//        }
//        // VIDA
//        public void BuyItem3(int cost)
//        {
//            score.CurrentScore = cost;
//            if (GameInformation.TotalPots >= cost)
//            {
//                GameInformation.TotalPots -= cost;
//                GameInformation.trys.Trys += 1;
//                rewardIndicator.GetComponent<Text>().text = "Completado";
//                rewardIndicator.SetActive(true);
//                rewardIndicator.GetComponent<Animation>().Play();
//            }
//            else
//            {
//                rewardIndicator.GetComponent<Text>().text = "Sem pontos";
//                rewardIndicator.SetActive(true);
//            }
//        }
//}
//