using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.GameScene.Room
{
    public enum ExitPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GameObject defaultGridPrefab;
        [SerializeField] private GameObject exitGridPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject windowPrefab;
        [SerializeField] Vector2Int gridSize;
        [SerializeField] private GameObject[] exitVariants;
        [SerializeField] private ExitPosition exitPosition;

        private Vector3 _gridInitialPosition;
        private float _wallOffsetY;
        private float _windowOffsetY;

        void Start()
        {
            // GenerateGrid();
        }

        private void RoomParametersSetup()
        {
            _gridInitialPosition = new Vector3(0.5f, defaultGridPrefab.transform.position.y, 0.5f);
            _wallOffsetY = wallPrefab.transform.localScale.y / 2;
            _windowOffsetY = windowPrefab.transform.localScale.y / 2;
        }

        [Button]
        private void GenerateGrid()
        {
            // Создать 2 отдельных класса для генерации - пола и стен
            ClearGrid();
            RoomParametersSetup();

            GenerateFloor();
            GenerateWalls();
        }

        private void GenerateWalls()
        {
            for (int x = -1; x <= gridSize.x; x++)
            {
                for (int z = -1; z <= gridSize.y; z++)
                {
                    if (IsEdgePosition(x, z) && !IsCornerPosition(x, z))
                    {
                        Vector3 wallPosition = new Vector3(_gridInitialPosition.x + x, _gridInitialPosition.y,
                            _gridInitialPosition.z + z);
                        GameObject wallObj;

                        if (IsDoorPosition(x, z, out Quaternion doorRotation))
                        {
                            wallObj = Instantiate(exitVariants[Random.Range(0, exitVariants.Length)],
                                wallPosition + Vector3.up * _windowOffsetY, doorRotation);
                        }
                        else
                        {
                            GetWallTransformData(x, z, out Quaternion rotation, out Vector3 positionOffset);
                            wallObj = Instantiate(wallPrefab, wallPosition + Vector3.up * _wallOffsetY + positionOffset,
                                rotation);
                        }

                        wallObj.transform.parent = transform;
                    }
                }
            }
        }

        private bool IsEdgePosition(int x, int z)
        {
            return x == -1 || z == -1 || x == gridSize.x || z == gridSize.y;
        }

        private bool IsCornerPosition(int x, int z)
        {
            return (x == -1 && z == -1) || (x == gridSize.x && z == -1) ||
                   (x == -1 && z == gridSize.y) || (x == gridSize.x && z == gridSize.y);
        }

        private bool IsDoorPosition(int x, int z, out Quaternion rotation)
        {
            var doorRotations = new Dictionary<ExitPosition, Quaternion>
            {
                { ExitPosition.Top, Quaternion.Euler(0, 270, 0) },
                { ExitPosition.Bottom, Quaternion.Euler(0, 90, 0) },
                { ExitPosition.Left, Quaternion.Euler(0, 180, 0) },
                { ExitPosition.Right, Quaternion.Euler(0, 0, 0) }
            };

            rotation = Quaternion.identity;
            int midX = gridSize.x / 2;
            int midZ = gridSize.y / 2;

            if (exitPosition == ExitPosition.Top && z == gridSize.y && x == midX ||
                exitPosition == ExitPosition.Bottom && z == -1 && x == midX ||
                exitPosition == ExitPosition.Left && x == -1 && z == midZ ||
                exitPosition == ExitPosition.Right && x == gridSize.x && z == midZ)
            {
                rotation = doorRotations[exitPosition];
                return true;
            }

            return false;
        }

        private void GetWallTransformData(int x, int z, out Quaternion rotation, out Vector3 positionOffset)
        {
            rotation = Quaternion.identity;
            positionOffset = Vector3.zero;

            var wallTransformData = new Dictionary<(int, int), (Quaternion, Vector3)>
            {
                { (-1, 0), (Quaternion.Euler(0, 90, 0), new Vector3(0.4f, 0, 0)) },
                { (gridSize.x, 0), (Quaternion.Euler(0, 90, 0), new Vector3(-0.4f, 0, 0)) },
                { (0, -1), (Quaternion.identity, new Vector3(0, 0, 0.4f)) },
                { (0, gridSize.y), (Quaternion.identity, new Vector3(0, 0, -0.4f)) }
            };

            if (x == -1)
            {
                (rotation, positionOffset) = wallTransformData[(-1, 0)];
            }
            else if (x == gridSize.x)
            {
                (rotation, positionOffset) = wallTransformData[(gridSize.x, 0)];
            }
            else if (z == -1)
            {
                (rotation, positionOffset) = wallTransformData[(0, -1)];
            }
            else if (z == gridSize.y)
            {
                (rotation, positionOffset) = wallTransformData[(0, gridSize.y)];
            }
        }


        private void GenerateFloor()
        {
            Vector3 floorPosition = new Vector3(_gridInitialPosition.x + (gridSize.x - 1) / 2.0f,
                _gridInitialPosition.y, _gridInitialPosition.z + (gridSize.y - 1) / 2.0f);
            GameObject floorObj = Instantiate(defaultGridPrefab, floorPosition, Quaternion.identity);
            floorObj.transform.localScale = new Vector3(gridSize.x, 0.1f, gridSize.y);
            floorObj.transform.parent = transform;

            Renderer floorRenderer = floorObj.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorRenderer.sharedMaterial.mainTextureScale = new Vector2(gridSize.x, gridSize.y);
            }
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