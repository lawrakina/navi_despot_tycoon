using UnityEngine;

namespace Misc.Trajectory
{
    [HelpURL("https://docs.google.com/document/d/1pOku7G-X-U1qHPqZLVki4D_UomUsONdF8uoXV8D33Ts/edit#heading=h.tfunulk62mmv")]
    public class TrajectoryDisplay : TrajectoryBase
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _target;

        [SerializeField] private Vector3 _velocity;
        [SerializeField] private Vector3 _angularVelocity;

        public bool UsePhysicMode;

        private TrajectoryConfig _config;
        private GameObject[] _instances;
        private GameObject _rigballTargetInstance;

        private TrajectoryCalculationBase _physicCalculation;
        private TrajectoryCalculationBase _math;

        private void Awake()
        {
            _config = TrajectoryConfig.Instance;

            _instances = new GameObject[_config.ObjectsCount];

            for (int i = 0; i < _instances.Length; i++)
            {
                var newObject = Instantiate(_bulletPrefab, transform);

                newObject.SetActive(false);

                _instances[i] = newObject;
            }

            _rigballTargetInstance = Instantiate(_config.RigballCollisionPrefab, transform);
            _rigballTargetInstance.SetActive(false);
            _target.SetActive(false);

            _physicCalculation = new PhysicCalculation();
            _math = new MathCalculation();
        }

        private void OnDisable()
        {
            foreach (var instance in _instances)
            {
                Destroy(instance);
            }
        }

        public override void EnableTrajectory()
        {
            _target.SetActive(true);
        }

        public override void DisableTrajectory()
        {
            foreach (var instance in _instances)
            {
                instance.SetActive(false);
            }

            _rigballTargetInstance.SetActive(false);
            _target.SetActive(false);
        }

        public override void UpdateTrajectory(Vector3 targetPos)
        {
            ShowTrajectory(_bulletPrefab, transform.position, _velocity, _angularVelocity);
        }

        private void UpdateTrajectoryBalls(Vector3 origin, Vector3 direction)
        {
            for (int i = 0; i < _config.ObjectsCount; i++)
            {
                float time = i * _config.TrajectorySimulationToleraste;

                _instances[i].transform.position = origin + direction * time + Physics.gravity * (time * time) / 2f;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_origin, _origin + _velocity);
        }

        private Vector3 _origin;

        public void ShowTrajectory(GameObject prefab, Vector3 origin, Vector3 speed, Vector3 angularVelocity)
        {
            TrajectoryCalculationBase calculator = UsePhysicMode ? _physicCalculation : _math;
            var result = calculator.ShowTrajectory(prefab, origin, speed, angularVelocity);

            int ballIndex = 0;
            for (int i = 0; i < result.Points.Length; i++)
            {
                if (result.SecondCollisionPointIndex != null && i > result.SecondCollisionPointIndex.Value)
                    return;

                if (i % _config.EmitBallEachSimulationStep == 0 && ballIndex < _instances.Length)
                {
                    _instances[ballIndex].SetActive(true);
                    _instances[ballIndex].transform.position = result.Points[i];
                    _instances[ballIndex].transform.localScale = Vector3.one * _config.SizeByDistance.Evaluate((float)i / _instances.Length);
                    
                    ballIndex++;
                }
            }

            while (ballIndex < _instances.Length)
            {
                //выключаем шары которые не были использованы
                _instances[ballIndex].SetActive(false);
                ballIndex++;
            }

            if (result.FirstCollisionPoint != null)
            {
                _rigballTargetInstance.SetActive(true);
                _rigballTargetInstance.transform.position = result.FirstCollisionPoint.Value;
            }
            else
            {
                _rigballTargetInstance.SetActive(false);
            }
        }
    }
}