using System;
using System.Collections;
using System.Collections.Generic;
using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using UnityEngine;
using StarterAssets;

public class Diggggg : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip;
    public GameObject gg;
    [Header("Async parameters")]
    [Tooltip("Enable to edit the terrain asynchronously and avoid impacting the frame rate too much.")]
    public bool editAsynchronously = true;
    [SerializeField] Camera camera;
    [SerializeField] GameObject g;

    [Header("Modification parameters")]
    public BrushType brush = BrushType.Sphere;
    public ActionType action = ActionType.Dig;
    [Range(0, 7)] public int textureIndex;
    [Range(0.5f, 10f)] public float size = 4f;
    [Range(0f, 2f)] public float opacity = 0.5f;

    [Header("Persistence parameters (make sure persistence is enabled in Digger Master Runtime)")]
    public KeyCode keyToPersistData = KeyCode.P;

    public KeyCode keyToDeleteData = KeyCode.K;

    private DiggerMasterRuntime diggerMasterRuntime;
    private DiggerNavMeshRuntime diggerNavMeshRuntime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
        if (!diggerMasterRuntime)
        {
            enabled = false;
            Debug.LogWarning(
                "DiggerRuntimeUsageExample component requires DiggerMasterRuntime component to be setup in the scene. DiggerRuntimeUsageExample will be disabled.");
        }
        diggerNavMeshRuntime = FindObjectOfType<DiggerNavMeshRuntime>();
        if (!diggerNavMeshRuntime)
        {
            enabled = false;
            Debug.LogWarning("DiggerNavMeshUsageExample requires DiggerNavMeshRuntime component to be setup in the scene. DiggerNavMeshUsageExample will be disabled.");
            return;
        }

        // this is mandatory and should be called only once in a Start method
        diggerNavMeshRuntime.CollectNavMeshSources();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {           
            // Perform a raycast to find terrain surface and call Modify method of DiggerMasterRuntime to edit it
            if (Physics.Raycast(g.transform.position, g.transform.forward, out var hit, 2000f))
            {
                StartCoroutine(DigTerrain(hit));
            }
        }

        if (Input.GetKeyDown(keyToPersistData))
        {
            diggerMasterRuntime.PersistAll();
#if !UNITY_EDITOR
                Debug.Log("Persisted all modified chunks");
#endif
        }
        else if (Input.GetKeyDown(keyToDeleteData))
        {
            diggerMasterRuntime.DeleteAllPersistedData();
#if !UNITY_EDITOR
                Debug.Log("Deleted all persisted data");
#endif
        }
    }

    IEnumerator DigTerrain(RaycastHit hit)
    {
        gg.SetActive(true);
        GetComponent<ThirdPersonController>().enabled = false;
        animator.SetTrigger("Dig");
        yield return new WaitForSeconds((clip.length/0.25f));

        if (editAsynchronously)
        {
            diggerMasterRuntime.ModifyAsyncBuffured(hit.point, brush, action, textureIndex, opacity, size);
        }
        else
        {
            diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);
        }
        diggerNavMeshRuntime.UpdateNavMeshAsync(() => Debug.Log("NavMesh has been updated."));
        Debug.Log("NavMesh is updating...");
        GetComponent<ThirdPersonController>().enabled = true;
        gg.SetActive(false);
    }

    
}
