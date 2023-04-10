using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey.Utils;
using static UnityEngine.Rendering.DebugUI;

public class GridSystem : Singleton<GridSystem>
{
    public int gridLength = 10;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 2;

    public bool isShowDeubg = true;
    public BuildItemSO placedObjectTypeSO;
    public GridXYZ<GridObject> grid;

    private BuildItemSO.Dir dir;

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;
    [SerializeField] private List<BuildItemSO> buildItemSOList = null;

    public Text debug;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        ObserverManager.instance.Register("Esc");
        grid = new GridXYZ<GridObject>(gridLength, gridWidth, gridHeight, cellSize, Vector3.zero, (GridXYZ<GridObject> g, int x, int z, int y) => new GridObject(g, x, z, y));
        //GroundInit();

    }
    private void Start()
    {
        LoadAll();
    }
    float tickTime = 0f;
    int min = 0;
    int hour = 0;
    int day = 0;
    private void FixedUpdate()
    {
        tickTime += 0.02f;
        if (tickTime >= 1f)
        {
            min += 1;
            tickTime = 0f;
        }
        if (min >= 60)
        {
            hour++;
            min = 0;
        }
        if (hour == 24)
        {
            day++;
            hour = 0;
        }
    }
    private void Update()
    {
        debug.text = string.Format("{0:D2}", hour) + ":" + string.Format("{0:D2}", min / 10 * 10);
        if (isShowDeubg)
        {
            grid.DrawLine();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ObserverManager.instance.RespondListener("Esc");
            UIManager.instance.ShowBottomMenu();

            // DeselectObjectType();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = BuildItemSO.GetNextDir(dir);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            placedObjectTypeSO = buildItemSOList[0];
            RefreshSelectedObjectType();
        }
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition("Grid");
            grid.GetGridXZ(mousePosition, out int x, out int z);
            Vector2Int placedObjectOriginGirdPos = grid.GridIntXZ(new Vector2Int(x, z));

            // Test Can Build
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOriginGirdPos, dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPos(placedObjectOriginGirdPos.x, 0, placedObjectOriginGirdPos.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.CellSize;

                BuildItemDone placedObject = BuildItemDone.Create(placedObjectWorldPosition, placedObjectOriginGirdPos, dir, placedObjectTypeSO);

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

                OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                //DeselectObjectType();
            }
            else
            {
                // Cannot build here
                // UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
            }
        }
    }
    void GroundInit()
    {
        int offset = 128;
        int r = 10;
        int ym = 0;
        int xym = (int)(r / 1.414);
        for (int x = 0; x < xym; x++)
        {
            int j = 0;
            while (x * x + j * j - r * r <= 0)
            {
                j++;
            }
            ym = x * x + j * j - r * r - j < 0 ? j : j - 1;
            for (int l = 0; l < ym; l++)
            {
                CreateOnInit(offset + x, offset + l);
                CreateOnInit(offset + l, offset + x);

                CreateOnInit(offset - (x + 1), offset + l);
                CreateOnInit(offset - (l + 1), offset + x);

                CreateOnInit(offset - (x + 1), offset - (l + 1));
                CreateOnInit(offset - (l + 1), offset - (x + 1));

                CreateOnInit(offset + x, offset - (l + 1));
                CreateOnInit(offset + l, offset - (x + 1));

            }
        }
    }
    public void CreateOnInit(int x, int y)
    {
        Vector2Int placedObjectOriginGirdPos = new Vector2Int(x, y);
        List<Vector2Int> gridPositionList = buildItemSOList[5].GetGridPositionList(placedObjectOriginGirdPos, dir);
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                return;
            }
        }
        Vector3 placedObjectWorldPosition = grid.GetWorldPos(placedObjectOriginGirdPos.x, 0, placedObjectOriginGirdPos.y);
        BuildItemDone placedObject = BuildItemDone.Create(placedObjectWorldPosition, new Vector2Int(x, y), dir, buildItemSOList[0]);

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
        }
    }

    public void LoadAll()
    {
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                CreateOnInit(i, j);

            }
        }
    }
    /// <summary>
    /// 获得鼠标世界快照位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMouseWorldSnappedPosition(string layerNmae)
    {
        Vector3 mousePosition = GetMouseWorldPosition(layerNmae);
        grid.GetGridXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null)
        {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPos(x, 0, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.CellSize;
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }
    public Vector3 GetMouseWorldPosition(string layerName)
    {
        int layer = LayerMask.GetMask(layerName);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layer))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectTypeSO != null)
        {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }
    private void DeselectObjectType()
    {
        placedObjectTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class GridObject
{

    private GridXYZ<GridObject> grid;
    private int x;
    private int y;
    private int z;
    public BuildItemDone placedObject;

    public GridObject(GridXYZ<GridObject> grid, int x, int z, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
        placedObject = null;
    }

    public override string ToString()
    {
        return x + "," + y + "," + z + ":" + placedObject;
    }

    public void SetPlacedObject(BuildItemDone placedObject)
    {
        this.placedObject = placedObject;
        //grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPlacedObject()
    {
        placedObject = null;
        //grid.TriggerGridObjectChanged(x, y);
    }

    public BuildItemDone GetPlacedObject()
    {
        return placedObject;
    }

    public bool CanBuild()
    {
        return placedObject == null;
    }

}
