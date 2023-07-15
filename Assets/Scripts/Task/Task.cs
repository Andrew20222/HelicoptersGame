using TMPro;
using UnityEngine;

namespace Views.Task
{
    public class Task : MonoBehaviour
    {
        [SerializeField] private TMP_Text viewTask;

        public void ShowTask()
        {
            viewTask.text = "Collect all the checkpoints";
        }
    }
}