using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public enum GridTileType {

    Sidewalk,
    Road,
   
    House,
    StartingPoint
}

public enum ColorSide {
    Red,
    Blue,
    Green,
    Yellow
}

public class GridTile : MonoBehaviour
{
    public GridTileType type;
    public ColorSide side; 
    public Transform pathTrace;

    public Transform truckTransform;

    public List<GridTile> path = new List<GridTile>();

    public void Select() {
        GetComponentInChildren<Animator>().SetBool("selected", true);
    }

    public void Unselect() {
        GetComponentInChildren<Animator>().SetBool("selected", false);
    }

    public void StartTrace(Transform t) {
        pathTrace = t;
        pathTrace.position = GetPathPoint();

    }
    
    public Vector3 GetPathPoint () {
        return transform.position + Vector3.up * 0.1f;
    }

    private Color GetSideColor() {
        GridManager manager = GameObject.FindObjectOfType<GridManager>();
        switch (side) {
            case ColorSide.Blue:
                return manager.blue;
            case ColorSide.Red:
                return manager.red;
            case ColorSide.Green:
                return manager.green;
            case ColorSide.Yellow:
                return manager.yellow;
        }

        return Color.white;
    }

    private void Awake() {
        GridManager manager = GameObject.FindObjectOfType<GridManager>();

        if (type == GridTileType.StartingPoint)
            GetComponentInChildren<MeshRenderer>().material = manager.spawnGridTile;
        else if (type == GridTileType.Road)
            GetComponentInChildren<MeshRenderer>().material = manager.roadMaterial;

        if (truckTransform) truckTransform.GetComponent<Truck>().side = side;
        
        if (type == GridTileType.StartingPoint) {
            GetComponentInChildren<MeshRenderer>().material.color = GetSideColor();

        }

    }

    public void BeginTruckMovement() {
        GameManager.TruckStarted();
        StartCoroutine("TruckMovement");
    }

    public float truckMoveSpeed = 0.5f;
    public float rotateSpeed;
    public float scaleSpeed = 5;

    IEnumerator TruckMovement() {

        truckTransform.position = new Vector3(transform.position.x, truckTransform.position.y, transform.position.z);
        truckTransform.localScale = new Vector3(1,1,1);

        for (int i = 0; i < path.Count; i++) {

            Vector3 movePoint = path[i].transform.position;
            movePoint.y = truckTransform.position.y;
            float time = 0f;

            while (Vector3.Distance(truckTransform.position, movePoint) >= 0.01f && truckTransform.GetComponent<Truck>().collided == false) {

                truckTransform.rotation = Quaternion.Slerp(truckTransform.rotation, Quaternion.LookRotation((movePoint-truckTransform.position).normalized, truckTransform.up), rotateSpeed * Time.deltaTime );

                truckTransform.position = Vector3.Lerp(truckTransform.position, movePoint, time/truckMoveSpeed);
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }

        }
        
        if (truckTransform.GetComponent<Truck>().collided) {

            yield return new WaitForSeconds(4f);

        }

        // Path Completed (Dissapear maybe allow for new creation)

        float scale = 1f;


        while (scale >= 0) {
            scale -= Time.deltaTime * scaleSpeed;
            truckTransform.localScale = new Vector3(1, scale, 1);
            
            foreach (Disc d in pathTrace.GetComponentsInChildren<Disc>()) {
                d.Radius = scale;
            }

            pathTrace.GetComponentInChildren<Polyline>().Thickness = 0.125f * scale;

            yield return new WaitForEndOfFrame();
        }


        GameObject.Destroy(pathTrace.gameObject);
        
        truckTransform.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        
        truckTransform.gameObject.SetActive(true);
        truckTransform.position = new Vector3(transform.position.x, truckTransform.position.y, transform.position.z);

        scale = 0f;

        while (scale <= 1f) {
            scale += Time.deltaTime * scaleSpeed;
            truckTransform.localScale = new Vector3(1,scale,1);
            yield return new WaitForEndOfFrame();
        }

        truckTransform.localScale = new Vector3(1,1,1);
        path.Clear();
        

    }

}
