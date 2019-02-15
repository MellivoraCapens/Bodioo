using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public static float maxHorsepower;
    public static float maxRpm=9.0f;
    
    static float currentHp=Mathf.Clamp(currentHp,0.0f,maxHorsepower);
    
    public static float rpm=Mathf.Clamp(rpm,0.8f,maxRpm);
    public float differential;
    public float Torque;
    public int currentGear;
    public List<float> gearRatio;
    public float rGearRatiot;
    public float turboRpm;
    public float turboPower;
    public float NitroPower;
    public Motor()
    {

        maxHorsepower = 300;
        maxRpm = 9.0f;
        currentGear = 0;
        rpm = 0.8f;
        gearRatio = new List<float>();
        gearRatio.Capacity = 7;
        Torque = 400;
        differential = 10;
        turboRpm = 4;
        turboPower = 1.5f;
        NitroPower=1000;


    }
   
    public void CurrentHp()
    {
        currentHp = rpm * Torque / 7120;
    }
    
}
