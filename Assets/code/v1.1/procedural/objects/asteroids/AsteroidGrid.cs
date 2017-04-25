using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    using AsteroidCellGrid = Dictionary<int, Dictionary<int, Dictionary<int, AsteroidCell>>>;
    using Grid_ZDefined = Dictionary<int, Dictionary<int, AsteroidCell>>;
    using Grid_YDefined = Dictionary<int, AsteroidCell>;

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
        public float GridSize = 5000.0f;
        public float MinDensity;
        public float MaxDensity;
        public float GenerationRadius;

        private System.Random _random = new System.Random(UniverseMap.GetSeed());
        private AsteroidCellGrid _grid = new AsteroidCellGrid();
        private Player _center;
        private float _player_radius;

        private AsteroidCell create(bool active)
        {
            GameObject go = Instantiate(Sources[_random.Next(0, Sources.Length)]);
            AsteroidCell cell = go.GetComponent<AsteroidCell>();

            cell.Size = GridSize;
            cell.TargetDensity = (float)(_random.NextDouble() * _random.NextDouble() * (MaxDensity + MinDensity) - MinDensity);

            go.transform.SetParent(transform);
            go.SetActive(active);

            return cell;
        }
        private AsteroidCell access(int x, int y, int z, bool active)
        {
            Grid_ZDefined get_z;
            Grid_YDefined get_y;
            AsteroidCell get_x;

            if (!_grid.TryGetValue(z, out get_z))
            {
                get_x = create(active);
                get_x.Location = new int[3] { x, y, z };
                get_x.transform.position = new Vector3(x * GridSize, y * GridSize, z * GridSize);
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
                get_x.Init();

                get_y.Add(x, get_x);
            }

            return get_x;
        }

        private void Start()
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = GlobalFog;

            _center = FindObjectOfType<Player>();

            Camera cam = _center.GetComponentInChildren<Camera>();
            _player_radius = cam.farClipPlane;
            int radius = Mathf.CeilToInt(_player_radius / (2 * GridSize));

            for ( int z = -radius; z<=radius;z++ )
            {
                for (int y = -radius; y <= radius; y++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        if (CheckOverlap(_center.transform.position, _player_radius, new Vector3(x*GridSize, y * GridSize, z * GridSize), GridSize))
                        {
                            access(x, y, z, true);
                        }
                    }
                }
            }
        }
        private void Update()
        {
            RenderSettings.fogDensity = GlobalFog * _center.cameraSystem.interpolation + GlobalFogScanner * (1 - _center.cameraSystem.interpolation);
        }
    }
}