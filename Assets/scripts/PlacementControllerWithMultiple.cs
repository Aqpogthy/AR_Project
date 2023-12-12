using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ARRaycastManager))]
public class PlacementControllerWithMultiple : MonoBehaviour
{
    [SerializeField] private Button AppleBtn;
    [SerializeField] private Button AppleTreeBtn;
    [SerializeField] private Button TreeBtn;
    private GameObject placedPrefab;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>(); ChangePrefabTo("Apple");
        AppleBtn.onClick.AddListener(() => ChangePrefabTo("Apple"));
        AppleTreeBtn.onClick.AddListener(() => ChangePrefabTo("AppleTree"));
        TreeBtn.onClick.AddListener(() => ChangePrefabTo("Tree"));
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
        Color a = AppleBtn.image.color;
        Color at = AppleTreeBtn.image.color;
        Color t = TreeBtn.image.color;
        switch (prefabName)
        {
            case "Apple":
                a.a = 1f;
                at.a = 0.5f;
                t.a = 0.5f;
                break;
            case "AppleTree":
                a.a = 0.5f;
                at.a = 1f;
                t.a = 0.5f;
                break;
            case "Tree":
                a.a = 0.5f;
                at.a = 0.5f;
                t.a = 1f;
                break;
        }
        AppleBtn.image.color = a;
        AppleTreeBtn.image.color = at;
        TreeBtn.image.color = t;
    }
}
