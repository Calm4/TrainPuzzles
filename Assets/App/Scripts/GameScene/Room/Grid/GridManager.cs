using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.GameScene.Room
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GameObject defaultGridPrefab;
        [SerializeField] private GameObject exitGridPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject windowPrefab;
        [SerializeField] Vector2Int gridSize;
        [SerializeField] private GameObject[] exitVariants;
        [SerializeField] private ExitPosition exitPosition;

        private RoomParameters _roomParameters;

        void Start()
        {
            GenerateGrid();
        }

        private void RoomParametersSetup()
        {
            _roomParameters = new RoomParameters(defaultGridPrefab, wallPrefab, windowPrefab);
        }

        [Button]
        private void GenerateGrid()
        {
            ClearGrid();
            RoomParametersSetup();

            var floorGenerator = new FloorGenerator(defaultGridPrefab, transform, gridSize, _roomParameters);
            floorGenerator.GenerateFloor();

            var wallGenerator = new WallGenerator(wallPrefab, exitVariants, transform, gridSize, exitPosition, _roomParameters);
            wallGenerator.GenerateWalls();
        }

        [Button]
        private void ClearGrid()
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }

            foreach (GameObject child in children)
            {
                DestroyImmediate(child);
            }
        }
    }
}