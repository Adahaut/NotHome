using UnityEngine;

public class SplineTerrainModifier : MonoBehaviour
{
    public Terrain terrain;
    public Transform[] controlPoints;

    void Start()
    {
        ModifyTerrainAlongSpline();
    }

    void ModifyTerrainAlongSpline()
    {
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, resolution, resolution);

        foreach (Transform point in controlPoints)
        {
            Vector3 terrainPos = point.position - terrain.transform.position;
            int x = Mathf.RoundToInt((terrainPos.x / terrainData.size.x) * resolution);
            int z = Mathf.RoundToInt((terrainPos.z / terrainData.size.z) * resolution);

            heights[z, x] = terrainPos.y / terrainData.size.y;
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
