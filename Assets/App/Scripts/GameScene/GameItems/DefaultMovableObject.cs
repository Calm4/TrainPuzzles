using App.Scripts.GameScene.Interfaces;
using UnityEngine;

namespace App.Scripts.GameScene.GameItems
{
    public enum MovementAxis
    {
        Horizontal,
        Vertical
    }
    public class DefaultMovableObject : MonoBehaviour, IMovable
    {
        [SerializeField] private MovementAxis movementAxis;
        private BoxCollider _boxCollider;

        private void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        public BoxCollider GetBoxCollider()
        {
            return _boxCollider;
        }

        public MovementAxis GetMovableObjectMovementAxis()
        {
            return movementAxis;
        }
    }
}
