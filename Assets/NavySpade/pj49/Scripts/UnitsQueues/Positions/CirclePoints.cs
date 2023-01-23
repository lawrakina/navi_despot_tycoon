using System.Collections.Generic;
using UnityEngine;


namespace NavySpade.pj49.Scripts.UnitsQueues.Positions{
    public class CirclePoints : PointsHolder{
        #region Private data

        private int _numberOfSides;
        private float _polygonRadius;
        

        #endregion

        #region Trash

        public override Vector2 RectSize => _rectSize;
        private Vector2 _rectSize = new Vector2(8, 8);
        

        #endregion

        protected override Vector3[] GetPositions(){
            if (_positions == null)
                return GetPositions(_numberOfSides, _polygonRadius);
            return _positions;
        }
        public Vector3[] GetPositions(int pointsCount, float radius ){
            _numberOfSides = pointsCount;
            _polygonRadius = radius;
            _positions = DrawPolygon(transform.position, radius, pointsCount);
            return _positions;
        }

        private Vector3[] DrawPolygon(Vector3 center, float radius, int numSides){
            var result = new List<Vector3>();
            var startCorner = new Vector3(radius, 0, 0) + center;
            var previousCorner = startCorner;
            result.Add(startCorner);

            for (var i = 1; i < numSides; i++){
                var cornerAngle = 2f * Mathf.PI / (float) numSides * i;
                var currentCorner = new Vector3(
                    Mathf.Cos(cornerAngle) * radius,
                    0,
                    Mathf.Sin(cornerAngle) * radius) + center;
                Debug.DrawLine(currentCorner, previousCorner);
                previousCorner = currentCorner;
                result.Add(currentCorner);
            }

            Debug.DrawLine(startCorner, previousCorner);
            return result.ToArray();
        }

        public override void ChangeSize(int totalCount){
            //nothing
        }
    }
}