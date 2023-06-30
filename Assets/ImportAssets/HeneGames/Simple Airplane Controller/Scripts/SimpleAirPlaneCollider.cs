using System.Collections;
using System.Collections.Generic;
using HeheGames.Simple_Airplane_Controller;
using UnityEngine;


    public class SimpleAirPlaneCollider : MonoBehaviour
    {
        public bool collideSometing;

        [HideInInspector]
        public AirPlaneController controller;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<AirPlaneController>() == null && other.gameObject.GetComponent<LandingArea>() == null)
            {
                collideSometing = true;
            }
        }
    }