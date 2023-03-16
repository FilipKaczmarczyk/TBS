using Actions;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    private void OnEnable()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void OnDisable()
    {
        ShootAction.OnAnyShoot -= ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }
}
