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
            for (int x = -1; x <= gridSize.x; x++)
            {
                for (int z = -1; z <= gridSize.y; z++)
                {
                    if (x == -1 || z == -1 || x == gridSize.x || z == gridSize.y)
                    {
                        Vector3 wallPosition = new Vector3(_gridInitialPosition.x + x, _gridInitialPosition.y, _gridInitialPosition.z + z);
                        GameObject wallObj;
                        if ((x == gridSize.x - 1 && z == gridSize.y) /*|| (x == gridSize.x && z == gridSize.y - 1)*/)
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
                    else
                    {
                        Vector3 gridPosition = new Vector3(_gridInitialPosition.x + x, _gridInitialPosition.y, _gridInitialPosition.z + z);
                        GameObject gridObj;
                        if (x == gridSize.x - 1 && z == gridSize.y - 1)
                        {
                            gridObj = Instantiate(exitGridPrefab, gridPosition, Quaternion.identity); 
                        }
                        else
                        {
                            gridObj = Instantiate(defaultGridPrefab, gridPosition, Quaternion.identity);
                        }
                        gridObj.transform.parent = transform;
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
