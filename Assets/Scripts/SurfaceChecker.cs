using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class SurfaceChecker : MonoBehaviour
{

    public ARRaycastManager arRayCastMng;
    public bool canPlace;
    private Pose placementPose;

    public GameObject placementIndicator;
    public GameObject spawnObject;
    public GameObject nametxt;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdateTargetIndicator();

        if (canPlace && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // the player touches the screen on the prefab
            Instantiate(spawnObject, placementPose.position, placementPose.rotation);
            
        }



    }

    private void UpdateTargetIndicator()
    {
        if (canPlace)
        {
            //show the placement Indicator
            //update position and rotation base on the placementPose
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            nametxt.transform.SetPositionAndRotation(placementPose.position+new Vector3(0f,2f,0f), placementPose.rotation);
        }
        else
        {
            //hide the placement indicator
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        arRayCastMng.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        canPlace = hits.Count > 0;

        if (canPlace)
        {
            placementPose = hits[0].pose;

            Vector3 cameraForward = Camera.current.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }
}
