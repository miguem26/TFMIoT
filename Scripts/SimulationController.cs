using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

// tiene todas las variables del menu de simulacion y un array con todos los objetos actuales en la escena
// dependiendo de sus configuraciones 
public class SimulationController : MonoBehaviour
{
    public int dayState = 0; //0 mañana 1 tarde y 2 noche
    public int tempAct = 22;
    public int tempDestFrio = 22;// dispositvos de frio como aire acondicionado
    public int tempDestCalor = 22; // dispositivos de calor como caldera
    public bool checkMovimiento = false;
    public bool checkCalidad = false;
    public bool checkGases = false;

  
    public GameObject menuConfig;
    public GameObject menuSim;
    public List<GameObject> listActiveObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // cooldown para el boton

    // Update is called once per frame
    void Update()
    {

        // se recorrera la lista de objetos y se aplicaran las opciones de simulacion de estos, se puede reducir el numero de veces que se invoca update para mejor rendimiento
        for (int x = 0; x < listActiveObjects.Count; x++)
        {
            if(listActiveObjects[x].GetComponent<Light>() != null) // si tiene componente luz sabemos que es una luz
                ApplyLightConfiguration(listActiveObjects[x]);

            if(listActiveObjects[x].GetComponent<ControlTempConfiguration>() != null){
                if ( listActiveObjects[x].GetComponent<ControlTempConfiguration>().cooldown == false ){  // Para mejorar la optimizacion se ejecutara cada 5 segundos
                    ApplyTempConfiguration(listActiveObjects[x]);
                    ActualizarTemp(listActiveObjects[x]);
                    listActiveObjects[x].GetComponent<ControlTempConfiguration>().resetTime();
                } 

            }

            if(listActiveObjects[x].GetComponent<SensorConfiguration>() != null)
                ApplySensorConfiguration(listActiveObjects[x]);

            if(listActiveObjects[x].GetComponent<ExteriorConfiguration>() != null) 
                ApplyExteriorConfiguration(listActiveObjects[x]);    

            if(listActiveObjects[x].GetComponent<InteriorConfiguration>() != null) 
                ApplyInteriorConfiguration(listActiveObjects[x]);  

            if(listActiveObjects[x].GetComponent<LimpiezaConfiguration>() != null) 
                ApplyLimpiezaConfiguration(listActiveObjects[x]);  

            if(listActiveObjects[x].GetComponent<SeguridadConfiguration>() != null) 
                ApplySeguridadConfiguration(listActiveObjects[x]);  

            if(listActiveObjects[x].GetComponent<ElectrodomesticosConfiguration>() != null){
                ApplyElectrodomesticosConfiguration(listActiveObjects[x]);  
                 
            }

        }
        
    }

    public void AddObject(GameObject gameObject){
        listActiveObjects.Add(gameObject);
        Debug.Log(listActiveObjects.Count);
    }

    public void RemoveObject(GameObject gameObject){
        listActiveObjects.Remove(gameObject);
    }

    public void SetDayState(int state){
        dayState = state;
    }

    public void onAumentar(){
        tempAct++;
        menuSim.transform.Find("TempActual").Find("Temp").GetComponent<TextMesh>().text = tempAct.ToString();
    }

    public void onDisminuir(){
        tempAct--;
        menuSim.transform.Find("TempActual").Find("Temp").GetComponent<TextMesh>().text = tempAct.ToString();
    }

    public void MovimientoCheck(){
        checkMovimiento = !checkMovimiento;
    }

    public void CalidadCheck(){
        checkCalidad = !checkCalidad;
    }

    public void GasesCheck(){
        checkGases = !checkGases;
    }

    /****************LUCES*********************/
    private void ApplyLightConfiguration(GameObject gameObject){
        
        if(dayState == 0 && gameObject.GetComponent<LightConfiguration>().morning){ // se enciende la luz
            gameObject.GetComponent<Light>().enabled = true;
            //Actualizamos su valor en el configurador y en el menu
            gameObject.GetComponent<LightConfiguration>().autoEncendido = gameObject.GetComponent<Light>().enabled;
        }else if(dayState == 1 && gameObject.GetComponent<LightConfiguration>().evening){ // se enciende la luz
            gameObject.GetComponent<Light>().enabled = true;
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<LightConfiguration>().autoEncendido = gameObject.GetComponent<Light>().enabled;
        }else if(dayState == 2 && gameObject.GetComponent<LightConfiguration>().night){   // se enciende la luz
            gameObject.GetComponent<Light>().enabled = true;
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<LightConfiguration>().autoEncendido = gameObject.GetComponent<Light>().enabled;
        }else{
            gameObject.GetComponent<LightConfiguration>().autoEncendido = false;
        }
        
        if(gameObject.GetComponent<LightConfiguration>().autoEncendido == false && gameObject.GetComponent<LightConfiguration>().encendido == false){
            gameObject.GetComponent<Light>().enabled = false;
        }
    }
    /*************FINAL LUCES****************/

    /****************CONTROL TEMPERATURA*********************/
    private void ApplyTempConfiguration(GameObject gameObject){

        //comprobamos si está encendido en caso de que este al auto y el encendido se obedece la temp del encendido
        if(gameObject.GetComponent<ControlTempConfiguration>().encendido){
            //comprobamos si es frio o calor
            if(gameObject.GetComponent<ControlTempConfiguration>().tipo == 0){ // calor 
                tempDestCalor = gameObject.GetComponent<ControlTempConfiguration>().temp;
            }else{//frio
                tempDestFrio = gameObject.GetComponent<ControlTempConfiguration>().temp;
            }

        }else if(gameObject.GetComponent<ControlTempConfiguration>().modo == 1){ // si esta en auto
            if(dayState == 0 && gameObject.GetComponent<ControlTempConfiguration>().checkMan){
                gameObject.GetComponent<ControlTempConfiguration>().autoEncendido = true;
            if(gameObject.GetComponent<ControlTempConfiguration>().tipo == 0){ // calor 
                tempDestCalor = gameObject.GetComponent<ControlTempConfiguration>().tempMan;
            }else{//frio
                tempDestFrio = gameObject.GetComponent<ControlTempConfiguration>().tempMan;
            }
                
            }else if(dayState == 2 && gameObject.GetComponent<ControlTempConfiguration>().checkNoche){
                gameObject.GetComponent<ControlTempConfiguration>().autoEncendido = true;
            if(gameObject.GetComponent<ControlTempConfiguration>().tipo == 0){ // calor 
                tempDestCalor = gameObject.GetComponent<ControlTempConfiguration>().tempNoche;
            }else{//frio
                tempDestFrio = gameObject.GetComponent<ControlTempConfiguration>().tempNoche;
            }

            }else{
               gameObject.GetComponent<ControlTempConfiguration>().autoEncendido = false;
            }
        }

        //finalmente para encender o apagar la pantalla 
        if(gameObject.GetComponent<ControlTempConfiguration>().encendido || gameObject.GetComponent<ControlTempConfiguration>().autoEncendido){
            //si tiene material lo cambiamos   
            if(gameObject.GetComponent<ControlTempConfiguration>().materialOn != null){
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<ControlTempConfiguration>().materialOn,2);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;
            }


            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Encendido", true);
            }

        }else{
             if(gameObject.GetComponent<ControlTempConfiguration>().materialOn != null){
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<ControlTempConfiguration>().materialOff,2);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;
             }

            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Encendido", false);
            }
        }
    }

    public void ActualizarTemp(GameObject gameObject){

        if(gameObject.GetComponent<ControlTempConfiguration>().encendido || (gameObject.GetComponent<ControlTempConfiguration>().modo == 1 && gameObject.GetComponent<ControlTempConfiguration>().autoEncendido)){
            if(gameObject.GetComponent<ControlTempConfiguration>().tipo == 0){ // calor 
                if(tempAct < tempDestCalor){
                    tempAct++;
                }
            }else{//frio
                if(tempAct > tempDestFrio){
                    tempAct--;
                }
            }

            menuSim.transform.Find("TempActual").Find("Temp").GetComponent<TextMesh>().text = tempAct.ToString();
        }

    }
    /*************FINAL TEMPERATURA****************/

    /********SENSORES**********/
    public void ApplySensorConfiguration(GameObject gameObject){
        // miramos el tipo del sensor 0 movimiento 1 calidad aire 2 gases
        switch (gameObject.GetComponent<SensorConfiguration>().tipo)
        {
            case 0:
                if(gameObject.GetComponent<SensorConfiguration>().encendido && checkMovimiento){ // pitara y luz constante
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<SensorConfiguration>().materialOn,0);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;

                    if(!gameObject.GetComponent<AudioSource>().isPlaying)
                        gameObject.GetComponent<AudioSource>().Play();

                }else if(gameObject.GetComponent<SensorConfiguration>().encendido && !checkMovimiento){ // parpadeo
                    gameObject.GetComponent<SensorConfiguration>().BlinkSensor();
                    gameObject.GetComponent<AudioSource>().Stop();
                }else{ // apagado
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<SensorConfiguration>().materialOff,0);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;   

                    gameObject.GetComponent<AudioSource>().Stop();             

                }
                break;
            
            case 1: 
                if(gameObject.GetComponent<SensorConfiguration>().encendido && checkCalidad){ // pitara y luz constante
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<SensorConfiguration>().materialOn,0);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;

                    if(!gameObject.GetComponent<AudioSource>().isPlaying)
                        gameObject.GetComponent<AudioSource>().Play();

                }else if(gameObject.GetComponent<SensorConfiguration>().encendido && !checkMovimiento){ // parpadeo
                    gameObject.GetComponent<SensorConfiguration>().BlinkSensor();
                    gameObject.GetComponent<AudioSource>().Stop();
                }else{ // apagado
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<SensorConfiguration>().materialOff,0);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;   

                    gameObject.GetComponent<AudioSource>().Stop();             

                }
                break;

            case 2: 
                if(gameObject.GetComponent<SensorConfiguration>().encendido && checkGases){ // pitara y luz constante
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<SensorConfiguration>().materialOn,0);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;

                    if(!gameObject.GetComponent<AudioSource>().isPlaying)
                        gameObject.GetComponent<AudioSource>().Play();

                }else if(gameObject.GetComponent<SensorConfiguration>().encendido && !checkMovimiento){ // parpadeo
                    gameObject.GetComponent<SensorConfiguration>().BlinkSensor();
                    gameObject.GetComponent<AudioSource>().Stop();
                }else{ // apagado
                    Material[] materials;
                    materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
                    materials.SetValue(gameObject.GetComponent<SensorConfiguration>().materialOff,0);
                    gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;   

                    gameObject.GetComponent<AudioSource>().Stop();             

                }
                break;
            default:
                break;
        }

    }
    /********FIN SENSORES**********/

    /********EXTERIOR**********/
    public void ApplyExteriorConfiguration(GameObject gameObject){
        if(dayState == 0 && gameObject.GetComponent<ExteriorConfiguration>().morning){ // se enciende la luz
            gameObject.transform.Find("Water").GetComponent<ParticleSystem>().Play();
            //Actualizamos su valor en el configurador y en el menu
            gameObject.GetComponent<ExteriorConfiguration>().autoEncendido = true;
        }else if(dayState == 1 && gameObject.GetComponent<ExteriorConfiguration>().evening){ // se enciende la luz
            gameObject.transform.Find("Water").GetComponent<ParticleSystem>().Play();
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<ExteriorConfiguration>().autoEncendido = true;
        }else if(dayState == 2 && gameObject.GetComponent<ExteriorConfiguration>().night){   // se enciende la luz
            gameObject.transform.Find("Water").GetComponent<ParticleSystem>().Play();
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<ExteriorConfiguration>().autoEncendido = true;
        }else{
            gameObject.GetComponent<ExteriorConfiguration>().autoEncendido = false;
        }
        
        if(gameObject.GetComponent<ExteriorConfiguration>().autoEncendido == false && gameObject.GetComponent<ExteriorConfiguration>().encendido == false){
            gameObject.transform.Find("Water").GetComponent<ParticleSystem>().Stop();
        }
    }
    /********FIN EXTERIOR**********/

    /********INTERIOR**********/
    public void ApplyInteriorConfiguration(GameObject gameObject){
        if(dayState == 0 && gameObject.GetComponent<InteriorConfiguration>().morning){ // se enciende la luz
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Abierta", true);
            }

            if(gameObject.GetComponent<AudioSource>() != null && !gameObject.GetComponent<AudioSource>().isPlaying){
                  gameObject.GetComponent<AudioSource>().Play();
            }
            //Actualizamos su valor en el configurador y en el menu
            gameObject.GetComponent<InteriorConfiguration>().autoEncendido = true;
        }else if(dayState == 1 && gameObject.GetComponent<InteriorConfiguration>().evening){ // se enciende la luz
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Abierta", true);
            }

            if(gameObject.GetComponent<AudioSource>() != null && !gameObject.GetComponent<AudioSource>().isPlaying){
                  gameObject.GetComponent<AudioSource>().Play();
            }
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<InteriorConfiguration>().autoEncendido = true;
        }else if(dayState == 2 && gameObject.GetComponent<InteriorConfiguration>().night){   // se enciende la luz
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Abierta", true);
            }

            if(gameObject.GetComponent<AudioSource>() != null && !gameObject.GetComponent<AudioSource>().isPlaying){
                  gameObject.GetComponent<AudioSource>().Play();
            }
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<InteriorConfiguration>().autoEncendido = true;
        }else{
            gameObject.GetComponent<InteriorConfiguration>().autoEncendido = false;
        }
        
        if(gameObject.GetComponent<InteriorConfiguration>().autoEncendido == false && gameObject.GetComponent<InteriorConfiguration>().encendido == false){
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Abierta", false);
            }

            if(gameObject.GetComponent<AudioSource>() != null){
                gameObject.GetComponent<AudioSource>().Pause();  
            }
        }
    }
    /********FIN INTERIOR**********/

    /********LIMPIEZA**********/
    public void ApplyLimpiezaConfiguration(GameObject gameObject){
        if(dayState == 0 && gameObject.GetComponent<LimpiezaConfiguration>().morning){ // se enciende la luz
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Encendido", true);
            }

            //Actualizamos su valor en el configurador y en el menu
            gameObject.GetComponent<LimpiezaConfiguration>().autoEncendido = true;
        }else if(dayState == 1 && gameObject.GetComponent<LimpiezaConfiguration>().evening){ // se enciende la luz
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Encendido", true);
            }

            //Actualizamos su valor en el configurador
            gameObject.GetComponent<LimpiezaConfiguration>().autoEncendido = true;
        }else if(dayState == 2 && gameObject.GetComponent<LimpiezaConfiguration>().night){   // se enciende la luz
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Encendido", true);
            }

            //Actualizamos su valor en el configurador
            gameObject.GetComponent<LimpiezaConfiguration>().autoEncendido = true;
        }else{
            gameObject.GetComponent<LimpiezaConfiguration>().autoEncendido = false;
        }
        
        if(gameObject.GetComponent<LimpiezaConfiguration>().autoEncendido == false && gameObject.GetComponent<LimpiezaConfiguration>().encendido == false){
            if(gameObject.GetComponent<Animator>() != null){
                gameObject.GetComponent<Animator>().SetBool("Encendido", false);
            }
        }
    }
    /********FIN LIMPIEZA**********/

/********SEGURIDAD**********/
    public void ApplySeguridadConfiguration(GameObject gameObject){
        if(dayState == 0 && gameObject.GetComponent<SeguridadConfiguration>().morning){ // se enciende la luz
            //Actualizamos su valor en el configurador y en el menu
            gameObject.GetComponent<SeguridadConfiguration>().autoEncendido = true;
        }else if(dayState == 1 && gameObject.GetComponent<SeguridadConfiguration>().evening){ // se enciende la luz
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<SeguridadConfiguration>().autoEncendido = true;
        }else if(dayState == 2 && gameObject.GetComponent<SeguridadConfiguration>().night){   // se enciende la luz
            //Actualizamos su valor en el configurador
            gameObject.GetComponent<SeguridadConfiguration>().autoEncendido = true;
        }else{
            gameObject.GetComponent<SeguridadConfiguration>().autoEncendido = false;
        }
        
        if(gameObject.GetComponent<SeguridadConfiguration>().autoEncendido == false && gameObject.GetComponent<SeguridadConfiguration>().encendido == false){
            if(gameObject.GetComponent<AudioSource>() != null){
                gameObject.GetComponent<AudioSource>().Stop();  
            }
        }else{ // si esta encendido o auto alertamos si hay colision con la camara (Usuario)
            if(Camera.main.transform.position.y < gameObject.transform.position.y && Camera.main.transform.position.x < gameObject.transform.position.x + 1 && Camera.main.transform.position.x > gameObject.transform.position.x - 1 && Camera.main.transform.position.z < gameObject.transform.position.z + 1 && Camera.main.transform.position.z > gameObject.transform.position.z - 1  ){
                if(gameObject.GetComponent<AudioSource>() != null && !gameObject.GetComponent<AudioSource>().isPlaying){
                    gameObject.GetComponent<AudioSource>().Play();  
                } 
            }else{ // ESTO NO SI QUEREMOS QUE SE SUENE TODO EL RATO HASTA QUE SE APAGUE

                if(gameObject.GetComponent<AudioSource>() != null){
                    gameObject.GetComponent<AudioSource>().Stop();  
                } 
            }
        }
    }
    /********FIN SEGURIDAD**********/

/********ELECTRODOMESTICOS**********/
    public void ApplyElectrodomesticosConfiguration(GameObject gameObject){
       if(gameObject.GetComponent<ElectrodomesticosConfiguration>().encendido){ // si esta encendido empezamos a calcular el consumo
           
           gameObject.GetComponent<ElectrodomesticosConfiguration>().changeConsumo();

            if(transform.Find("MenuConfig") && transform.Find("MenuConfig").GetComponent<ConfigurationController>().GetConfigurationObject().Equals(gameObject)){
                //si entra aqui significa que el menu abierto en confi es de este gameobject por lo que podemos cambiar el valor sin problema
                transform.Find("MenuConfig").GetComponent<ConfigurationController>().SetElectrodomesticosConfiguration();
            }
        

       }
    }
    /********FIN ELECTRODOMESTICOS**********/
}
