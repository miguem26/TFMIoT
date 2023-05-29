using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
      private bool cooldown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // cooldown para el boton
    void ResetCooldown(){
        cooldown = false;
    }

    //Create the object
    public void CreateObject(string path){
        // para que no se clique varias veces el boton
        if ( cooldown == false ) {
            // Para que spawnee delante sumamos el vector forward de la camara
            GameObject objectCreated = Instantiate(Resources.Load("Prefabs/"+path)) as GameObject;
            objectCreated.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;

            //metemos el objeto en el array de simulacion
            GameObject.Find("SimulationController").GetComponent<SimulationController>().AddObject(objectCreated);

            // Ocultamos el menu de dispositivos
            GameObject.Find("MenuController").GetComponent<MenuController>().CloseObjetos();
            Invoke("ResetCooldown",5.0f);
            cooldown = true;
        }

    }
    

    //Delete the object
    public void DeleteObject(){
        GameObject.Find("SimulationController").GetComponent<SimulationController>().RemoveObject(this.gameObject);
        Destroy(this.gameObject);
    }

    //Shows the info (Hacemos esto desde aqui ya que es un prefab y no puede tener un objeto que no sea del gameobject)
    public void showInfo(string path){
        GameObject.Find("InfoController").GetComponent<InfoController>().showInfo(path);
    }

    
    //Shows the Config (Hacemos esto desde aqui ya que es un prefab y no puede tener un objeto que no sea del gameobject)
    public void showConfig(string type){
        GameObject.Find("MenuController").GetComponent<MenuController>().ShowConfig(type, this.gameObject);
    }

}
