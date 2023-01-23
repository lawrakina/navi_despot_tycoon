using NaughtyAttributes;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Navigation
{
    public class AIPoint : ExtendedMonoBehavior<AIPoint>
    {
        public AIPoint NextPoint;
        public NavigationLayerAsset LayerAsset;

        [Foldout("Debug")] public bool IsShowGizmos;

        [Foldout("Debug"), ShowIf(nameof(IsShowGizmos))]
        public float SphereSize = .3f;

        private void OnDrawGizmos()
        {
            if (IsShowGizmos == false)
                return;

            Gizmos.color = LayerAsset.DebugColor;

            if (NextPoint == null)
                return;

            Gizmos.DrawLine(transform.position, NextPoint.transform.position);
            Gizmos.DrawWireSphere(transform.position, SphereSize);
        }

        public static (Vector3 position, AIPoint aiPoint) GetNearestPathPoint(Vector3 pos, NavigationLayerAsset layer)
        {
            var maxRayDistance = Mathf.Infinity;
            var inPathPoint = Vector3.zero;
            AIPoint startAIPath = null;

            //https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
            foreach (var aiPoint in Active)
            {
                if (aiPoint.NextPoint == null || aiPoint.LayerAsset != layer)
                    continue;

                var nVector = aiPoint.NextPoint.transform.position - aiPoint.transform.position;
                nVector.Normalize();

                var ap = aiPoint.transform.position - pos;
                var scale = Vector3.Scale(ap,
                    new Vector3(Mathf.Abs(nVector.x), Mathf.Abs(nVector.y), Mathf.Abs(nVector.z)));

                var dirToPlane = ap - scale;

                var plane = new Plane(Quaternion.Euler(0, 90, 0) * nVector, aiPoint.transform.position);
                var ray = new Ray(pos, dirToPlane);

                plane.Raycast(ray, out var planeDistance);
                var currentInPathPoint = ray.GetPoint(planeDistance);
                var deltaPosToPath = pos - currentInPathPoint;

                if (Vector3.Dot(nVector, currentInPathPoint - aiPoint.transform.position) < 0)
                    continue;
                
                if(Vector3.Distance(aiPoint.transform.position, currentInPathPoint) > Vector3.Distance(aiPoint.NextPoint.transform.position, aiPoint.transform.position))
                    continue;

                if (deltaPosToPath.magnitude < maxRayDistance)
                {
                    maxRayDistance = deltaPosToPath.magnitude;
                    inPathPoint = currentInPathPoint;
                    startAIPath = aiPoint;
                }
            }

            return (inPathPoint, startAIPath);
        }
    }
}