// using System.Collections.Generic;
// using System.Linq;
// using DG.Tweening;
// using Unity.Mathematics;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace NavySpade.Project_51.Scripts.Player
// {
//     public class ResourceStack : MonoBehaviour
//     {
//         private bool _isRun;
//         private float _startZ;
//         private Vector3 _previousPosition;
//         private Vector3 _velocity;
//
//         private readonly Dictionary<ResourceType, List<CellInfo>> _stack =
//             new Dictionary<ResourceType, List<CellInfo>>();
//
//         public void Init(ResourceType[] resourceTypes)
//         {
//             var startX = -(PlayerConfig.Instance.StackItemSize.x / 2) * (resourceTypes.Length - 1);
//             var x = startX;
//             var yStart = transform.localPosition.y;
//             var zStart = transform.localPosition.z;
//             var y = yStart;
//             var z = zStart;
//
//             foreach (var resourceType in resourceTypes)
//             {
//                 var pile = new List<CellInfo>();
//                 _stack.Add(resourceType, pile);
//                 for (var i = 0; i < PlayerConfig.Instance.StackHeightCount; i++)
//                 {
//                     var resourceCell = new GameObject("ResourceHolder");
//                     resourceCell.transform.SetParent(transform);
//                     resourceCell.transform.localPosition = new Vector3(x, y, z);
//                     resourceCell.transform.localScale = PlayerConfig.Instance.StackItemScale;
//                     var cell = new CellInfo(resourceCell.transform, false);
//                     pile.Add(cell);
//
//                     y += PlayerConfig.Instance.StackItemSize.y;
//                 }
//
//                 y = yStart;
//                 x += PlayerConfig.Instance.StackItemSize.x;
//             }
//         }
//
//         public bool TryAddResource(MinedItem item, out Transform cellTransform)
//         {
//             cellTransform = null;
//             var pile = _stack[item.Type];
//             var firstEmpty = pile.FirstOrDefault(t => !t.Full);
//             if (firstEmpty == null)
//             {
//                 return false;
//             }
//
//             item.transform.localRotation = quaternion.identity;
//             item.transform.DOScale(1f, PlayerConfig.Instance.StackItemAppearDuration).ChangeStartValue(Vector3.zero)
//                 .SetEase(PlayerConfig.Instance.StackItemAppearEase);
//             cellTransform = firstEmpty.Cell;
//             firstEmpty.Item = item;
//             firstEmpty.Full = true;
//
//             return true;
//         }
//
//         public MinedItem[] ExtractItems(ResourceType type, int count, int countLeft)
//         {
//             var result = new MinedItem[count];
//             var startIndex = countLeft;
//             var endIndex = startIndex + count;
//             var pile = _stack[type];
//             var index = 0;
//             for (var i = endIndex - 1; i >= startIndex; i--)
//             {
//                 MinedItem item;
//                 if (i >= pile.Count || !pile[i].Full)
//                 {
//                     item = Instantiate(type.ModelPrefab, transform);
//                 }
//                 else
//                 {
//                     item = pile[i].Item;
//                     pile[i].Item = null;
//                     pile[i].Full = false;
//                 }
//
//                 item.Init(ResourcesConfig.Instance.BuildEffectSettings, type, 1);
//                 result[index] = item;
//                 index++;
//             }
//
//             return result;
//         }
//
//         private void Update()
//         {
//             _velocity = (transform.position - _previousPosition) / Time.deltaTime;
//             if (_previousPosition == Vector3.zero)
//             {
//                 _velocity = Vector3.zero;
//             }
//
//             _previousPosition = transform.position;
//
//             if (!_isRun)
//             {
//                 return;
//             }
//
//             for (var index = 0; index < PlayerConfig.Instance.StackHeightCount; index++)
//             {
//                 var indexLerp = Mathf.InverseLerp(0, PlayerConfig.Instance.StackHeightCount - 1, index);
//                 var indexFlexibility = PlayerConfig.Instance.StackFlexibility.Evaluate(indexLerp);
//                 var zOffset = -_velocity.magnitude * indexFlexibility;
//                 foreach (var stack in _stack)
//                 {
//                     var cellInfo = stack.Value[index];
//                     var offsetPosition = new Vector3(
//                         cellInfo.Cell.transform.localPosition.x,
//                         cellInfo.Cell.transform.localPosition.y,
//                         zOffset
//                     );
//                     cellInfo.Cell.transform.localPosition = Vector3.Lerp(cellInfo.Cell.transform.localPosition,
//                         offsetPosition, PlayerConfig.Instance.StackItemSpeed * Time.deltaTime);
//                 }
//             }
//         }
//
//         public void ChangeMode(bool isRun)
//         {
//             _isRun = isRun;
//
//             foreach (var stack in _stack)
//             {
//                 foreach (var cell in stack.Value)
//                 {
//                     if (!_isRun)
//                     {
//                         cell.Cell.transform.DOLocalMoveZ(0, PlayerConfig.Instance.StackShakeDuration)
//                             .SetEase(PlayerConfig.Instance.StackShakeEase);
//                     }
//                     else
//                     {
//                         cell.Cell.transform.DOKill();
//                     }
//                 }
//             }
//         }
//
//         public void Destruct(Vector3 direction)
//         {
//             foreach (var stack in _stack)
//             {
//                 foreach (var cell in stack.Value)
//                 {
//                     if (!cell.Full)
//                     {
//                         continue;
//                     }
//
//                     var itemTransform = cell.Item.transform;
//                     cell.Item = null;
//                     cell.Full = false;
//                     itemTransform.parent = null;
//                     var position = transform.position + Random.insideUnitSphere + direction * _velocity.magnitude;
//                     itemTransform.DOScale(Vector3.zero, PlayerConfig.Instance.StackDestroyDuration).SetEase(Ease.InQuad);
//                     itemTransform
//                         .DOJump(position, PlayerConfig.Instance.StackDestroyJumpPower, 1,
//                             PlayerConfig.Instance.StackDestroyDuration).SetEase(Ease.Linear).OnComplete(() =>
//                         {
//                             Destroy(itemTransform.gameObject);
//                         });
//                 }
//             }
//         }
//
//         public void Clear()
//         {
//             foreach (var stack in _stack)
//             {
//                 foreach (var cell in stack.Value)
//                 {
//                     if (!cell.Full)
//                     {
//                         continue;
//                     }
//
//                     var item = cell.Item;
//                     cell.Item = null;
//                     cell.Full = false;
//                     Destroy(item.gameObject);
//                 }
//             }
//         }
//
//         private class CellInfo
//         {
//             public readonly Transform Cell;
//             public MinedItem Item;
//             public bool Full;
//
//             public CellInfo(Transform cell, bool full)
//             {
//                 Cell = cell;
//                 Full = full;
//             }
//         }
//     }
// }