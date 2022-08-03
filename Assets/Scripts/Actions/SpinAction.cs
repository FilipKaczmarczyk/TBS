using System;
using DG.Tweening;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        public void Spin(Action onSpinComplete)
        {
            IsActive = true;
            
            onActionComplete = onSpinComplete;
            
            transform.DOLocalRotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).
                SetRelative(true).SetEase(Ease.Linear).OnComplete(EndSpin);
        }

        private void EndSpin()
        {
            IsActive = false;
            
            onActionComplete();
        }
    }
}
