using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour{

    public Text palavra;
    // Start is called before the first frame update
    void Start(){
        palavra.text = "testesttes";
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void TestarLetras(string letra){
        if(letra == "A"){
            Debug.Log("teste");
        }
    }
}
