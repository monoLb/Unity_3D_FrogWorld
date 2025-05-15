using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnState(Vector3Int gridPos);
    void UpdateState(Vector3Int gridPos);
}
