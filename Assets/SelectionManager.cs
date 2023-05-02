using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class SelectionManager : MonoBehaviour
{
    public int lastSortingOrder = 1;
    public LayerMask mask;
    private List<GridTile> selectedTiles = new List<GridTile>();

    GridTile lastSelected;

    private bool drawingPath;
    private LineRenderer pathLine;

    public float selectionDistanceTolerance = 2f;

    public Transform pathTraceInstance;


    private void SelectNew(GridTile newTile)
    {

        if (lastSelected != null)
        {
            lastSelected.Unselect();
        }

        if (newTile != null)
        {
            newTile.Select();
        }

        lastSelected = newTile;
    }

    private bool isDrawing = false;
    private GridTile startingPoint;

    private void Deselect() {
        SelectNew(null);
        isDrawing = false;

        if (startingPoint && startingPoint.pathTrace)
            GameObject.Destroy(startingPoint.pathTrace.gameObject);

        startingPoint = null;
        selectedTiles.Clear();
    }

    private void Update()
    {

        // --
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {

                if (hit.transform.GetComponent<GridTile>())
                {
                    if (!selectedTiles.Contains(hit.transform.GetComponent<GridTile>()) && hit.transform.GetComponent<GridTile>().path.Count <= 0)
                    {
                        if (lastSelected != hit.transform.GetComponent<GridTile>())
                        {
                            if (lastSelected == null || Vector3.Distance(hit.transform.GetComponent<GridTile>().transform.position, lastSelected.transform.position) <= selectionDistanceTolerance)
                            {


                                if (!isDrawing && hit.transform.GetComponent<GridTile>().type == GridTileType.StartingPoint)
                                {
                                    isDrawing = true;
                                    Transform pathTrace = Transform.Instantiate(pathTraceInstance);
                                    hit.transform.GetComponent<GridTile>().StartTrace(pathTrace);
                                    startingPoint = hit.transform.GetComponent<GridTile>();

                                    ColorSide side = hit.transform.GetComponent<GridTile>().side;
                                    
                                    GridManager manager = GameObject.FindObjectOfType<GridManager>();
                                    switch (side) {
                                        case ColorSide.Blue:
                                            pathTrace.GetComponentInChildren<Disc>().Color = manager.blueLine;
                                            pathTrace.GetComponentInChildren<Polyline>().Color = manager.blueLine;
                                            pathTrace.Find("EndDisc").GetComponent<Disc>().Color = manager.blueLine;
                                            break;
                                        case ColorSide.Red:
                                            pathTrace.GetComponentInChildren<Disc>().Color = manager.redLine;
                                            pathTrace.GetComponentInChildren<Polyline>().Color = manager.redLine;
                                            pathTrace.Find("EndDisc").GetComponent<Disc>().Color = manager.redLine;
                                            break;
                                        case ColorSide.Green:
                                            pathTrace.GetComponentInChildren<Disc>().Color = manager.greenLine;
                                            pathTrace.GetComponentInChildren<Polyline>().Color = manager.greenLine;
                                            pathTrace.Find("EndDisc").GetComponent<Disc>().Color = manager.greenLine;
                                            break;
                                        case ColorSide.Yellow:
                                            pathTrace.GetComponentInChildren<Disc>().Color = manager.yellowLine;
                                            pathTrace.GetComponentInChildren<Polyline>().Color = manager.yellowLine;
                                            pathTrace.Find("EndDisc").GetComponent<Disc>().Color = manager.yellowLine;
                                            break;  
                                    }

                                    pathTrace.GetComponentInChildren<Polyline>().SortingOrder = lastSortingOrder;
                                    lastSortingOrder++;

                                    SelectNew(hit.transform.GetComponent<GridTile>());
                                    selectedTiles.Add(hit.transform.GetComponent<GridTile>());

                                    Vector3 direction = hit.transform.GetComponent<GridTile>().transform.position - startingPoint.pathTrace.position;
                                    startingPoint.pathTrace.GetComponentInChildren<Polyline>().AddPoint(new Vector3(direction.x, -direction.z));

                                    startingPoint.path.Add(hit.transform.GetComponent<GridTile>());

                                }

                                if (isDrawing && hit.transform.GetComponent<GridTile>().type == GridTileType.Road)
                                {
                                    SelectNew(hit.transform.GetComponent<GridTile>());
                                    selectedTiles.Add(hit.transform.GetComponent<GridTile>());

                                    Vector3 direction = hit.transform.GetComponent<GridTile>().transform.position - startingPoint.pathTrace.position;
                                    startingPoint.pathTrace.GetComponentInChildren<Polyline>().AddPoint(new Vector3(direction.x, -direction.z));

                                    startingPoint.pathTrace.Find("EndDisc").transform.position = hit.transform.GetComponent<GridTile>().GetPathPoint();

                                    startingPoint.path.Add(hit.transform.GetComponent<GridTile>());
                                }

                            }

                        }
                    }
                }

            }
            else
            {
                if (startingPoint) startingPoint.path.Clear();
                Deselect();
            }
        }

        // --
        if (Input.GetMouseButtonUp(1) && isDrawing)
        {
            SelectNew(null);
            isDrawing = false;

            if (startingPoint)
            {
                GameObject.Destroy(startingPoint.pathTrace.gameObject);
            }

            startingPoint.path.Clear();

            startingPoint = null;
            selectedTiles.Clear();
        }

        if (Input.GetMouseButtonUp(0)) {
            isDrawing = false;
            SelectNew(null);

            if (startingPoint && startingPoint.path.Count > 0) {
                startingPoint.BeginTruckMovement();
            }

            startingPoint = null;
            selectedTiles.Clear();
        }

    }
}
