using System.Collections;
using System.Collections.Generic;

// DO NOT EDIT - AUTOMATICALLY GENERATED

namespace JustTrack {
    public enum Dimension {
        AD_INSTANCE_NAME,
        AD_NETWORK,
        AD_PLACEMENT,
        AD_SDK_NAME,
        AD_SEGMENT_NAME,
        AGE,
        CATEGORY_NAME,
        CUSTOM_1,
        CUSTOM_2,
        CUSTOM_3,
        ELEMENT_ID,
        ELEMENT_NAME,
        GENDER,
        ITEM_ID,
        ITEM_NAME,
        ITEM_TYPE,
        METHOD,
        PREVIOUS_APP_VERSION_CODE,
        PROVIDER_NAME,
        SESSION_ID,
        TEST_GROUP,
    }

    internal static class DimensionConversions {
        internal static string DimensionToString(Dimension dimension) {
            switch (dimension) {
            case Dimension.AD_INSTANCE_NAME:
                return "ad_instance_name";
            case Dimension.AD_NETWORK:
                return "ad_network";
            case Dimension.AD_PLACEMENT:
                return "ad_placement";
            case Dimension.AD_SDK_NAME:
                return "ad_sdk_name";
            case Dimension.AD_SEGMENT_NAME:
                return "ad_segment_name";
            case Dimension.AGE:
                return "age";
            case Dimension.CATEGORY_NAME:
                return "category_name";
            case Dimension.CUSTOM_1:
                return "custom_1";
            case Dimension.CUSTOM_2:
                return "custom_2";
            case Dimension.CUSTOM_3:
                return "custom_3";
            case Dimension.ELEMENT_ID:
                return "element_id";
            case Dimension.ELEMENT_NAME:
                return "element_name";
            case Dimension.GENDER:
                return "gender";
            case Dimension.ITEM_ID:
                return "item_id";
            case Dimension.ITEM_NAME:
                return "item_name";
            case Dimension.ITEM_TYPE:
                return "item_type";
            case Dimension.METHOD:
                return "method";
            case Dimension.PREVIOUS_APP_VERSION_CODE:
                return "previous_app_version_code";
            case Dimension.PROVIDER_NAME:
                return "provider_name";
            case Dimension.SESSION_ID:
                return "session_id";
            case Dimension.TEST_GROUP:
                return "test_group";
            default:
                return "";
            }
        }
    }
}
