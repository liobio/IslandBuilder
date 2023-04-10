using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostObject : MonoBehaviour
{
    private Transform selectItem;
    private BuildItemSO placedObjectTypeSO;
    public Material red;
    public Material blue;
    private MeshRenderer ghost;
    private void Start()
    {
        GridSystem.instance.OnSelectedChanged += Instance_OnSelectedChanged;

    }
    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshSelect();
    }
    private void LateUpdate()
    {
        Vector3 targetPosition = GridSystem.instance.GetMouseWorldSnappedPosition("Grid");
        if (transform.childCount > 0)
        {
            ghost = transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>();
            GridSystem.instance.grid.GetGridXZ(targetPosition, out int x, out int z);
            Vector2Int placedObjectOriginGirdPos = GridSystem.instance.grid.GridIntXZ(new Vector2Int(x, z));
            if (!GridSystem.instance.grid.GetGridObject(placedObjectOriginGirdPos.x, placedObjectOriginGirdPos.y).CanBuild())
            {
                Material[] materials = ghost.materials;
                materials[0] = red;
                ghost.materials = materials;
            }
            else
            {
                Material[] materials = ghost.materials;
                materials[0] = blue;
                ghost.materials = materials;
            }
        }

        targetPosition.y = 1f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, GridSystem.instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }
    private void RefreshSelect()
    {
        if (selectItem != null)
        {
            Destroy(selectItem.gameObject);
            selectItem = null;
        }

        placedObjectTypeSO = GridSystem.instance.placedObjectTypeSO;

        if (placedObjectTypeSO != null)
        {
            selectItem = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            selectItem.parent = transform;
            selectItem.localPosition = Vector3.zero;
            selectItem.localEulerAngles = Vector3.zero;
            SetLayerRecursive(selectItem.gameObject, "Tile");
        }
    }
    private void SetLayerRecursive(GameObject targetGameObject, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layerName);
        }
    }

}
