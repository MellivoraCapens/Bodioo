using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour

{
   
    
    private float temp = 0, amount;
    public Vector3 CenterOfMass;
    public float brakePower;
    AudioSource audioS;
    
   
    
   
    public ParticleSystem egzoz1, egzoz2;
    public float[] gear = new float[7];
    WheelFrictionCurve friction;
    private Rigidbody rb;
    private float m_horizantalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontLeftW, frontRightW;
    public WheelCollider backLeftW, backRightW;
    public Transform frontLeftT, frontRightT;
    public Transform backLeftT, backRightT;
   
    public float maxSteerAngle = 30;
    public static float maxHorsepower=300;
    public static float maxRpm =9000;

    static float currentHp = Mathf.Clamp(currentHp, 0.0f, maxHorsepower);

    public static float rpm = Mathf.Clamp(rpm, 800, maxRpm);
    public float differential=10;
    public float Torque=400;
    public int currentGear=0;
    public List<float> gearRatio;
    public float rGearRatiot=3.18f;
    public float turboRpm =4;
    public float turboPower=1.5f;
    public float NitroPower=1000;



    


   
   
    private void Start()
        
    {
        currentGear =0;
        rpm = 800;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMass;
        audioS = GetComponent<AudioSource>();

    }

    private void FixedUpdate()
    {
        

        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPose();
        gearSelection();
        audioS.pitch = rpm / 2000;

    }

    public void GetInput()
    {
        m_horizantalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

    }
    public void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizantalInput;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;


    }
    public void Accelerate()
    {
       

        if (currentGear == 0)
        {

            backLeftW.motorTorque = 0;
            backRightW.motorTorque = 0;
            rpm = currentRpm();
        }
        else
        {
            /*amount = rb.velocity.magnitude - temp;
            temp = rb.velocity.magnitude;
            rpm += amount;*/

            if (Input.GetKeyDown(KeyCode.Space))
            {
                backRightW.brakeTorque = 1000.0f;
                backLeftW.brakeTorque = 1000.0f;

                friction = backLeftW.sidewaysFriction;
                friction.stiffness = 0.5f;
                backLeftW.sidewaysFriction = friction;
                backRightW.sidewaysFriction = friction;
                friction.stiffness = 0.8f;
                frontLeftW.sidewaysFriction = friction;
                frontRightW.sidewaysFriction = friction;



            }
            if(Input.GetKeyUp(KeyCode.Space))
            {

                friction = backLeftW.sidewaysFriction;
                friction.stiffness = 2;
                backLeftW.sidewaysFriction = friction;
                backRightW.sidewaysFriction = friction;

                backRightW.brakeTorque = 0;
                backLeftW.brakeTorque = 0;
                
                frontLeftW.sidewaysFriction = friction;
                frontRightW.sidewaysFriction = friction;
                frontRightW.sidewaysFriction = friction;
            }
            
            Nitro();
            if (m_verticalInput > 0)
            {
                backLeftW.motorTorque = differential * -m_verticalInput * CurrentHp();
                backRightW.motorTorque = differential * -m_verticalInput * CurrentHp();
               // print("RPM"+rpm+ "HorsePower"+currentHp +"currentTorque"+  backLeftW.motorTorque+"     "+ rb.velocity.magnitude+"  " +backLeftW.rpm);
            }
            if (m_verticalInput <0)
            {
                backLeftW.brakeTorque = brakePower;
                backRightW.brakeTorque = brakePower;
                frontLeftW.brakeTorque = brakePower;
                frontRightW.brakeTorque = brakePower;
                currentHp = CurrentHp();
            }
            else
            {
                backLeftW.brakeTorque = 0;
                backRightW.brakeTorque = 0;
                frontLeftW.brakeTorque = 0;
                frontRightW.brakeTorque = 0;
                currentHp = CurrentHp();
            }
           
                 
            


        }
      
        


    }
    public void UpdateWheelPose()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);
        
    }
     public float CurrentHp()
    {
        if (rpm >= 8500)
            return currentHp = currentRpm()*10 /7120;
     
        else
        {
            currentHp = currentRpm() * Torque / 7120;
            return currentHp = Mathf.Clamp(currentHp, 0.0f, maxHorsepower); ;
        }
        
    }
    public float currentRpm()
    {
        if (rpm >= maxRpm)
            rpm -= 500;

      
        if (m_verticalInput <= 0 &&   rpm>800)
        {

            if (currentGear == 0)
            {
                return rpm -= Time.deltaTime * gear[Mathf.Abs(currentGear)] ;
                print(rpm);
            }
            else
            {

                amount = rb.velocity.magnitude - temp;
                temp = rb.velocity.magnitude;
                rpm -= Time.deltaTime * gear[Mathf.Abs(currentGear)] * Mathf.Abs(amount) * differential;
               // print(rpm + "    " + rb.velocity.magnitude);
            }
            return rpm;
        }
        else
             return rpm += m_verticalInput * Time.deltaTime * gear[Mathf.Abs(currentGear)];
       
    }
    public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);
        
        _transform.rotation = _quat;
    }
   
    
    public void gearSelection()
    {
        if (Input.GetKey(KeyCode.N))
        {
            currentGear = 0;
        }
        if (rpm > 8200 && currentGear<gear.Length-1 && currentGear !=0 )
        {
            rpm -= 3000;
            currentGear++;
        }
        if (rpm < 5000  && currentGear >1 )
        {
            rpm += 3000;
            currentGear--;
        }
        if (currentGear == 0 && Input.GetKey(KeyCode.LeftShift) )
        {
            currentGear++;
        }
    }
    public void gearSelectionManual()
    {
     
       
        if (currentGear == 0)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {

                currentGear++;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {

                currentGear--;

            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.LeftShift)&& currentGear!=gear.Length-1)
            {
                rpm -= 4;

                currentGear++;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                rpm += 4;
                currentGear--;

            }
           
        }
       
    }
    public void Nitro()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            egzoz1.maxParticles = 100;
            egzoz2.maxParticles = 100;
         
            backLeftW.motorTorque = -differential*m_verticalInput * (CurrentHp() + NitroPower);
            backRightW.motorTorque = differential*-m_verticalInput * (CurrentHp() + NitroPower);
      
        }
       
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            egzoz1.maxParticles = 0;

            egzoz2.maxParticles = 0;
        }
    }
   


}
