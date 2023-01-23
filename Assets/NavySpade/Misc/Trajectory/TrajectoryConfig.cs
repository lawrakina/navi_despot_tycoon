using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace Misc.Trajectory
{
    public class TrajectoryConfig : ObjectConfig<TrajectoryConfig>
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;

        [SerializeField] private int _objectsCount;
        [SerializeField] private float _startCalculateToleraste = .1f;
        [SerializeField] private float _trajectorySimulationToleraste = .02f;
        [SerializeField] private int _simulationFramesCount = 100;
        [SerializeField] private int _emitBallEachSimulationStep;
        [SerializeField] private AnimationCurve _sizeByDistance;
        
        [Tooltip("Какая позиция по y должна быть чтобы поставить метку")]
        [SerializeField] private float _targetYWorldPos;

        [SerializeField] private float _targetCalculationToleraste = 1;

        [Space]
        [Header("Шарики")]
        [SerializeField] private GameObject _rigballCollisionPrefab;

        public GameObject PlayerPrefab => _playerPrefab;

        public GameObject EnemyPrefab => _enemyPrefab;

        public int ObjectsCount => _objectsCount;

        public float TrajectorySimulationToleraste => _trajectorySimulationToleraste;

        public float TargetYWorldPos => _targetYWorldPos;

        public float TargetCalculationToleraste => _targetCalculationToleraste;

        public int EmitBallEachSimulationStep => _emitBallEachSimulationStep;

        public int SimulationFramesCount => _simulationFramesCount;

        public GameObject RigballCollisionPrefab => _rigballCollisionPrefab;

        public float StartCalculateToleraste => _startCalculateToleraste;

        public AnimationCurve SizeByDistance => _sizeByDistance;
    }
}