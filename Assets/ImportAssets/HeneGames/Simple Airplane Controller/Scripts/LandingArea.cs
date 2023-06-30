using System.Collections;
using System.Collections.Generic;
using HeheGames.Simple_Airplane_Controller;
using StateMachine;
using UnityEngine;

public class LandingArea : MonoBehaviour
    {
        [SerializeField] private Runway runway;

        private void OnTriggerEnter(Collider other)
        {
            //Check if colliding object has airplane collider component
            if (other.transform.TryGetComponent(out SimpleAirPlaneCollider _airPlaneCollider))
            {
                //Calculate that the plane is coming from the right direction
                Vector3 dirFromLandingAreaToPlayerPlane = (transform.position - _airPlaneCollider.transform.position).normalized;
                float _directionFloat = Vector3.Dot(transform.forward, dirFromLandingAreaToPlayerPlane);

                //If direction is right start landing
                if (_directionFloat > 0.5f)
                {
                    AirPlaneController _controller = _airPlaneCollider.controller;

                    runway.landingAdjuster.position = _controller.transform.position;

                    runway.AddAirplane(_controller);
                    _controller.currentState = new LandState(_controller);
                    _controller.AddLandingRunway(runway);
                }
            }
        }
    }
