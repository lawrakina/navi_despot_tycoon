using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace JustTrack {
    internal class UserEventDimensions {
        private Dictionary<Dimension, string> Dimensions;

        internal UserEventDimensions() {
            this.Dimensions = new Dictionary<Dimension, string>();
        }

        internal void Set(Dimension pDimension, string pValue) {
            this.Dimensions[pDimension] = pValue;
        }

        internal string Get(Dimension pDimension) {
            if (this.Dimensions.ContainsKey(pDimension)) {
                return this.Dimensions[pDimension];
            }

            return null;
        }

        #if UNITY_IOS
            internal string Encode() {
                StringBuilder sb = new StringBuilder();
                sb.Append('{');
                bool first = true;
                foreach (var item in Dimensions) {
                    if (first) {
                        first = false;
                    } else {
                        sb.Append(',');
                    }
                    sb.Append($"\"{DimensionConversions.DimensionToString(item.Key)}\":\"");
                    foreach (var c in item.Value) {
                        if (c < ' ' || c == '"' || c == '\\' || c > '~') {
                            sb.Append("\\u").Append(((int) c).ToString("X4"));
                        } else {
                            sb.Append(c);
                        }
                    }
                    sb.Append('"');
                }
                sb.Append('}');

                return sb.ToString();
            }
        #endif
    }
}
