using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuSalir;
    public GameObject menuDispositivos;
    public GameObject menuObjetos;
    private bool dispositivosShowed = false;
    public GameObject menuSimulacion;
    public GameObject menuConfig;
    private bool simulacionShowed = false;
    private string typeObjetos = "";
    private string typeConfi = "";
    public void ShowSalir(){
        menuSalir.SetActive(true);
    }

    public void closeSalir(){
        menuSalir.SetActive(false);
    }

    public void ShowCloseDispositivos(){
        dispositivosShowed = !dispositivosShowed;
        menuDispositivos.SetActive(dispositivosShowed);
    }

    public void CloseDispositivos(){
        dispositivosShowed = false;
        menuDispositivos.SetActive(false);
    }

    
    public void ShowCloseSimulacion(){
        simulacionShowed = !simulacionShowed;
        menuSimulacion.SetActive(simulacionShowed);

        if(menuSimulacion.activeSelf)
            menuSimulacion.transform.position =  Camera.main.transform.position + Camera.main.transform.forward * 0.35f; // cuanto mas grande sea el numero mas lejos
    }

    public void ShowObjetos(string type){
        menuObjetos.SetActive(true);
        if(type != ""){
            menuObjetos.transform.Find(type).gameObject.SetActive(true);
            typeObjetos = type;
        }


        //cerramos el menu dispositivos
        CloseDispositivos();
    }

    public void CloseObjetos(){
        menuObjetos.transform.Find(typeObjetos).gameObject.SetActive(false);
        menuObjetos.SetActive(false);
    }
    
    public void ShowConfig(string type, GameObject configurableObject){
        //cerramos el menu info si esta abierto
        if(GameObject.Find("MenuInfo") != null && GameObject.Find("MenuInfo").activeSelf)
            GameObject.Find("MenuInfo").SetActive(false);

        menuConfig.SetActive(true);
        if(type != ""){
            //cerramos el anterior tipo y asignamos la variable el nuevo
            if(typeConfi != "")
                menuConfig.transform.Find(typeConfi).gameObject.SetActive(false);

            menuConfig.transform.Find(type).gameObject.SetActive(true);
            typeConfi = type;
        }
            

        if(configurableObject != null){
            GameObject.Find("MenuConfig").GetComponent<ConfigurationController>().SetConfigurationObject(configurableObject);
        }
    }

    public void CloseConfig(){
        menuConfig.SetActive(false);
    }


}
