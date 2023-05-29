using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;


public class InfoController : MonoBehaviour
{
    public GameObject menuInfo;
    public TextMeshProUGUI textoInfo;
    public TextMeshProUGUI tituloInfo;
    private string json;
    private Info infoFile;

    //Clase para convertir de json a objeto
    [Serializable]
    public class Info
    {
        public string nombre;
        public string descripcion;
    }
    
    // mostrar el menu y monstrar la informacion segun el dispostivo
    public void showInfo(string pathInfo){
        //cerramos el menu confi si esta abierto para que no esten a la vez
        if(GameObject.Find("MenuConfig") != null && GameObject.Find("MenuConfig").activeSelf){
            GameObject.Find("MenuConfig").SetActive(false);
        }


        menuInfo.SetActive(true);
        TextAsset targetFile = Resources.Load<TextAsset>("Info/"+pathInfo);
        json = targetFile.text;
        infoFile = JsonUtility.FromJson<Info>(json);
        tituloInfo.text = infoFile.nombre;
        textoInfo.text = infoFile.descripcion;

    }

    public void closeInfo(){
        tituloInfo.text = "";
        textoInfo.text = "";
        menuInfo.SetActive(false);
    }
}
