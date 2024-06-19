using UnityEngine;

namespace App.Scripts.GameScene.Room
{
    public class RoomParameters
    {
        public Vector3 GridInitialPosition { get; private set; }
        public float WallOffsetY { get; private set; }
        public float WindowOffsetY { get; private set; }

        public RoomParameters(GameObject defaultGridPrefab, GameObject wallPrefab, GameObject windowPrefab)
        {
            GridInitialPosition = new Vector3(0.5f, defaultGridPrefab.transform.position.y, 0.5f);
            WallOffsetY = wallPrefab.transform.localScale.y / 2;
            WindowOffsetY = windowPrefab.transform.localScale.y / 2;
        }
    }
}