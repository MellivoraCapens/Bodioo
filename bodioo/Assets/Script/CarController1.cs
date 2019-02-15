using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController1 : MonoBehaviour
{
    WheelFrictionCurve friction;
    private Rigidbody rb;
    private float m_horizantalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public float differential = 10;
    public float Torque = 400;
    public int currentGear = 0;
    public float rpm = 800;
    public float maxSteerAngle = 30;
    public float speed;


    private float temp = 0, amount;
    public Vector3 CenterOfMass;
    public float brakePower;
    public float nitroPower=10;
    public float nitro = 1;
    public GameObject nos1,nos2;
    AudioSource audioS;

    public WheelCollider frontLeftW, frontRightW;
    public WheelCollider backLeftW, backRightW;
    public Transform frontLeftT, frontRightT;
    public Transform backLeftT, backRightT;
    void Start()
    {

        currentGear = 1;
        rpm = 800;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMass;
        audioS = GetComponent<AudioSource>();
        speed = rb.velocity.sqrMagnitude;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPose();
        gearSelection();
        currentRpm();
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

        }
        else if(currentGear >0)
        {

            if (m_verticalInput >= 0)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    nitro = nitroPower;
                    nos1.SetActive(true);
                    nos1.SetActive(true);
                }   
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    nitro = 1;
                    nos1.SetActive(false);
                    nos2.SetActive(false);
                }
                  

                print(nitro);

                    
                    backLeftW.motorTorque = differential * -m_verticalInput * Torque * 100*nitro;
                    backRightW.motorTorque = differential * -m_verticalInput * Torque * 100*nitro;
                    backLeftW.brakeTorque = 0;
                    backRightW.brakeTorque = 0;
                    frontLeftW.brakeTorque = 0;
                    frontRightW.brakeTorque = 0;

                if (Input.GetKey(KeyCode.Space))
                {
                    backRightW.brakeTorque = 10000.0f;
                    backLeftW.brakeTorque = 10000.0f;

                    friction = backLeftW.sidewaysFriction;
                    friction.stiffness = 0.8f;
                    backLeftW.sidewaysFriction = friction;
                    backRightW.sidewaysFriction = friction;
                    friction.stiffness = 1f;
                    frontLeftW.sidewaysFriction = friction;
                    frontRightW.sidewaysFriction = friction;



                }
                else
               {
                    friction = backLeftW.sidewaysFriction;
                    friction.stiffness = 2;
                    backLeftW.sidewaysFriction = friction;
                    backRightW.sidewaysFriction = friction;
                    frontLeftW.sidewaysFriction = friction;
                    frontRightW.sidewaysFriction = friction;
                    backRightW.brakeTorque = 0;
                    backLeftW.brakeTorque = 0;

                }
            }
            if (m_verticalInput < 0)
            {
                backLeftW.brakeTorque = brakePower;
                backRightW.brakeTorque = brakePower;
                frontLeftW.brakeTorque = brakePower;
                frontRightW.brakeTorque = brakePower;
               
                
            }



        }
        else if(currentGear==-1)
        {
         

            if (m_verticalInput <= 0)
            {

                backLeftW.motorTorque = differential * -m_verticalInput * Torque * 100;
                backRightW.motorTorque = differential * -m_verticalInput * Torque * 100;
                backLeftW.brakeTorque = 0;
                backRightW.brakeTorque = 0;
                frontLeftW.brakeTorque = 0;
                frontRightW.brakeTorque = 0;
            }
            if (m_verticalInput > 0)
            {
                if (rb.velocity.magnitude <1 && m_verticalInput > 0)
                    currentGear = 0;
                backLeftW.brakeTorque = brakePower;
                backRightW.brakeTorque = brakePower;
                frontLeftW.brakeTorque = brakePower;
                frontRightW.brakeTorque = brakePower;
                backLeftW.motorTorque = 0;
                backRightW.motorTorque = 0;
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
    public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);

        _transform.rotation = _quat;
    }
    public void gearSelection()
    {
        if (currentGear == 1 && rb.velocity.magnitude < 1 && m_verticalInput < 0)
            currentGear = 0;
        if (Input.GetKey(KeyCode.N))
        {
            currentGear = 0;
        }
        if (rpm > 5000 &&   currentGear != 0)
        {
            rpm -= 3000;
            currentGear++;
        }
        if (rpm < 2000 && currentGear > 1)
        {
            rpm += 3000;
            currentGear--;
        }
        if (currentGear == 0 && Input.GetKey(KeyCode.LeftShift))
        {
            currentGear++;
        }
        if (currentGear == 0 && m_verticalInput<0)
        {
            currentGear = -1;
        }
        
    }
    public void currentRpm()
    {
        if (rpm >= 6000 && currentGear==0)
            rpm -= 500;






            amount = rb.velocity.magnitude - temp;
            temp = rb.velocity.magnitude;
        rpm += amount*300;
        
       


    }

}
