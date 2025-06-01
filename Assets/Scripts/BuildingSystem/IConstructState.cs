using UnityEngine;

public interface IConstructState
{
    void EndState();
    void OnState(Vector3Int gridPos);
    void UpdateState(Vector3Int gridPos);
}
