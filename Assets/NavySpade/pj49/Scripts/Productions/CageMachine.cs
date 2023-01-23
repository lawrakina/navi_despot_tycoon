using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions
{
    public class CageMachine : ProductionElement
    {
        public float TimeToSpawnUnit;

        public Transform UnitSpawnPosition;

        public CollectingUnit UnitPrefab;

        private Timer _timer;

        private void Awake()
        {
            _timer = new Timer(TimeToSpawnUnit);
        }

        public override void UpdateOnWork()
        {
            if (_timer.IsFinish())
            {
                Instantiate(UnitPrefab, UnitSpawnPosition.transform.position, Quaternion.identity);
                _timer.Reload();
            }
            
            _timer.Update(Time.deltaTime);
        }
    }
}