using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementControllerWithMultiple : MonoBehaviour
{
    [SerializeField] private Button craigBtn;
    [SerializeField] private Button patBtn;
    [SerializeField] private Button patBtn2;
    private GameObject placedPrefab;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>(); ChangePrefabTo("Craig");
        craigBtn.onClick.AddListener(() => ChangePrefabTo("interactableObject"));
        patBtn.onClick.AddListener(() => ChangePrefabTo("Tree"));
        patBtn2.onClick.AddListener(() => ChangePrefabTo("Oak_Tree"));
    }
    void Update()
    {
        if (placedPrefab == null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // First, check if the touch is on a UI element
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // Touch is on a UI element, so do nothing else (or handle UI touch specifically)
                return;
            }

            if (touch.phase == TouchPhase.Began)
            {
                var touchPosition = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                // Check if the raycast hit a game object tagged as "knight"
                if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("knight"))
                {
                    Destroy(hit.transform.gameObject);
                }
                // Perform AR raycast if the touch is not on UI and is on a plane
                else if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                {
                    var hitPose = hits[0].pose;
                    Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                }
            }
        }
    }

    void ChangePrefabTo(string prefabName)
    {
        placedPrefab = Resources.Load<GameObject>($"prefabs/{prefabName}");
        if (placedPrefab == null)
        { Debug.LogError($"Prefab with name {prefabName} could not be loaded, make sure you check the naming of your prefabs..."); }
        Color cc = craigBtn.image.color;
        Color pc = patBtn.image.color;
        Color pc2 = patBtn.image.color;
        switch (prefabName)
        {
            case "Craig":
                cc.a = 1f;
                pc.a = 0.5f;
                pc2.a = 0.5f;
                break;
            case "Patrick":
                pc.a = 1f;
                cc.a = 0.5f;
                pc2.a = 0.5f;
                break;
            
        }
        craigBtn.image.color = cc;
        patBtn.image.color = pc;
        patBtn2.image.color = pc2;
    }
}
