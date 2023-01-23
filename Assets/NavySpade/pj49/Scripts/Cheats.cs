using NavySpade.pj49.Scripts.Cops;
using NavySpade.pj49.Scripts.Navigation;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class Cheats : MonoBehaviour
    {
        public KeyCode HoldToStopPolice;
        public KeyCode HoldToStopUnits;

        private void Update()
        {
            if (Input.GetKeyDown(HoldToStopPolice))
            {
                foreach (var aiNavPointMovement in AINavPointMovement.Active)
                {
                    if(aiNavPointMovement.GetComponent<Cop>() == null)
                        continue;

                    aiNavPointMovement.IsStayForewer = true;
                }
            }
            if (Input.GetKeyUp(HoldToStopPolice))
            {
                foreach (var aiNavPointMovement in AINavPointMovement.Active)
                {
                    if(aiNavPointMovement.GetComponent<Cop>() == null)
                        continue;

                    aiNavPointMovement.IsStayForewer = false;
                }
            }
            
            if (Input.GetKeyDown(HoldToStopUnits))
            {
                foreach (var aiNavPointMovement in AINavPointMovement.Active)
                {
                    if(aiNavPointMovement.GetComponent<CollectingUnit>() == null)
                        continue;

                    aiNavPointMovement.IsStayForewer = true;
                }
            }
            if (Input.GetKeyUp(HoldToStopUnits))
            {
                foreach (var aiNavPointMovement in AINavPointMovement.Active)
                {
                    if(aiNavPointMovement.GetComponent<CollectingUnit>() == null)
                        continue;

                    aiNavPointMovement.IsStayForewer = false;
                }
            }
        }
    }
}