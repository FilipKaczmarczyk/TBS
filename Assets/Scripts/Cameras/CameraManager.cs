using System;
using Actions;
using UnityEngine;

namespace Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject actionCameraGameObject;

        private void Start()
        {
            BaseAction.OnActionStarted += BaseAction_OnActionStarted;
            BaseAction.OnActionEnded += BaseAction_OnActionEnded;
        }

        private void ShowActionCamera()
        {
            actionCameraGameObject.SetActive(true);
        }
    
        private void HideActionCamera()
        {
            actionCameraGameObject.SetActive(false);
        }

        private void BaseAction_OnActionStarted(object sender, EventArgs e)
        {
            if (sender is ShootAction shootAction)
            {
                var shooterUnit = shootAction.Unit;
                var targetUnit = shootAction.GetTargetUnit();
            
                var cameraCharacterHeight = Vector3.up * 1.65f;

                var shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                var shoulderOffsetAmount = 0.5f;
                var shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                var actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
            
                ShowActionCamera();
            }
        }

        private void BaseAction_OnActionEnded(object sender, EventArgs e)
        {
            if (sender is ShootAction)
            {
                HideActionCamera();
            }
        }
    }
}
