using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    using AsteroidCellGrid = Dictionary<int, Dictionary<int, Dictionary<int, AsteroidGridCell>>>;
    using Grid_ZDefined = Dictionary<int, Dictionary<int, AsteroidGridCell>>;
    using Grid_YDefined = Dictionary<int, AsteroidGridCell>;

    public class AsteroidGrid : MonoBehaviour {
        public static float SquareValue(float x) { return x * x; }
        public static bool CheckOverlap(Vector3 sphere_pos, float sphere_radius, Vector3 cube_pos, float cube_side)
        {
            Vector3 C1 = cube_pos;
            Vector3 C2 = cube_pos + new Vector3(cube_side, cube_side, cube_side);

            float dist_squared = sphere_radius * sphere_radius;
            
            if (sphere_pos.x < C1.x)
                dist_squared -= SquareValue(sphere_pos.x - C1.x);
            else if (sphere_pos.x > C2.x)
                dist_squared -= SquareValue(sphere_pos.x - C2.x);

            if (sphere_pos.y < C1.y)
                dist_squared -= SquareValue(sphere_pos.y - C1.y);
            else if (sphere_pos.y > C2.y)
                dist_squared -= SquareValue(sphere_pos.y - C2.y);

            if (sphere_pos.z < C1.z)
                dist_squared -= SquareValue(sphere_pos.z - C1.z);
            else if (sphere_pos.z > C2.z)
                dist_squared -= SquareValue(sphere_pos.z - C2.z);

            return dist_squared > 0;
        }

        public float GlobalFog = 0.01f;
        public float GlobalFogScanner = 0.01f;
        public GameObject[] Sources;
        public float[] Probabilities;
        public float GridSize = 5000.0f;

        private System.Random _random = new System.Random(UniverseMap.GetSeed());
        private AsteroidCellGrid _grid = new AsteroidCellGrid();
        private Player _center;
        private float _player_radius;
        private int[] _location;
        private int[] _dimensions;

        public float GetRandomValue()
        {
            return (float)_random.NextDouble();
        }
        public int GetRandomIntValue(int min, int max)
        {
            return _random.Next(min, max);
        }

        private AsteroidGridCell create(bool active)
        {
            float random = (float)_random.NextDouble();
            int selected = 0;
            for (int j = 0; j < Probabilities.Length; j++)
            {
                random -= Probabilities[j];
                if (random <= 0)
                {
                    selected = j;
                    break;
                }
            }

            GameObject go = Instantiate(Sources[selected]);
            AsteroidGridCell cell = go.GetComponent<AsteroidGridCell>();

            cell.ParentGrid = this;
            cell.Size = GridSize;

            go.transform.SetParent(transform);
            go.SetActive(active);

            return cell;
        }
        private AsteroidGridCell access(int x, int y, int z, bool active)
        {
            Grid_ZDefined get_z;
            Grid_YDefined get_y;
            AsteroidGridCell get_x;

            if (!_grid.TryGetValue(z, out get_z))
            {
                get_x = create(active);
                get_x.Location = new int[3] { x, y, z };
                get_x.transform.position = new Vector3(x * GridSize, y * GridSize, z * GridSize);
                if ( get_x.gameObject.activeInHierarchy )
                    get_x.Init();

                get_y = new Grid_YDefined();
                get_z = new Grid_ZDefined();

                get_y.Add(x, get_x);
                get_z.Add(y, get_y);
                _grid.Add(z, get_z);
            }
            else if (!get_z.TryGetValue(y, out get_y))
            {
                get_x = create(active);
                get_x.Location = new int[3] { x, y, z };
                get_x.transform.position = new Vector3(x * GridSize, y * GridSize, z * GridSize);
                if (get_x.gameObject.activeInHierarchy)
                    get_x.Init();

                get_y = new Grid_YDefined();

                get_y.Add(x, get_x);
                get_z.Add(y, get_y);
            }
            else if (!get_y.TryGetValue(x, out get_x))
            {
                get_x = create(active);
                get_x.Location = new int[3] { x, y, z };
                get_x.transform.position = new Vector3(x * GridSize, y * GridSize, z * GridSize);
                if (get_x.gameObject.activeInHierarchy)
                    get_x.Init();

                get_y.Add(x, get_x);
            }
            else
            {
                get_x.gameObject.SetActive(active);
            }

            return get_x;
        }

        private void Start()
        {
            _center = FindObjectOfType<Player>();

            Camera cam = _center.cameraSystem.PlayerCamera;
            _player_radius = cam.farClipPlane;
            int radius = Mathf.CeilToInt(_player_radius / GridSize);

            _location = new int[3] { 0, 0, 0 };
            _dimensions = new int[3] { radius, radius, radius };

            for ( int z = -radius; z<=radius;z++ )
                for (int y = -radius; y <= radius; y++)
                    for (int x = -radius; x <= radius; x++)
                        access(x, y, z, true);

            RenderSettings.fog = true;
            RenderSettings.fogColor = cam.backgroundColor;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = _player_radius * 0.25f;
            RenderSettings.fogEndDistance = _player_radius;
        }
        private void Update()
        {
            Camera cam = _center.cameraSystem.PlayerCamera;
            _player_radius = cam.farClipPlane;

            int[] new_location = new int[3]
            {
                Mathf.RoundToInt(_center.transform.position.x / GridSize),
                Mathf.RoundToInt(_center.transform.position.y / GridSize),
                Mathf.RoundToInt(_center.transform.position.z / GridSize)
            };
            
            if (new_location[0] != _location[0])
            {
                int direction = System.Math.Sign(new_location[0] - _location[0]);
                for (int z = -_dimensions[2]; z <= _dimensions[2]; z++)
                    for (int y = -_dimensions[1]; y <= _dimensions[1]; y++) {
                        access(_location[0] - _dimensions[0] * direction, _location[1] + y, _location[2] + z, false);
                        access(new_location[0] + _dimensions[0] * direction, new_location[1] + y, new_location[2] + z, true);
                    }
            }

            if (new_location[1] != _location[1])
            {
                int direction = System.Math.Sign(new_location[1] - _location[1]);
                for (int z = -_dimensions[2]; z <= _dimensions[2]; z++)
                    for (int x = -_dimensions[0]; x <= _dimensions[0]; x++)
                    {
                        access(_location[0] + x, _location[1] - _dimensions[1] * direction, _location[2] + z, false);
                        access(new_location[0] + x, new_location[1] + _dimensions[1] * direction, new_location[2] + z, true);
                    }
            }

            if (new_location[2] != _location[2])
            {
                int direction = System.Math.Sign(new_location[2] - _location[2]);
                for (int y = -_dimensions[1]; y <= _dimensions[1]; y++)
                    for (int x = -_dimensions[0]; x <= _dimensions[0]; x++)
                    {
                        access(_location[0] + x, _location[1] + y, _location[2] - _dimensions[2] * direction, false);
                        access(new_location[0] + x, new_location[1] + y, new_location[2] + _dimensions[2] * direction, true);
                    }
            }

            _location = new_location;

            //RenderSettings.fogDensity = GlobalFog * _center.cameraSystem.interpolation + GlobalFogScanner * (1 - _center.cameraSystem.interpolation);
            RenderSettings.fogStartDistance = _player_radius * 0.25f;
            RenderSettings.fogEndDistance = _player_radius;
        }
    }
}