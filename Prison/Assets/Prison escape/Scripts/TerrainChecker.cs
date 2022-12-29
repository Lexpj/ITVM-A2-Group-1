/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChecker : MonoBehaviour
{
	private float[] GetTextureMix(Vector3 playerPos, Terrain t)
	{
		Vector3 tPos = t.transform.position;
		TerrainData tData = t.terraindata;

		int mapX = Mathf.RoundToInt((playerPos.x - tPos.x) / tData.size.x * tData.alphamapWidth);
		int mapZ = Mathf.RoundToInt((playerPos.z - tPos.z) / tData.size.x * tData.alphamapWidth);
		float[..] splatMapData = tData.GetAlphamaps(mapX, mapZ, 1, 1);

		float[] cellmix = new float[splatMapData.GetUpperBound(2) + 1];
	}
}
*/