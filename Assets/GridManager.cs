using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    public Transform gridTile;

    [Header("Grid Options")]
    public Vector2Int gridSize;
    public Vector2 spacing;

    [Header("Material Options")]
    public Material roadMaterial;
    public Material sidewalkMaterial;
    public Material houseGridTile;
    public Material spawnGridTile;

    public GridTile[] tiles;

    public Color red;
    public Color blue;
    public Color green;
    public Color yellow;

     public Color redLine;
    public Color blueLine;
    public Color greenLine;
    public Color yellowLine;

    // public static void MakeRoads() {
    //     GameObject[] objects = Selection.gameObjects;
    //     foreach (GameObject go in objects) {
    //         go.GetComponent<GridTile>().type = GridTileType.Road;
    //     }
    // }

    // public static void MakeSideWalk() {
    //     GameObject[] objects = Selection.gameObjects;
    //     foreach (GameObject go in objects) {
    //         go.GetComponent<GridTile>().type = GridTileType.Sidewalk;
    //     }
    // }
    

    // public static void GenerateTiles() {
        
    //     Vector3 origin = Vector3.zero;

    //     if (Selection.activeTransform.GetComponent<GridManager>()) {

    //         GridManager manager = Selection.activeTransform.GetComponent<GridManager>();

    //         manager.tiles = new GridTile[manager.gridSize.x * manager.gridSize.y];

    //         for (int x = 0; x < manager.gridSize.x; x++) {
    //             for (int y = 0; y < manager.gridSize.y; y++) {
    //                 Vector3 pos = new Vector3(origin.x + manager.spacing.x * x, 0, origin.z + manager.spacing.y * y);
    //                 Transform tile = Transform.Instantiate(manager.gridTile, pos, Quaternion.identity);
                    
    //                 manager.tiles[manager.gridSize.x * x + y] = tile.GetComponent<GridTile>();
                    
    //                 tile.SetParent(manager.transform);
    //             }
    //         }

    //     }

    // } 

}
