using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.Modules.Pooling.Runtime;

namespace NavySpade.pj46.UI.PoppingText
{
    public class PopapingTextManager : ExtendedMonoBehavior<PopapingTextManager>
    {
        public IndicatorUI Prefab;
        public int PoolCount = 15;

        public ObjectPoolOld<IndicatorUI> Pool => _pool;
        
        private ObjectPoolOld<IndicatorUI> _pool;

        protected override void Awake()
        {
            base.Awake();
            
            _pool = new ObjectPoolOld<IndicatorUI>();

            _pool.Initialize(transform, PoolCount, Prefab, c => c.Pool = _pool);
        }
    }
}