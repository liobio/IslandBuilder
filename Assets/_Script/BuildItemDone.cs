using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildItemDone : MonoBehaviour
{

    public string parentName;
    public static BuildItemDone Create(Vector3 worldPosition, Vector2Int origin, BuildItemSO.Dir dir, BuildItemSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        BuildItemDone placedObject = placedObjectTransform.GetComponent<BuildItemDone>();
        if (placedObject.parentName != null || placedObject.parentName == "")
            placedObjectTransform.parent = GameObject.Find(placedObject.parentName).transform;
        placedObject.Setup(placedObjectTypeSO, origin, dir);

        return placedObject;
    }




    private BuildItemSO placedObjectTypeSO;
    public Vector2Int origin;
    private BuildItemSO.Dir dir;

    private void Setup(BuildItemSO placedObjectTypeSO, Vector2Int origin, BuildItemSO.Dir dir)
    {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return placedObjectTypeSO.nameString;
    }

}
