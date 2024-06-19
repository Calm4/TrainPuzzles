using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.GameScene.Room
{
    public class WallGenerator
    {
        private readonly GameObject _wallPrefab;
        private readonly GameObject[] _exitVariants;
        private readonly Transform _parentTransform;
        private readonly Vector2Int _gridSize;
        private readonly ExitPosition _exitPosition;
        private readonly RoomParameters _roomParameters;

        public WallGenerator(GameObject wallPrefab, GameObject[] exitVariants, Transform parentTransform, Vector2Int gridSize, ExitPosition exitPosition, RoomParameters roomParameters)
        {
            _wallPrefab = wallPrefab;
            _exitVariants = exitVariants;
            _parentTransform = parentTransform;
            _gridSize = gridSize;
            _exitPosition = exitPosition;
            _roomParameters = roomParameters;
        }

        public void GenerateWalls()
        {
            for (int x = -1; x <= _gridSize.x; x++)
            {
                for (int z = -1; z <= _gridSize.y; z++)
                {
                    if (IsEdgePosition(x, z) && !IsCornerPosition(x, z))
                    {
                        Vector3 wallPosition = new Vector3(_roomParameters.GridInitialPosition.x + x, _roomParameters.GridInitialPosition.y,
                            _roomParameters.GridInitialPosition.z + z);
                        GameObject wallObj;

                        if (IsDoorPosition(x, z, out Quaternion doorRotation))
                        {
                            wallObj = Object.Instantiate(_exitVariants[Random.Range(0, _exitVariants.Length)],
                                wallPosition + Vector3.up * _roomParameters.WindowOffsetY, doorRotation);
                        }
                        else
                        {
                            GetWallTransformData(x, z, out Quaternion rotation, out Vector3 positionOffset);
                            wallObj = Object.Instantiate(_wallPrefab, wallPosition + Vector3.up * _roomParameters.WallOffsetY + positionOffset,
                                rotation);
                        }

                        wallObj.transform.parent = _parentTransform;
                    }
                }
            }
        }

        private bool IsEdgePosition(int x, int z)
        {
            return x == -1 || z == -1 || x == _gridSize.x || z == _gridSize.y;
        }

        private bool IsCornerPosition(int x, int z)
        {
            return (x == -1 && z == -1) || (x == _gridSize.x && z == -1) ||
                   (x == -1 && z == _gridSize.y) || (x == _gridSize.x && z == _gridSize.y);
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
            int midX = _gridSize.x / 2;
            int midZ = _gridSize.y / 2;

            if (_exitPosition == ExitPosition.Top && z == _gridSize.y && x == midX ||
                _exitPosition == ExitPosition.Bottom && z == -1 && x == midX ||
                _exitPosition == ExitPosition.Left && x == -1 && z == midZ ||
                _exitPosition == ExitPosition.Right && x == _gridSize.x && z == midZ)
            {
                rotation = doorRotations[_exitPosition];
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
                { (_gridSize.x, 0), (Quaternion.Euler(0, 90, 0), new Vector3(-0.4f, 0, 0)) },
                { (0, -1), (Quaternion.identity, new Vector3(0, 0, 0.4f)) },
                { (0, _gridSize.y), (Quaternion.identity, new Vector3(0, 0, -0.4f)) }
            };

            if (x == -1)
            {
                (rotation, positionOffset) = wallTransformData[(-1, 0)];
            }
            else if (x == _gridSize.x)
            {
                (rotation, positionOffset) = wallTransformData[(_gridSize.x, 0)];
            }
            else if (z == -1)
            {
                (rotation, positionOffset) = wallTransformData[(0, -1)];
            }
            else if (z == _gridSize.y)
            {
                (rotation, positionOffset) = wallTransformData[(0, _gridSize.y)];
            }
        }
    }
}