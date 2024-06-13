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
            ClearGrid();
            RoomParametersSetup();

            Vector3 floorPosition = new Vector3(_gridInitialPosition.x + (gridSize.x - 1) / 2.0f, _gridInitialPosition.y, _gridInitialPosition.z + (gridSize.y - 1) / 2.0f);
            GameObject floorObj = Instantiate(defaultGridPrefab, floorPosition, Quaternion.identity);
            floorObj.transform.localScale = new Vector3(gridSize.x, 0.1f, gridSize.y);
            floorObj.transform.parent = transform;

            Renderer floorRenderer = floorObj.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
               floorRenderer.sharedMaterial.mainTextureScale = new Vector2(gridSize.x, gridSize.y);

            }

            for (int x = -1; x <= gridSize.x; x++)
            {
                for (int z = -1; z <= gridSize.y; z++)
                {
                    if (x == -1 || z == -1 || x == gridSize.x || z == gridSize.y)
                    {
                        Vector3 wallPosition = new Vector3(_gridInitialPosition.x + x, _gridInitialPosition.y, _gridInitialPosition.z + z);
                        GameObject wallObj;

                        bool isDoorPosition = false;
                        switch (exitPosition)
                        {
                            case ExitPosition.Top:
                                isDoorPosition = z == gridSize.y && x == gridSize.x / 2;
                                break;
                            case ExitPosition.Bottom:
                                isDoorPosition = z == -1 && x == gridSize.x / 2;
                                break;
                            case ExitPosition.Left:
                                isDoorPosition = x == -1 && z == gridSize.y / 2;
                                break;
                            case ExitPosition.Right:
                                isDoorPosition = x == gridSize.x && z == gridSize.y / 2;
                                break;
                        }

                        if (isDoorPosition)
                        {
                            windowPrefab = exitVariants[Random.Range(0, exitVariants.Length)];
                            wallObj = Instantiate(windowPrefab, wallPosition + Vector3.up * _windowOffsetY, Quaternion.identity);
                        }
                        else
                        {
                            wallObj = Instantiate(wallPrefab, wallPosition + Vector3.up * _wallOffsetY, Quaternion.identity);
                        }

                        wallObj.transform.parent = transform;
                    }
                }
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
