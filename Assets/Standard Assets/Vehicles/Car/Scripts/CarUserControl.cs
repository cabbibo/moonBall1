using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = 100 *CrossPlatformInputManager.GetAxis("LeftStickX");
            float v = 10000 * (Input.GetAxis("R2"));//CrossPlatformInputManager.GetAxis("R2");

            float handbrake = Input.GetAxis("[]");
            m_Car.Move(h, v, v, handbrake);

        }
    }
}
