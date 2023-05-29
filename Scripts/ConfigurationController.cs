using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ConfigurationController : MonoBehaviour
{

    //Necesitamos una referencia del objeto para cambiar sus atributos
    private GameObject configurationObject;

    private bool cooldownElectro = false;

    void ResetCooldownElectro(){
        cooldownElectro = false;
    }

    void Update(){
        if(configurationObject.GetComponent<ElectrodomesticosConfiguration>() != null){ // hacemos esto para que se actualice el valoren directo cada 3 segundos
            if(cooldownElectro == false){
                SetElectrodomesticosConfiguration(); 
                Invoke("ResetCooldownElectro",3.0f);
                cooldownElectro = true;
            }
        }
    }

    public GameObject GetConfigurationObject(){
        return configurationObject;
    }
    public void SetConfigurationObject(GameObject objectConf){
        configurationObject = objectConf;

        //Comprobamos el tipo para establecer la configuracion que ya tenia
        if(configurationObject.GetComponent<Light>() != null){ 
            SetLightConfiguration();
        }

        if(configurationObject.GetComponent<ControlTempConfiguration>() != null){
            SetControlTempConfiguration();
        }

        if(configurationObject.GetComponent<SensorConfiguration>() != null){
            SetSensorConfiguration();
        }

        if(configurationObject.GetComponent<ExteriorConfiguration>() != null){
            SetExteriorConfiguration();
        }

        if(configurationObject.GetComponent<InteriorConfiguration>() != null){
            SetInteriorConfiguration();
        }

        if(configurationObject.GetComponent<LimpiezaConfiguration>() != null){
            SetLimpiezaConfiguration();
        }

        if(configurationObject.GetComponent<SeguridadConfiguration>() != null){
            SetSeguridadConfiguration();
        }

        if(configurationObject.GetComponent<ElectrodomesticosConfiguration>() != null){
            SetElectrodomesticosConfiguration();
        }

    }

    /********LUCES**********/
    public void SetLightConfiguration(){
        GameObject luzGameObjectMenu = this.gameObject.transform.Find("Luces").gameObject; // menu configuration apartado luces
        luzGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LightConfiguration>().encendido;
        luzGameObjectMenu.transform.Find("SliderRed").GetComponent<PinchSlider>().SliderValue = configurationObject.GetComponent<LightConfiguration>().colorRed;
        luzGameObjectMenu.transform.Find("SliderGreen").GetComponent<PinchSlider>().SliderValue = configurationObject.GetComponent<LightConfiguration>().colorGreen;
        luzGameObjectMenu.transform.Find("SliderBlue").GetComponent<PinchSlider>().SliderValue = configurationObject.GetComponent<LightConfiguration>().colorBlue;
        luzGameObjectMenu.transform.Find("SliderBrillo").GetComponent<PinchSlider>().SliderValue = configurationObject.GetComponent<LightConfiguration>().intensity;
        luzGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckManana").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LightConfiguration>().morning;
        luzGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckTarde").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LightConfiguration>().evening;
        luzGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckNoche").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LightConfiguration>().night;

    }

    public void OnClickOffOn(){
        configurationObject.GetComponent<Light>().enabled = !configurationObject.GetComponent<Light>().enabled;

        //Actualizamos valores
        configurationObject.GetComponent<LightConfiguration>().encendido = !configurationObject.GetComponent<LightConfiguration>().encendido;
        this.gameObject.transform.Find("Luces").transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LightConfiguration>().encendido;
        
    }
    public void OnSliderUpdatedBrillo(SliderEventData eventData){
        configurationObject.GetComponent<Light>().intensity = eventData.NewValue*10/2.0f; // entre 4 para que los valores vayan de 0 a 5
        //Actualizamos valores
        configurationObject.GetComponent<LightConfiguration>().intensity = eventData.NewValue;
    }

    public void OnSliderUpdatedRed(SliderEventData eventData){
        configurationObject.GetComponent<Light>().color = new Color(eventData.NewValue, configurationObject.GetComponent<Light>().color.g, configurationObject.GetComponent<Light>().color.b);
    
        configurationObject.GetComponent<LightConfiguration>().colorRed = configurationObject.GetComponent<Light>().color.r;
    }

    public void OnSliderUpdatedGreen(SliderEventData eventData){
        configurationObject.GetComponent<Light>().color = new Color(configurationObject.GetComponent<Light>().color.r, eventData.NewValue, configurationObject.GetComponent<Light>().color.b);
        //Actualizamos valores
        configurationObject.GetComponent<LightConfiguration>().colorGreen = configurationObject.GetComponent<Light>().color.g;
    }

    public void OnSliderUpdatedBlue(SliderEventData eventData){
        configurationObject.GetComponent<Light>().color = new Color(configurationObject.GetComponent<Light>().color.r, configurationObject.GetComponent<Light>().color.g, eventData.NewValue);
        //Actualizamos valores
        configurationObject.GetComponent<LightConfiguration>().colorBlue =  configurationObject.GetComponent<Light>().color.b;   
    }

    public void OnClickMorning(){
        configurationObject.GetComponent<LightConfiguration>().morning =  !configurationObject.GetComponent<LightConfiguration>().morning;
    }

    public void OnClickEvening(){
        configurationObject.GetComponent<LightConfiguration>().evening =  !configurationObject.GetComponent<LightConfiguration>().evening;
    }

    public void OnClickNight(){
        configurationObject.GetComponent<LightConfiguration>().night =  !configurationObject.GetComponent<LightConfiguration>().night;
    }

    /********FINAL LUCES**********/

    /********TEMPERATURA**********/
    public void OnClickOffOnTemp(){
        //Actualizamos valores
        configurationObject.GetComponent<ControlTempConfiguration>().encendido = !configurationObject.GetComponent<ControlTempConfiguration>().encendido;

        //si tiene material lo cambiamos
        if(configurationObject.GetComponent<ControlTempConfiguration>().materialOn != null){
            if(configurationObject.GetComponent<ControlTempConfiguration>().encendido){
                Material[] materials;
                materials = configurationObject.GetComponentInChildren<MeshRenderer>().materials;
                materials.SetValue(configurationObject.GetComponent<ControlTempConfiguration>().materialOn,2);
                configurationObject.GetComponentInChildren<MeshRenderer>().materials = materials;
            }else{
                Material[] materials;
                materials = configurationObject.GetComponentInChildren<MeshRenderer>().materials;
                materials.SetValue(configurationObject.GetComponent<ControlTempConfiguration>().materialOff,2);
                configurationObject.GetComponentInChildren<MeshRenderer>().materials = materials;
            }
        }

        //si tiene componente animacion lo encendemos/apagamos
        if(configurationObject.GetComponent<Animator>() != null){
            if(configurationObject.GetComponent<ControlTempConfiguration>().encendido){
                configurationObject.GetComponent<Animator>().SetBool("Encendido", true);
            }else{
                configurationObject.GetComponent<Animator>().SetBool("Encendido", false);
            }
        }
    }

    public void SetControlTempConfiguration(){
        GameObject ControlTempGameObjectMenu = this.gameObject.transform.Find("ControlTemp").gameObject; // menu configuration
        ControlTempGameObjectMenu.transform.Find("Temp").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().temp.ToString();
        ControlTempGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ControlTempConfiguration>().encendido;

        if(configurationObject.GetComponent<ControlTempConfiguration>().modo == 0){
            ControlTempGameObjectMenu.transform.Find("RadialSet").Find("RadialMan").GetComponent<Interactable>().IsToggled = true;
            ControlTempGameObjectMenu.transform.Find("RadialSet").Find("RadialAuto").GetComponent<Interactable>().IsToggled = false;
        }else{
            ControlTempGameObjectMenu.transform.Find("RadialSet").Find("RadialAuto").GetComponent<Interactable>().IsToggled = true;
            ControlTempGameObjectMenu.transform.Find("RadialSet").Find("RadialMan").GetComponent<Interactable>().IsToggled = false;
        }
        
        ControlTempGameObjectMenu.transform.Find("CheckboxCollection").Find("CheckManana").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ControlTempConfiguration>().checkMan;
        
        ControlTempGameObjectMenu.transform.Find("CheckboxCollection").Find("CheckNoche").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ControlTempConfiguration>().checkNoche;
        ControlTempGameObjectMenu.transform.Find("CheckboxCollection").Find("TempManana").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().tempMan.ToString();
        ControlTempGameObjectMenu.transform.Find("CheckboxCollection").Find("TempNoche").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().tempNoche.ToString();
    }

    public void setModoFuncionamiento(int modo){
            configurationObject.GetComponent<ControlTempConfiguration>().modo =  modo;
    }

    public void OnClickMorningTemp(){
        configurationObject.GetComponent<ControlTempConfiguration>().checkMan =  !configurationObject.GetComponent<ControlTempConfiguration>().checkMan;
    }

    public void OnClickNightTemp(){
         configurationObject.GetComponent<ControlTempConfiguration>().checkNoche =  !configurationObject.GetComponent<ControlTempConfiguration>().checkNoche;
    }

    public void onAumentar(){
        configurationObject.GetComponent<ControlTempConfiguration>().temp = configurationObject.GetComponent<ControlTempConfiguration>().temp + 1;

        this.gameObject.transform.Find("ControlTemp").transform.Find("Temp").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().temp.ToString();
    }

    public void onDisminuir(){
        configurationObject.GetComponent<ControlTempConfiguration>().temp = configurationObject.GetComponent<ControlTempConfiguration>().temp - 1;

        this.gameObject.transform.Find("ControlTemp").transform.Find("Temp").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().temp.ToString();
    }

    public void onAumentarManana(){
        configurationObject.GetComponent<ControlTempConfiguration>().tempMan = configurationObject.GetComponent<ControlTempConfiguration>().tempMan + 1;

        this.gameObject.transform.Find("ControlTemp").Find("CheckboxCollection").transform.Find("TempManana").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().tempMan.ToString();
    }

    public void onDisminuirManana(){
        configurationObject.GetComponent<ControlTempConfiguration>().tempMan = configurationObject.GetComponent<ControlTempConfiguration>().tempMan - 1;

        this.gameObject.transform.Find("ControlTemp").Find("CheckboxCollection").transform.Find("TempManana").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().tempMan.ToString();
    }

    public void onAumentarNoche(){
        configurationObject.GetComponent<ControlTempConfiguration>().tempNoche = configurationObject.GetComponent<ControlTempConfiguration>().tempNoche + 1;

        this.gameObject.transform.Find("ControlTemp").Find("CheckboxCollection").transform.Find("TempNoche").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().tempNoche.ToString();
    }

    public void onDisminuirNoche(){
        configurationObject.GetComponent<ControlTempConfiguration>().tempNoche = configurationObject.GetComponent<ControlTempConfiguration>().tempNoche - 1;

        this.gameObject.transform.Find("ControlTemp").Find("CheckboxCollection").transform.Find("TempNoche").GetComponent<TextMesh>().text = configurationObject.GetComponent<ControlTempConfiguration>().tempNoche.ToString();
    }
    /********FIN TEMPERATURA**********/

    /********SENSORES**********/

    public void SetSensorConfiguration(){
        GameObject SensorGameObjectMenu = this.gameObject.transform.Find("Sensores").gameObject; // menu configuration apartado luces
        SensorGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<SensorConfiguration>().encendido;
    }

    public void OnClickOffOnSensor(){
        //Actualizamos valores
        configurationObject.GetComponent<SensorConfiguration>().encendido = !configurationObject.GetComponent<SensorConfiguration>().encendido;

        if(configurationObject.GetComponent<SensorConfiguration>().materialOn != null){
            if(configurationObject.GetComponent<SensorConfiguration>().encendido){
                Material[] materials;
                materials = configurationObject.GetComponentInChildren<MeshRenderer>().materials;
                materials.SetValue(configurationObject.GetComponent<SensorConfiguration>().materialOn,0);
                configurationObject.GetComponentInChildren<MeshRenderer>().materials = materials;
            }else{
                Material[] materials;
                materials = configurationObject.GetComponentInChildren<MeshRenderer>().materials;
                materials.SetValue(configurationObject.GetComponent<SensorConfiguration>().materialOff,0);
                configurationObject.GetComponentInChildren<MeshRenderer>().materials = materials;
            }
        }
    }

    /********FIN SENSORES**********/

    /********EXTERIOR**********/
    public void SetExteriorConfiguration(){
        GameObject exteriorGameObjectMenu = this.gameObject.transform.Find("Exterior").gameObject; // menu configuration apartado luces
        exteriorGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ExteriorConfiguration>().encendido;
        exteriorGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckManana").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ExteriorConfiguration>().morning;
        exteriorGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckTarde").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ExteriorConfiguration>().evening;
        exteriorGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckNoche").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ExteriorConfiguration>().night;
    }

    public void OnClickOffOnExterior(){
        //Actualizamos valores
        configurationObject.GetComponent<ExteriorConfiguration>().encendido = !configurationObject.GetComponent<ExteriorConfiguration>().encendido;
        
        // activamos/desactivamos particulas
        if(configurationObject.GetComponent<ExteriorConfiguration>().encendido){
            configurationObject.transform.Find("Water").GetComponent<ParticleSystem>().Play();
        }else{
            configurationObject.transform.Find("Water").GetComponent<ParticleSystem>().Stop();
        }

    }

    public void OnClickMorningExterior(){
        configurationObject.GetComponent<ExteriorConfiguration>().morning =  !configurationObject.GetComponent<ExteriorConfiguration>().morning;
    }

    public void OnClickEveningExterior(){
        configurationObject.GetComponent<ExteriorConfiguration>().evening =  !configurationObject.GetComponent<ExteriorConfiguration>().evening;
    }

    public void OnClickNightExterior(){
        configurationObject.GetComponent<ExteriorConfiguration>().night =  !configurationObject.GetComponent<ExteriorConfiguration>().night;
    }
    /********FIN EXTERIOR**********/

    /********INTERIOR**********/
    public void SetInteriorConfiguration(){
        GameObject interiorGameObjectMenu = this.gameObject.transform.Find("Interior").gameObject; // menu configuration apartado luces
        interiorGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<InteriorConfiguration>().encendido;
        interiorGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckManana").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<InteriorConfiguration>().morning;
        interiorGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckTarde").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<InteriorConfiguration>().evening;
        interiorGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckNoche").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<InteriorConfiguration>().night;
    }

    public void OnClickOffOnInterior(){
        //Actualizamos valores
        configurationObject.GetComponent<InteriorConfiguration>().encendido = !configurationObject.GetComponent<InteriorConfiguration>().encendido;
        
        // activamos/desactivamos animacion
        if(configurationObject.GetComponent<Animator>() != null){
            if(configurationObject.GetComponent<InteriorConfiguration>().encendido){
                configurationObject.GetComponent<Animator>().SetBool("Abierta", true);
            }else{
                configurationObject.GetComponent<Animator>().SetBool("Abierta", false);
            }
        }

        if(configurationObject.GetComponent<AudioSource>() != null){
            if(configurationObject.GetComponent<InteriorConfiguration>().encendido){
                configurationObject.GetComponent<AudioSource>().Play();
            }else{
                configurationObject.GetComponent<AudioSource>().Pause();
            }
        }

    }

    public void OnClickMorningInterior(){
        configurationObject.GetComponent<InteriorConfiguration>().morning =  !configurationObject.GetComponent<InteriorConfiguration>().morning;
    }

    public void OnClickEveningInterior(){
        configurationObject.GetComponent<InteriorConfiguration>().evening =  !configurationObject.GetComponent<InteriorConfiguration>().evening;
    }

    public void OnClickNightInterior(){
        configurationObject.GetComponent<InteriorConfiguration>().night =  !configurationObject.GetComponent<InteriorConfiguration>().night;
    }
    /********FIN INTERIOR**********/

    /********LIMPIEZA**********/
    public void SetLimpiezaConfiguration(){
        GameObject limpiezaGameObjectMenu = this.gameObject.transform.Find("Limpieza").gameObject; // menu configuration apartado luces
        limpiezaGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LimpiezaConfiguration>().encendido;
        limpiezaGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckManana").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LimpiezaConfiguration>().morning;
        limpiezaGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckTarde").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LimpiezaConfiguration>().evening;
        limpiezaGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckNoche").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<LimpiezaConfiguration>().night;
    }

    public void OnClickOffOnLimpieza(){
        //Actualizamos valores
        configurationObject.GetComponent<LimpiezaConfiguration>().encendido = !configurationObject.GetComponent<LimpiezaConfiguration>().encendido;
        
        // activamos/desactivamos animacion
        if(configurationObject.GetComponent<Animator>() != null){
            if(configurationObject.GetComponent<LimpiezaConfiguration>().encendido){
                configurationObject.GetComponent<Animator>().SetBool("Encendido", true);
            }else{
                configurationObject.GetComponent<Animator>().SetBool("Encendido", false);
            }
        }
    }

    public void OnClickMorningLimpieza(){
        configurationObject.GetComponent<LimpiezaConfiguration>().morning =  !configurationObject.GetComponent<LimpiezaConfiguration>().morning;
    }

    public void OnClickEveningLimpieza(){
        configurationObject.GetComponent<LimpiezaConfiguration>().evening =  !configurationObject.GetComponent<LimpiezaConfiguration>().evening;
    }

    public void OnClickNightLimpieza(){
        configurationObject.GetComponent<LimpiezaConfiguration>().night =  !configurationObject.GetComponent<LimpiezaConfiguration>().night;
    }
    /********FIN LIMPIEZA**********/

    /********SEGURIDAD**********/
    public void SetSeguridadConfiguration(){
        GameObject seguridadGameObjectMenu = this.gameObject.transform.Find("Seguridad").gameObject; // menu configuration apartado luces
        seguridadGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<SeguridadConfiguration>().encendido;
        seguridadGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckManana").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<SeguridadConfiguration>().morning;
        seguridadGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckTarde").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<SeguridadConfiguration>().evening;
        seguridadGameObjectMenu.transform.Find("CheckboxCollection").transform.Find("CheckNoche").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<SeguridadConfiguration>().night;
    }

    public void OnClickOffOnSeguridad(){
        //Actualizamos valores
        configurationObject.GetComponent<SeguridadConfiguration>().encendido = !configurationObject.GetComponent<SeguridadConfiguration>().encendido;
        
    }

    public void OnClickMorningSeguridad(){
        configurationObject.GetComponent<SeguridadConfiguration>().morning =  !configurationObject.GetComponent<SeguridadConfiguration>().morning;
    }

    public void OnClickEveningSeguridad(){
        configurationObject.GetComponent<SeguridadConfiguration>().evening =  !configurationObject.GetComponent<SeguridadConfiguration>().evening;
    }

    public void OnClickNightSeguridad(){
        configurationObject.GetComponent<SeguridadConfiguration>().night =  !configurationObject.GetComponent<SeguridadConfiguration>().night;
    }
    /********FIN SEGURIDAD**********/

    /********ELECTRODOMESTICOS**********/
    public void SetElectrodomesticosConfiguration(){
        GameObject electrodomesticosGameObjectMenu = this.gameObject.transform.Find("Electrodomesticos").gameObject; // menu configuration apartado luces
        electrodomesticosGameObjectMenu.transform.Find("OnOff").GetComponent<Interactable>().IsToggled = configurationObject.GetComponent<ElectrodomesticosConfiguration>().encendido;
        if(configurationObject.GetComponent<ElectrodomesticosConfiguration>().modo == 0){ // hay que ponerlo asi o sale bug por el codigo interno de mrkt
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialEco").GetComponent<Interactable>().IsToggled = true;
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialNormal").GetComponent<Interactable>().IsToggled = false;
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialUltra").GetComponent<Interactable>().IsToggled = false;
        }else if(configurationObject.GetComponent<ElectrodomesticosConfiguration>().modo == 1){
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialEco").GetComponent<Interactable>().IsToggled = false;
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialNormal").GetComponent<Interactable>().IsToggled = true;
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialUltra").GetComponent<Interactable>().IsToggled = false;
        }else{
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialEco").GetComponent<Interactable>().IsToggled = false;
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialNormal").GetComponent<Interactable>().IsToggled = false;
            electrodomesticosGameObjectMenu.transform.Find("RadialSet").Find("RadialUltra").GetComponent<Interactable>().IsToggled = true;
        }

        electrodomesticosGameObjectMenu.transform.Find("Consumo").GetComponent<TextMesh>().text = configurationObject.GetComponent<ElectrodomesticosConfiguration>().consumoTotal.ToString();
    }

    public void OnClickOffOnElectrodomesticos(){
        //Actualizamos valores
        configurationObject.GetComponent<ElectrodomesticosConfiguration>().encendido = !configurationObject.GetComponent<ElectrodomesticosConfiguration>().encendido;
        
    }

    public void setModoFuncionamientoElectrodomesticos(int modo){
            configurationObject.GetComponent<ElectrodomesticosConfiguration>().modo =  modo;
    }


    /********FIN ELECTRODOMESTICOS**********/
}
