using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrodomesticosConfiguration : MonoBehaviour
{
    //Contiene todas las variables configurables de la luz
    public bool encendido = true;
    //Configurable
    public int modo = 1; //0 eco 1 normal 2 ultra 
    public int consumoEco = 0;
    public int consumoNormal = 0;
    public int consumoUltra = 0;
    public int consumoTotal;

    public bool cooldown = false; // cooldown del parpadeo

    public void ResetCooldownBlink(){
        cooldown = false;
    }

    public void changeConsumo(){
        if(cooldown == false){
            updateConsumo();
            Invoke("ResetCooldownBlink",3.0f);
            cooldown = true;
        }
    }

    private void updateConsumo(){
        if(modo == 0){//eco
            consumoTotal = consumoTotal + consumoEco;
        }else if(modo == 1){//normal
            consumoTotal = consumoTotal + consumoNormal;
        }else if(modo == 2){//ultra
            consumoTotal = consumoTotal + consumoUltra;
        }
    }

}
