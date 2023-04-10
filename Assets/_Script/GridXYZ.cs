using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridXYZ<T>
{


    private int length;
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private T[,,] gridArray;
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }
    public GridXYZ(int length, int width, int height, float cellSize, Vector3 originPosition, Func<GridXYZ<T>, int, int, int, T> createGridObject)
    {
        Length = length;
        Width = width;
        Height = height;
        CellSize = cellSize;
        this.originPosition = originPosition;
        this.gridArray = new T[length, width, height];
        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < width; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridArray[x, z, y] = createGridObject(this, x, z, y);
                }
            }
        }

    }
    public void DrawLine()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length + 1; x++)
            {
                Debug.DrawLine(GetWorldPos(x, y, 0), GetWorldPos(x, y, width), Color.white, width * cellSize);
            }
            for (int z = 0; z < width + 1; z++)
            {
                Debug.DrawLine(GetWorldPos(0, y, z), GetWorldPos(length, y, z), Color.white, width * cellSize);
            }
        }
    }
    public Vector3 GetWorldPos(int x, int z, int y)
    {
        return new Vector3(x, z, y) * cellSize + originPosition;
    }
    public void GetGridXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }
    public Vector2Int GridIntXZ(Vector2Int gridPosition)
    {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, length - 1),
            Mathf.Clamp(gridPosition.y, 0, width - 1)
        );
    }
    public T GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < length && z < width)
        {
            return gridArray[x, z, 0];
        }
        else
        {
            return default(T);
        }
    }
    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }
    public int Length { get => length; set => length = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float CellSize { get => cellSize; set => cellSize = value; }
}

