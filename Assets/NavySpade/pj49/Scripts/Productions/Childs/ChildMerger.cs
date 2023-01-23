using System.Collections;
using System.Linq;
using DG.Tweening;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Navigation;
using pj40.Core.Tweens;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class ChildMerger : MonoBehaviour
    {
        public ResourcesInventory Inventory;
        public ResourceAsset ResourceAsset;
        public ResourceAsset FoodAsset;

        public ItemStackDisplay Display;
        public float DelayToTryMerge;

        public float FlySpeed;
        public Transform MergePoint;

        public GameObject MergeEffect;
        public float MergeTime;

        public GameObject UnitPrefab;
        public Transform UnitSpawnPosition;


        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(DelayToTryMerge);

                var item = Inventory.Items.FirstOrDefault(i => i.Resource == ResourceAsset);

                if (item != null && item.Amount > 0 && Child.Active.Count > 0)
                    yield return MergeAnimation();
            }
        }

        private IEnumerator MergeAnimation()
        {
            var closedUnit = Child.Active.FindClosed(transform.position);
            
            var closedFood = Display.SpawnedDatas.First();
            var closedFoodVisual = closedFood.Instance.transform.GetChild(0);

            closedUnit.GetComponent<AINavPointMovement>().enabled = false;
            closedUnit.GetComponent<Rigidbody>().isKinematic = true;
            closedFoodVisual.GetComponent<CreateParabolaAnimation>().enabled = false;

            closedUnit.transform.DOMove(MergePoint.position, FlySpeed);
            
            closedFoodVisual.gameObject.SetActive(true);
            yield return closedFoodVisual.transform.DOMove(MergePoint.position, FlySpeed).WaitForCompletion();
            
            MergeEffect.SetActive(true);
            closedUnit.gameObject.SetActive(false);
            closedFoodVisual.gameObject.SetActive(false);
            
            yield return new WaitForSeconds(MergeTime);
            
            MergeEffect.SetActive(false);

            Inventory.TryRemoveResource(closedFood.Asset.CreateItem());
            Destroy(closedUnit.gameObject);

            Instantiate(UnitPrefab, UnitSpawnPosition.position, Quaternion.identity);
        }
    }
}