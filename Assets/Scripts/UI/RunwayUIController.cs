using UnityEngine;
using TMPro;

namespace GameUI
{
    public class RunwayUIController : MonoBehaviour
    {
        [SerializeField] private Runway.LandingAreas.Runway runway;
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private GameObject uiContent;

        private void Update()
        {
            if(runway.AirplaneIsLanding())
            {
                uiContent.SetActive(true);
                debugText.text = "Airplane is landing";
            }
            else if(runway.AirplaneLandingCompleted())
            {
                uiContent.SetActive(true);
                debugText.text = "Press space to launch";
            }
            else if(runway.AirplaneIsTakingOff())
            {
                uiContent.SetActive(true);
                debugText.text = "Airplane is taking off";
            }
            else
            {
                uiContent.SetActive(false);
                debugText.text = "";
            }
        }
    }
}