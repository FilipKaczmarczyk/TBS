using UnityEngine;
using UnityEngine.UI;

namespace UI.WorldUI
{
    public class UnitWorldUIActionPoint : MonoBehaviour
    {
        [SerializeField] private Sprite actionPointFull;
        [SerializeField] private Sprite actionPointEmpty;
        
        private Image _actionPointsImage;

        public void Awake()
        {
            _actionPointsImage = GetComponent<Image>();
        }

        public void FillImage()
        {
            _actionPointsImage.sprite = actionPointFull;
        }
        
        public void EmptyImage()
        {
            _actionPointsImage.sprite = actionPointEmpty;
        }
    }
}