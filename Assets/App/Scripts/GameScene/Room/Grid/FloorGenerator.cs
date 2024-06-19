using UnityEngine;

namespace App.Scripts.GameScene.Room
{
    public class FloorGenerator
    {
        private readonly GameObject _defaultGridPrefab;
        private readonly Transform _parentTransform;
        private readonly Vector2Int _gridSize;
        private readonly RoomParameters _roomParameters;

        public FloorGenerator(GameObject defaultGridPrefab, Transform parentTransform, Vector2Int gridSize, RoomParameters roomParameters)
        {
            _defaultGridPrefab = defaultGridPrefab;
            _parentTransform = parentTransform;
            _gridSize = gridSize;
            _roomParameters = roomParameters;
        }

        public void GenerateFloor()
        {
            Vector3 floorPosition = new Vector3(_roomParameters.GridInitialPosition.x + (_gridSize.x - 1) / 2.0f,
                _roomParameters.GridInitialPosition.y, _roomParameters.GridInitialPosition.z + (_gridSize.y - 1) / 2.0f);
            GameObject floorObj = Object.Instantiate(_defaultGridPrefab, floorPosition, Quaternion.identity);
            floorObj.transform.localScale = new Vector3(_gridSize.x, 0.1f, _gridSize.y);
            floorObj.transform.parent = _parentTransform;

            Renderer floorRenderer = floorObj.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorRenderer.sharedMaterial.mainTextureScale = new Vector2(_gridSize.x, _gridSize.y);
            }
        }
    }
}