using System.Collections;
using System.Collections.Generic;
using HeheGames.Simple_Airplane_Controller;
using StateMachine;
using StateMachines;
using UnityEngine;



    public class Runway : MonoBehaviour
    {
        private bool landingCompleted;
        private float landingSpeed;
        private AirPlaneController landingAirplaneController;
        private Vector3 landingAdjusterStartLocalPos;

        [Header("Input")]
        [SerializeField] private KeyCode launchKey = KeyCode.Space;

        [Header("Runway references")]
        public string runwayName = "Runway";
        [SerializeField] private LandingArea landingArea;
        public Transform landingAdjuster;
        [SerializeField] private Transform landingfinalPos;

        private void Start()
        {
            landingSpeed = 1f;
            landingAdjusterStartLocalPos = landingAdjuster.localPosition;
        }

        private void Update()
        {
            //Airplane is landing (Landing area add airplane controller reference)
            if(landingAirplaneController != null)
            {
                //Set airplane to landing adjuster child
                landingAirplaneController.transform.SetParent(landingAdjuster.transform);

                //Move landing adjuster to landing final pos position
                if(!landingCompleted)
                {
                    landingSpeed += Time.deltaTime;
                    landingAdjuster.localPosition = Vector3.Lerp(landingAdjuster.localPosition, landingfinalPos.localPosition, landingSpeed * Time.deltaTime);

                    float _distanceToLandingFinalPos = Vector3.Distance(landingAdjuster.position, landingfinalPos.position);
                    if (_distanceToLandingFinalPos < 0.1f)
                    {
                        landingCompleted = true;
                    }
                }
                else
                {
                    landingAdjuster.localPosition = Vector3.Lerp(landingAdjuster.localPosition, landingfinalPos.localPosition, landingSpeed * Time.deltaTime);

                    //Launch airplane
                    if (Input.GetKeyDown(launchKey))
                    {
                        landingAirplaneController.currentState = new TakeoffState(landingAirplaneController);
                    }

                    //Reset runway if landing airplane is taking off
                    if (landingAirplaneController.currentState == new FlyState(landingAirplaneController))
                    {
                        landingAirplaneController.transform.SetParent(null);
                        landingAirplaneController = null;
                        landingCompleted = false;
                        landingSpeed = 1f;
                        landingAdjuster.localPosition = landingAdjusterStartLocalPos;
                    }
                }
            }
        }

        //Landing area add airplane controller reference
        public void AddAirplane(AirPlaneController _simpleAirPlane)
        {
            landingAirplaneController = _simpleAirPlane;
        }

        [SerializeField] private AirStateMachine stateMachine;

        public bool AirplaneLandingCompleted()
        {
            if (landingAirplaneController != null)
            {
                if (landingAirplaneController.currentState != new LandState(landingAirplaneController))
                {
                    return landingCompleted;
                }
            }

            return false;
        }

        public bool AirplaneIsLanding()
        {
            if(landingAirplaneController != null && !landingCompleted)
            {
                return true;
            }

            return false;
        }

        public bool AriplaneIsTakingOff()
        {
            if (landingAirplaneController != null)
            {
                if(landingAirplaneController.currentState == new TakeoffState(landingAirplaneController))
                {
                    return true;
                }
            }

            return false;
        }
    }
