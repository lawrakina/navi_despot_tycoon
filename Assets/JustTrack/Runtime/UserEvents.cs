using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DO NOT EDIT - AUTOMATICALLY GENERATED
namespace JustTrack {
    /**
     * A screen was shown to the user. Use element_name or element_id to specify which screen.
     */
    public class UserScreenShowEvent: StandardUserEvent {
        public UserScreenShowEvent(string pElementName, string pElementId) : base(new EventDetails("user", "screen", "show")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A notification was shown to the user. Use element_name or element_id to specify which notification.
     */
    public class UserNotificationShowEvent: StandardUserEvent {
        public UserNotificationShowEvent(string pElementName, string pElementId) : base(new EventDetails("user", "notification", "show")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A dialog was shown to the user. Use element_name or element_id to specify which dialog.
     */
    public class UserDialogShowEvent: StandardUserEvent {
        public UserDialogShowEvent(string pElementName, string pElementId) : base(new EventDetails("user", "dialog", "show")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A button was shown to the user. Use element_name or element_id to specify which button.
     */
    public class UserButtonShowEvent: StandardUserEvent {
        public UserButtonShowEvent(string pElementName, string pElementId) : base(new EventDetails("user", "button", "show")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A card was shown to the user. Use element_name or element_id to specify which card.
     */
    public class UserCardShowEvent: StandardUserEvent {
        public UserCardShowEvent(string pElementName, string pElementId) : base(new EventDetails("user", "card", "show")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A button was clicked by the user. Use element_name or element_id to specify which button.
     */
    public class UserButtonClickEvent: StandardUserEvent {
        public UserButtonClickEvent(string pElementName, string pElementId) : base(new EventDetails("user", "button", "click")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A notification was clicked by the user. Use element_name or element_id to specify which notification.
     */
    public class UserNotificationClickEvent: StandardUserEvent {
        public UserNotificationClickEvent(string pElementName, string pElementId) : base(new EventDetails("user", "notification", "click")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * A card was clicked by the user. Use element_name or element_id to specify which card.
     */
    public class UserCardClickEvent: StandardUserEvent {
        public UserCardClickEvent(string pElementName, string pElementId) : base(new EventDetails("user", "card", "click")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * You can provide an exact numeric rating as value to see average on the dashboard and/or group the ratings using element_name/element_id
     * Note: If you want to see the count per Rating (ex. 1-5 stars) you need to provide the rating as element_name/element_id or custom dimension.
     */
    public class UserRatingProvideEvent: StandardUserEvent {
        public UserRatingProvideEvent(string pElementName, string pElementId) : base(new EventDetails("user", "rating", "provide")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }

        public UserRatingProvideEvent(string pElementName, string pElementId, double pCount) : base(new EventDetails("user", "rating", "provide")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * You can provide an exact numeric rating as value to see average on the dashboard and/or group the ratings using element_name/element_id
     * Note: If you want to see the count per Rating (ex. 1-5 stars) you need to provide the rating as element_name/element_id or custom dimension.
     */
    public class UserRatingPositiveEvent: StandardUserEvent {
        public UserRatingPositiveEvent(string pElementName, string pElementId) : base(new EventDetails("user", "rating", "positive")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }

        public UserRatingPositiveEvent(string pElementName, string pElementId, double pCount) : base(new EventDetails("user", "rating", "positive")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * You can provide an exact numeric rating as value to see average on the dashboard and/or group the ratings using element_name/element_id
     * Note: If you want to see the count per Rating (ex. 1-5 stars) you need to provide the rating as element_name/element_id or custom dimension.
     */
    public class UserRatingNeutralEvent: StandardUserEvent {
        public UserRatingNeutralEvent(string pElementName, string pElementId) : base(new EventDetails("user", "rating", "neutral")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }

        public UserRatingNeutralEvent(string pElementName, string pElementId, double pCount) : base(new EventDetails("user", "rating", "neutral")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * You can provide an exact numeric rating as value to see average on the dashboard and/or group the ratings using element_name/element_id
     * Note: If you want to see the count per Rating (ex. 1-5 stars) you need to provide the rating as element_name/element_id or custom dimension.
     */
    public class UserRatingNegativeEvent: StandardUserEvent {
        public UserRatingNegativeEvent(string pElementName, string pElementId) : base(new EventDetails("user", "rating", "negative")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }

        public UserRatingNegativeEvent(string pElementName, string pElementId, double pCount) : base(new EventDetails("user", "rating", "negative")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * The user started the registration process.
     */
    public class RegistrationProcessStartEvent: StandardUserEvent {
        public RegistrationProcessStartEvent() : base(new EventDetails("registration", "process", "start")) {
        }
    }

    /**
     * The user finished the registration process. You can provide the duration since the start of the process to see an average on the dashboard.
     */
    public class RegistrationProcessFinishEvent: StandardUserEvent {
        public RegistrationProcessFinishEvent() : base(new EventDetails("registration", "process", "finish")) {
        }

        public RegistrationProcessFinishEvent(double pDuration, TimeUnitGroup unit) : base(new EventDetails("registration", "process", "finish")) {
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * The user failed the registration process. You can provide the duration since the start of the process to see an average on the dashboard.
     */
    public class RegistrationProcessFailEvent: StandardUserEvent {
        public RegistrationProcessFailEvent() : base(new EventDetails("registration", "process", "fail")) {
        }

        public RegistrationProcessFailEvent(double pDuration, TimeUnitGroup unit) : base(new EventDetails("registration", "process", "fail")) {
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track the terms of services being shown to the user.
     */
    public class RegistrationTosShowEvent: StandardUserEvent {
        public RegistrationTosShowEvent() : base(new EventDetails("registration", "tos", "show")) {
        }
    }

    /**
     * Track the terms of services being accepted to the user.
     */
    public class RegistrationTosAcceptEvent: StandardUserEvent {
        public RegistrationTosAcceptEvent() : base(new EventDetails("registration", "tos", "accept")) {
        }
    }

    /**
     * Track the terms of services being declined to the user.
     */
    public class RegistrationTosDeclineEvent: StandardUserEvent {
        public RegistrationTosDeclineEvent() : base(new EventDetails("registration", "tos", "decline")) {
        }
    }

    /**
     * Track requesting users to provide their age.
     */
    public class RegistrationAgeRequestEvent: StandardUserEvent {
        public RegistrationAgeRequestEvent() : base(new EventDetails("registration", "age", "request")) {
        }
    }

    /**
     * Track users providing their age.
     */
    public class RegistrationAgeProvideEvent: StandardUserEvent {
        public RegistrationAgeProvideEvent(string pAge) : base(new EventDetails("registration", "age", "provide")) {
            if (pAge != null && pAge != "") {
                base.SetDimension(Dimension.AGE, pAge);
            }
        }
    }

    /**
     * Track users declining providing their age.
     */
    public class RegistrationAgeDeclineEvent: StandardUserEvent {
        public RegistrationAgeDeclineEvent() : base(new EventDetails("registration", "age", "decline")) {
        }
    }

    /**
     * Track requesting users to provide their gender.
     */
    public class RegistrationGenderRequestEvent: StandardUserEvent {
        public RegistrationGenderRequestEvent() : base(new EventDetails("registration", "gender", "request")) {
        }
    }

    /**
     * Track users providing their gender.
     */
    public class RegistrationGenderProvideEvent: StandardUserEvent {
        public RegistrationGenderProvideEvent(string pGender) : base(new EventDetails("registration", "gender", "provide")) {
            if (pGender != null && pGender != "") {
                base.SetDimension(Dimension.GENDER, pGender);
            }
        }
    }

    /**
     * Track users declining providing their gender.
     */
    public class RegistrationGenderDeclineEvent: StandardUserEvent {
        public RegistrationGenderDeclineEvent() : base(new EventDetails("registration", "gender", "decline")) {
        }
    }

    /**
     * Login method selected
     */
    public class LoginLoginmethodSelectEvent: StandardUserEvent {
        public LoginLoginmethodSelectEvent(string pProviderName) : base(new EventDetails("login", "loginmethod", "select")) {
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
        }
    }

    /**
     * Track 3rd party authorization progress
     */
    public class LoginProviderauthorizationStartEvent: StandardUserEvent {
        public LoginProviderauthorizationStartEvent(string pProviderName) : base(new EventDetails("login", "providerauthorization", "start")) {
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
        }
    }

    /**
     * Track 3rd party authorization progress
     */
    public class LoginProviderauthorizationFinishEvent: StandardUserEvent {
        public LoginProviderauthorizationFinishEvent(string pProviderName) : base(new EventDetails("login", "providerauthorization", "finish")) {
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
        }

        public LoginProviderauthorizationFinishEvent(string pProviderName, double pDuration, TimeUnitGroup unit) : base(new EventDetails("login", "providerauthorization", "finish")) {
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track 3rd party authorization progress
     */
    public class LoginProviderauthorizationFailEvent: StandardUserEvent {
        public LoginProviderauthorizationFailEvent(string pProviderName) : base(new EventDetails("login", "providerauthorization", "fail")) {
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
        }

        public LoginProviderauthorizationFailEvent(string pProviderName, double pDuration, TimeUnitGroup unit) : base(new EventDetails("login", "providerauthorization", "fail")) {
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track login progress
     */
    public class LoginProcessStartEvent: StandardUserEvent {
        public LoginProcessStartEvent() : base(new EventDetails("login", "process", "start")) {
        }
    }

    /**
     * Track login progress
     */
    public class LoginProcessFinishEvent: StandardUserEvent {
        public LoginProcessFinishEvent() : base(new EventDetails("login", "process", "finish")) {
        }

        public LoginProcessFinishEvent(double pDuration, TimeUnitGroup unit) : base(new EventDetails("login", "process", "finish")) {
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track login progress
     */
    public class LoginProcessFailEvent: StandardUserEvent {
        public LoginProcessFailEvent() : base(new EventDetails("login", "process", "fail")) {
        }

        public LoginProcessFailEvent(double pDuration, TimeUnitGroup unit) : base(new EventDetails("login", "process", "fail")) {
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Provide information when user has granted the usage permission or if it is missing
     */
    public class UsagePermissionRequiredEvent: StandardUserEvent {
        public UsagePermissionRequiredEvent() : base(new EventDetails("usage", "permission", "required")) {
        }
    }

    /**
     * Provide information when user has granted the usage permission or if it is missing
     */
    public class UsagePermissionGrantEvent: StandardUserEvent {
        public UsagePermissionGrantEvent() : base(new EventDetails("usage", "permission", "grant")) {
        }
    }

    /**
     * Used for dialog before sending the user to the system settings
     */
    public class UsageRequestShowEvent: StandardUserEvent {
        public UsageRequestShowEvent() : base(new EventDetails("usage", "request", "show")) {
        }
    }

    /**
     * Used for dialog before sending the user to the system settings
     */
    public class UsageRequestAcceptEvent: StandardUserEvent {
        public UsageRequestAcceptEvent() : base(new EventDetails("usage", "request", "accept")) {
        }
    }

    /**
     * Used for dialog before sending the user to the system settings
     */
    public class UsageRequestDeclineEvent: StandardUserEvent {
        public UsageRequestDeclineEvent() : base(new EventDetails("usage", "request", "decline")) {
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessStartEvent: StandardUserEvent {
        public VerificationProcessStartEvent(string pMethod) : base(new EventDetails("verification", "process", "start")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessCancelEvent: StandardUserEvent {
        public VerificationProcessCancelEvent(string pMethod) : base(new EventDetails("verification", "process", "cancel")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessFinishEvent: StandardUserEvent {
        public VerificationProcessFinishEvent(string pMethod) : base(new EventDetails("verification", "process", "finish")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessFailEvent: StandardUserEvent {
        public VerificationProcessFailEvent(string pMethod) : base(new EventDetails("verification", "process", "fail")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessNumberrequestedEvent: StandardUserEvent {
        public VerificationProcessNumberrequestedEvent(string pMethod) : base(new EventDetails("verification", "process", "numberrequested")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessNumbervalidEvent: StandardUserEvent {
        public VerificationProcessNumbervalidEvent(string pMethod) : base(new EventDetails("verification", "process", "numbervalid")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessCoderequestedEvent: StandardUserEvent {
        public VerificationProcessCoderequestedEvent(string pMethod) : base(new EventDetails("verification", "process", "coderequested")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * Track verification progress
     * Verification is typically needed before payout
     */
    public class VerificationProcessCodevalidEvent: StandardUserEvent {
        public VerificationProcessCodevalidEvent(string pMethod) : base(new EventDetails("verification", "process", "codevalid")) {
            if (pMethod != null && pMethod != "") {
                base.SetDimension(Dimension.METHOD, pMethod);
            }
        }
    }

    /**
     * An interstitial ad was successfully served. Events will be triggered automatically when using Ironsource mediation.
     */
    internal class AdInterstitialSuccessEvent: StandardUserEvent {
        internal AdInterstitialSuccessEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, string pTestGroup, string pAdSegmentName, string pAdInstanceName, double pRevenue) : base(new EventDetails("ad", "interstitial", "success")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            if (pTestGroup != null && pTestGroup != "") {
                base.SetDimension(Dimension.TEST_GROUP, pTestGroup);
            }
            if (pAdSegmentName != null && pAdSegmentName != "") {
                base.SetDimension(Dimension.AD_SEGMENT_NAME, pAdSegmentName);
            }
            if (pAdInstanceName != null && pAdInstanceName != "") {
                base.SetDimension(Dimension.AD_INSTANCE_NAME, pAdInstanceName);
            }
            base.SetValueAndUnit(pRevenue, JustTrack.Unit.Count);
        }
    }

    /**
     * A rewarded video ad was successfully served. Events will be triggered automatically when using Ironsource mediation.
     */
    internal class AdRewardedSuccessEvent: StandardUserEvent {
        internal AdRewardedSuccessEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, string pTestGroup, string pAdSegmentName, string pAdInstanceName, double pRevenue) : base(new EventDetails("ad", "rewarded", "success")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            if (pTestGroup != null && pTestGroup != "") {
                base.SetDimension(Dimension.TEST_GROUP, pTestGroup);
            }
            if (pAdSegmentName != null && pAdSegmentName != "") {
                base.SetDimension(Dimension.AD_SEGMENT_NAME, pAdSegmentName);
            }
            if (pAdInstanceName != null && pAdInstanceName != "") {
                base.SetDimension(Dimension.AD_INSTANCE_NAME, pAdInstanceName);
            }
            base.SetValueAndUnit(pRevenue, JustTrack.Unit.Count);
        }
    }

    /**
     * A banner ad was successfully served. Events will be triggered automatically when using Ironsource mediation.
     */
    internal class AdBannerSuccessEvent: StandardUserEvent {
        internal AdBannerSuccessEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, string pTestGroup, string pAdSegmentName, string pAdInstanceName, double pRevenue) : base(new EventDetails("ad", "banner", "success")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            if (pTestGroup != null && pTestGroup != "") {
                base.SetDimension(Dimension.TEST_GROUP, pTestGroup);
            }
            if (pAdSegmentName != null && pAdSegmentName != "") {
                base.SetDimension(Dimension.AD_SEGMENT_NAME, pAdSegmentName);
            }
            if (pAdInstanceName != null && pAdInstanceName != "") {
                base.SetDimension(Dimension.AD_INSTANCE_NAME, pAdInstanceName);
            }
            base.SetValueAndUnit(pRevenue, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdOfferwallLoadEvent: StandardUserEvent {
        public AdOfferwallLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "offerwall", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdOfferwallLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "offerwall", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdOfferwallOpenEvent: StandardUserEvent {
        public AdOfferwallOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "offerwall", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdOfferwallOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "offerwall", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdOfferwallCloseEvent: StandardUserEvent {
        public AdOfferwallCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "offerwall", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdOfferwallCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "offerwall", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdOfferwallClickEvent: StandardUserEvent {
        public AdOfferwallClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "offerwall", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdOfferwallClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "offerwall", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdOfferwallShowEvent: StandardUserEvent {
        public AdOfferwallShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "offerwall", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdOfferwallShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "offerwall", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdInterstitialLoadEvent: StandardUserEvent {
        public AdInterstitialLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "interstitial", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdInterstitialOpenEvent: StandardUserEvent {
        public AdInterstitialOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "interstitial", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdInterstitialCloseEvent: StandardUserEvent {
        public AdInterstitialCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "interstitial", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdInterstitialClickEvent: StandardUserEvent {
        public AdInterstitialClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "interstitial", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdInterstitialShowEvent: StandardUserEvent {
        public AdInterstitialShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "interstitial", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdRewardedLoadEvent: StandardUserEvent {
        public AdRewardedLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "rewarded", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdRewardedOpenEvent: StandardUserEvent {
        public AdRewardedOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "rewarded", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdRewardedCloseEvent: StandardUserEvent {
        public AdRewardedCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "rewarded", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdRewardedClickEvent: StandardUserEvent {
        public AdRewardedClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "rewarded", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdRewardedShowEvent: StandardUserEvent {
        public AdRewardedShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "rewarded", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdBannerLoadEvent: StandardUserEvent {
        public AdBannerLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "banner", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdBannerLoadEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "banner", "load")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdBannerOpenEvent: StandardUserEvent {
        public AdBannerOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "banner", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdBannerOpenEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "banner", "open")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track ads
     */
    public class AdBannerCloseEvent: StandardUserEvent {
        public AdBannerCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "banner", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdBannerCloseEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "banner", "close")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdBannerClickEvent: StandardUserEvent {
        public AdBannerClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "banner", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdBannerClickEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "banner", "click")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track ads
     */
    public class AdBannerShowEvent: StandardUserEvent {
        public AdBannerShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "banner", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdBannerShowEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pCount) : base(new EventDetails("ad", "banner", "show")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * Track interstitial video starts.
     */
    public class AdInterstitialStartEvent: StandardUserEvent {
        public AdInterstitialStartEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "start")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialStartEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "interstitial", "start")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track interstitial video stops.
     */
    public class AdInterstitialStopEvent: StandardUserEvent {
        public AdInterstitialStopEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "interstitial", "stop")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdInterstitialStopEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "interstitial", "stop")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track rewarded video starts.
     */
    public class AdRewardedStartEvent: StandardUserEvent {
        public AdRewardedStartEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "start")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedStartEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "rewarded", "start")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Track rewarded video stops.
     */
    public class AdRewardedStopEvent: StandardUserEvent {
        public AdRewardedStopEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement) : base(new EventDetails("ad", "rewarded", "stop")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
        }

        public AdRewardedStopEvent(string pAdSdkName, string pAdNetwork, string pAdPlacement, double pDuration, TimeUnitGroup unit) : base(new EventDetails("ad", "rewarded", "stop")) {
            if (pAdSdkName != null && pAdSdkName != "") {
                base.SetDimension(Dimension.AD_SDK_NAME, pAdSdkName);
            }
            if (pAdNetwork != null && pAdNetwork != "") {
                base.SetDimension(Dimension.AD_NETWORK, pAdNetwork);
            }
            if (pAdPlacement != null && pAdPlacement != "") {
                base.SetDimension(Dimension.AD_PLACEMENT, pAdPlacement);
            }
            base.SetValueAndUnit(pDuration, TimeUnitGroupConversions.ToUnit(unit));
        }
    }

    /**
     * Used to track the player`s progress. Use element_name or element_id to specify the level.
     */
    public class ProgressionLevelStartEvent: StandardUserEvent {
        public ProgressionLevelStartEvent(string pElementName, string pElementId) : base(new EventDetails("progression", "level", "start")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * Used to track the player`s progress. Use element_name or element_id to specify the quest.
     */
    public class ProgressionQuestStartEvent: StandardUserEvent {
        public ProgressionQuestStartEvent(string pElementName, string pElementId) : base(new EventDetails("progression", "quest", "start")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * Used to track the player`s progress. Use element_name or element_id to specify the level.
     */
    public class ProgressionLevelFinishEvent: StandardUserEvent {
        public ProgressionLevelFinishEvent(string pElementName, string pElementId) : base(new EventDetails("progression", "level", "finish")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * Used to track the player`s progress. Use element_name or element_id to specify the level.
     */
    public class ProgressionLevelFailEvent: StandardUserEvent {
        public ProgressionLevelFailEvent(string pElementName, string pElementId) : base(new EventDetails("progression", "level", "fail")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * Used to track the player`s progress. Use element_name or element_id to specify the quest.
     */
    public class ProgressionQuestFinishEvent: StandardUserEvent {
        public ProgressionQuestFinishEvent(string pElementName, string pElementId) : base(new EventDetails("progression", "quest", "finish")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * Used to track the player`s progress. Use element_name or element_id to specify the quest.
     */
    public class ProgressionQuestFailEvent: StandardUserEvent {
        public ProgressionQuestFailEvent(string pElementName, string pElementId) : base(new EventDetails("progression", "quest", "fail")) {
            if (pElementName != null && pElementName != "") {
                base.SetDimension(Dimension.ELEMENT_NAME, pElementName);
            }
            if (pElementId != null && pElementId != "") {
                base.SetDimension(Dimension.ELEMENT_ID, pElementId);
            }
        }
    }

    /**
     * {count} can reflect the number of items or their value (ex. value in in-app currency)
     */
    public class ProgressionResourceSinkEvent: StandardUserEvent {
        public ProgressionResourceSinkEvent(string pItemType, string pItemName, string pItemId) : base(new EventDetails("progression", "resource", "sink")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
        }

        public ProgressionResourceSinkEvent(string pItemType, string pItemName, string pItemId, double pCount) : base(new EventDetails("progression", "resource", "sink")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * {count} can reflect the number of items or their value (ex. value in in-app currency)
     */
    public class ProgressionResourceSourceEvent: StandardUserEvent {
        public ProgressionResourceSourceEvent(string pItemType, string pItemName, string pItemId) : base(new EventDetails("progression", "resource", "source")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
        }

        public ProgressionResourceSourceEvent(string pItemType, string pItemName, string pItemId, double pCount) : base(new EventDetails("progression", "resource", "source")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * {count} can reflect the number of items or their value (ex. value in in-app currency)
     */
    public class PurchaseOptionClickEvent: StandardUserEvent {
        public PurchaseOptionClickEvent(string pItemType, string pItemName, string pItemId) : base(new EventDetails("purchase", "option", "click")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
        }

        public PurchaseOptionClickEvent(string pItemType, string pItemName, string pItemId, double pCount) : base(new EventDetails("purchase", "option", "click")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * {count} can reflect the number of items or their value (ex. value in in-app currency)
     */
    public class PurchaseOptionConfirmEvent: StandardUserEvent {
        public PurchaseOptionConfirmEvent(string pItemType, string pItemName, string pItemId) : base(new EventDetails("purchase", "option", "confirm")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
        }

        public PurchaseOptionConfirmEvent(string pItemType, string pItemName, string pItemId, double pCount) : base(new EventDetails("purchase", "option", "confirm")) {
            if (pItemType != null && pItemType != "") {
                base.SetDimension(Dimension.ITEM_TYPE, pItemType);
            }
            if (pItemName != null && pItemName != "") {
                base.SetDimension(Dimension.ITEM_NAME, pItemName);
            }
            if (pItemId != null && pItemId != "") {
                base.SetDimension(Dimension.ITEM_ID, pItemId);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * {count} can reflect the number of items or their value (ex. value in in-app currency)
     */
    public class PayoutOptionClickEvent: StandardUserEvent {
        public PayoutOptionClickEvent(string pCategoryName, string pProviderName) : base(new EventDetails("payout", "option", "click")) {
            if (pCategoryName != null && pCategoryName != "") {
                base.SetDimension(Dimension.CATEGORY_NAME, pCategoryName);
            }
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
        }

        public PayoutOptionClickEvent(string pCategoryName, string pProviderName, double pCount) : base(new EventDetails("payout", "option", "click")) {
            if (pCategoryName != null && pCategoryName != "") {
                base.SetDimension(Dimension.CATEGORY_NAME, pCategoryName);
            }
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

    /**
     * {count} can reflect the number of items or their value (ex. value in in-app currency)
     */
    public class PayoutOptionConfirmEvent: StandardUserEvent {
        public PayoutOptionConfirmEvent(string pCategoryName, string pProviderName) : base(new EventDetails("payout", "option", "confirm")) {
            if (pCategoryName != null && pCategoryName != "") {
                base.SetDimension(Dimension.CATEGORY_NAME, pCategoryName);
            }
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
        }

        public PayoutOptionConfirmEvent(string pCategoryName, string pProviderName, double pCount) : base(new EventDetails("payout", "option", "confirm")) {
            if (pCategoryName != null && pCategoryName != "") {
                base.SetDimension(Dimension.CATEGORY_NAME, pCategoryName);
            }
            if (pProviderName != null && pProviderName != "") {
                base.SetDimension(Dimension.PROVIDER_NAME, pProviderName);
            }
            base.SetValueAndUnit(pCount, JustTrack.Unit.Count);
        }
    }

#if UNITY_ANDROID
    internal static class AndroidEventBuilder {
        internal static AndroidJavaClass TimeUnitGroupClass = new AndroidJavaClass("io.justtrack.TimeUnitGroup");

        internal static AndroidJavaObject buildEvent(UserEventBase pEvent) {
            string custom1 = pEvent.GetDimension(Dimension.CUSTOM_1);
            string custom2 = pEvent.GetDimension(Dimension.CUSTOM_2);
            string custom3 = pEvent.GetDimension(Dimension.CUSTOM_3);
            AndroidJavaObject javaEvent = null;
            switch (pEvent.Name.Name) {
            case "user_screen_show":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "screen" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserScreenShowEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_notification_show":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "notification" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserNotificationShowEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_dialog_show":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "dialog" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserDialogShowEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_button_show":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "button" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserButtonShowEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_card_show":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "card" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserCardShowEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_button_click":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "button" && pEvent.Name.Action == "click") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserButtonClickEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_notification_click":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "notification" && pEvent.Name.Action == "click") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserNotificationClickEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_card_click":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "card" && pEvent.Name.Action == "click") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UserCardClickEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "user_rating_provide":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "rating" && pEvent.Name.Action == "provide") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingProvideEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingProvideEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                    }
                }
                break;
            case "user_rating_positive":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "rating" && pEvent.Name.Action == "positive") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingPositiveEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingPositiveEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                    }
                }
                break;
            case "user_rating_neutral":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "rating" && pEvent.Name.Action == "neutral") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingNeutralEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingNeutralEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                    }
                }
                break;
            case "user_rating_negative":
                if (pEvent.Name.Category == "user" && pEvent.Name.Element == "rating" && pEvent.Name.Action == "negative") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingNegativeEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.UserRatingNegativeEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                    }
                }
                break;
            case "registration_process_start":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "process" && pEvent.Name.Action == "start") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessStartEvent");
                }
                break;
            case "registration_process_finish":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "process" && pEvent.Name.Action == "finish") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessFinishEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessFinishEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessFinishEvent");
                    }
                }
                break;
            case "registration_process_fail":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "process" && pEvent.Name.Action == "fail") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessFailEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessFailEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.RegistrationProcessFailEvent");
                    }
                }
                break;
            case "registration_tos_show":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "tos" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationTosShowEvent");
                }
                break;
            case "registration_tos_accept":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "tos" && pEvent.Name.Action == "accept") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationTosAcceptEvent");
                }
                break;
            case "registration_tos_decline":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "tos" && pEvent.Name.Action == "decline") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationTosDeclineEvent");
                }
                break;
            case "registration_age_request":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "age" && pEvent.Name.Action == "request") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationAgeRequestEvent");
                }
                break;
            case "registration_age_provide":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "age" && pEvent.Name.Action == "provide") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationAgeProvideEvent", pEvent.GetDimension(Dimension.AGE));
                }
                break;
            case "registration_age_decline":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "age" && pEvent.Name.Action == "decline") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationAgeDeclineEvent");
                }
                break;
            case "registration_gender_request":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "gender" && pEvent.Name.Action == "request") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationGenderRequestEvent");
                }
                break;
            case "registration_gender_provide":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "gender" && pEvent.Name.Action == "provide") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationGenderProvideEvent", pEvent.GetDimension(Dimension.GENDER));
                }
                break;
            case "registration_gender_decline":
                if (pEvent.Name.Category == "registration" && pEvent.Name.Element == "gender" && pEvent.Name.Action == "decline") {
                    javaEvent = new AndroidJavaObject("io.justtrack.RegistrationGenderDeclineEvent");
                }
                break;
            case "login_loginmethod_select":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "loginmethod" && pEvent.Name.Action == "select") {
                    javaEvent = new AndroidJavaObject("io.justtrack.LoginLoginmethodSelectEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME));
                }
                break;
            case "login_providerauthorization_start":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "providerauthorization" && pEvent.Name.Action == "start") {
                    javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationStartEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME));
                }
                break;
            case "login_providerauthorization_finish":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "providerauthorization" && pEvent.Name.Action == "finish") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationFinishEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationFinishEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationFinishEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME));
                    }
                }
                break;
            case "login_providerauthorization_fail":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "providerauthorization" && pEvent.Name.Action == "fail") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationFailEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationFailEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProviderauthorizationFailEvent", pEvent.GetDimension(Dimension.PROVIDER_NAME));
                    }
                }
                break;
            case "login_process_start":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "process" && pEvent.Name.Action == "start") {
                    javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessStartEvent");
                }
                break;
            case "login_process_finish":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "process" && pEvent.Name.Action == "finish") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessFinishEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessFinishEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessFinishEvent");
                    }
                }
                break;
            case "login_process_fail":
                if (pEvent.Name.Category == "login" && pEvent.Name.Element == "process" && pEvent.Name.Action == "fail") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessFailEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessFailEvent", pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.LoginProcessFailEvent");
                    }
                }
                break;
            case "usage_permission_required":
                if (pEvent.Name.Category == "usage" && pEvent.Name.Element == "permission" && pEvent.Name.Action == "required") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UsagePermissionRequiredEvent");
                }
                break;
            case "usage_permission_grant":
                if (pEvent.Name.Category == "usage" && pEvent.Name.Element == "permission" && pEvent.Name.Action == "grant") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UsagePermissionGrantEvent");
                }
                break;
            case "usage_request_show":
                if (pEvent.Name.Category == "usage" && pEvent.Name.Element == "request" && pEvent.Name.Action == "show") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UsageRequestShowEvent");
                }
                break;
            case "usage_request_accept":
                if (pEvent.Name.Category == "usage" && pEvent.Name.Element == "request" && pEvent.Name.Action == "accept") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UsageRequestAcceptEvent");
                }
                break;
            case "usage_request_decline":
                if (pEvent.Name.Category == "usage" && pEvent.Name.Element == "request" && pEvent.Name.Action == "decline") {
                    javaEvent = new AndroidJavaObject("io.justtrack.UsageRequestDeclineEvent");
                }
                break;
            case "verification_process_start":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "start") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessStartEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_cancel":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "cancel") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessCancelEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_finish":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "finish") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessFinishEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_fail":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "fail") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessFailEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_numberrequested":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "numberrequested") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessNumberrequestedEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_numbervalid":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "numbervalid") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessNumbervalidEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_coderequested":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "coderequested") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessCoderequestedEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "verification_process_codevalid":
                if (pEvent.Name.Category == "verification" && pEvent.Name.Element == "process" && pEvent.Name.Action == "codevalid") {
                    javaEvent = new AndroidJavaObject("io.justtrack.VerificationProcessCodevalidEvent", pEvent.GetDimension(Dimension.METHOD));
                }
                break;
            case "ad_interstitial_success":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "success") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialSuccessEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.GetDimension(Dimension.TEST_GROUP), pEvent.GetDimension(Dimension.AD_SEGMENT_NAME), pEvent.GetDimension(Dimension.AD_INSTANCE_NAME), pEvent.Value);
                    }
                }
                break;
            case "ad_rewarded_success":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "success") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedSuccessEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.GetDimension(Dimension.TEST_GROUP), pEvent.GetDimension(Dimension.AD_SEGMENT_NAME), pEvent.GetDimension(Dimension.AD_INSTANCE_NAME), pEvent.Value);
                    }
                }
                break;
            case "ad_banner_success":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "banner" && pEvent.Name.Action == "success") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerSuccessEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.GetDimension(Dimension.TEST_GROUP), pEvent.GetDimension(Dimension.AD_SEGMENT_NAME), pEvent.GetDimension(Dimension.AD_INSTANCE_NAME), pEvent.Value);
                    }
                }
                break;
            case "ad_offerwall_load":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "offerwall" && pEvent.Name.Action == "load") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_offerwall_open":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "offerwall" && pEvent.Name.Action == "open") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_offerwall_close":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "offerwall" && pEvent.Name.Action == "close") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_offerwall_click":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "offerwall" && pEvent.Name.Action == "click") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_offerwall_show":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "offerwall" && pEvent.Name.Action == "show") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdOfferwallShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_load":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "load") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_open":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "open") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_close":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "close") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_click":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "click") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_show":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "show") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_load":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "load") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_open":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "open") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_close":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "close") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_click":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "click") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_show":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "show") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_banner_load":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "banner" && pEvent.Name.Action == "load") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerLoadEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_banner_open":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "banner" && pEvent.Name.Action == "open") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerOpenEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_banner_close":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "banner" && pEvent.Name.Action == "close") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerCloseEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_banner_click":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "banner" && pEvent.Name.Action == "click") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerClickEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_banner_show":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "banner" && pEvent.Name.Action == "show") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdBannerShowEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_start":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "start") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialStartEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialStartEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialStartEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_interstitial_stop":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "interstitial" && pEvent.Name.Action == "stop") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialStopEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialStopEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdInterstitialStopEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_start":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "start") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedStartEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedStartEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedStartEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "ad_rewarded_stop":
                if (pEvent.Name.Category == "ad" && pEvent.Name.Element == "rewarded" && pEvent.Name.Action == "stop") {
                    if (pEvent.Unit == Unit.Milliseconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedStopEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("MILLISECONDS"));
                    } else if (pEvent.Unit == Unit.Seconds) {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedStopEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT), pEvent.Value, TimeUnitGroupClass.GetStatic<AndroidJavaObject>("SECONDS"));
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.AdRewardedStopEvent", pEvent.GetDimension(Dimension.AD_SDK_NAME), pEvent.GetDimension(Dimension.AD_NETWORK), pEvent.GetDimension(Dimension.AD_PLACEMENT));
                    }
                }
                break;
            case "progression_level_start":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "level" && pEvent.Name.Action == "start") {
                    javaEvent = new AndroidJavaObject("io.justtrack.ProgressionLevelStartEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "progression_quest_start":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "quest" && pEvent.Name.Action == "start") {
                    javaEvent = new AndroidJavaObject("io.justtrack.ProgressionQuestStartEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "progression_level_finish":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "level" && pEvent.Name.Action == "finish") {
                    javaEvent = new AndroidJavaObject("io.justtrack.ProgressionLevelFinishEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "progression_level_fail":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "level" && pEvent.Name.Action == "fail") {
                    javaEvent = new AndroidJavaObject("io.justtrack.ProgressionLevelFailEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "progression_quest_finish":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "quest" && pEvent.Name.Action == "finish") {
                    javaEvent = new AndroidJavaObject("io.justtrack.ProgressionQuestFinishEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "progression_quest_fail":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "quest" && pEvent.Name.Action == "fail") {
                    javaEvent = new AndroidJavaObject("io.justtrack.ProgressionQuestFailEvent", pEvent.GetDimension(Dimension.ELEMENT_NAME), pEvent.GetDimension(Dimension.ELEMENT_ID));
                }
                break;
            case "progression_resource_sink":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "resource" && pEvent.Name.Action == "sink") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.ProgressionResourceSinkEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.ProgressionResourceSinkEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID));
                    }
                }
                break;
            case "progression_resource_source":
                if (pEvent.Name.Category == "progression" && pEvent.Name.Element == "resource" && pEvent.Name.Action == "source") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.ProgressionResourceSourceEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.ProgressionResourceSourceEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID));
                    }
                }
                break;
            case "purchase_option_click":
                if (pEvent.Name.Category == "purchase" && pEvent.Name.Element == "option" && pEvent.Name.Action == "click") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.PurchaseOptionClickEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.PurchaseOptionClickEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID));
                    }
                }
                break;
            case "purchase_option_confirm":
                if (pEvent.Name.Category == "purchase" && pEvent.Name.Element == "option" && pEvent.Name.Action == "confirm") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.PurchaseOptionConfirmEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.PurchaseOptionConfirmEvent", pEvent.GetDimension(Dimension.ITEM_TYPE), pEvent.GetDimension(Dimension.ITEM_NAME), pEvent.GetDimension(Dimension.ITEM_ID));
                    }
                }
                break;
            case "payout_option_click":
                if (pEvent.Name.Category == "payout" && pEvent.Name.Element == "option" && pEvent.Name.Action == "click") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.PayoutOptionClickEvent", pEvent.GetDimension(Dimension.CATEGORY_NAME), pEvent.GetDimension(Dimension.PROVIDER_NAME), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.PayoutOptionClickEvent", pEvent.GetDimension(Dimension.CATEGORY_NAME), pEvent.GetDimension(Dimension.PROVIDER_NAME));
                    }
                }
                break;
            case "payout_option_confirm":
                if (pEvent.Name.Category == "payout" && pEvent.Name.Element == "option" && pEvent.Name.Action == "confirm") {
                    if (pEvent.Unit == Unit.Count) {
                        javaEvent = new AndroidJavaObject("io.justtrack.PayoutOptionConfirmEvent", pEvent.GetDimension(Dimension.CATEGORY_NAME), pEvent.GetDimension(Dimension.PROVIDER_NAME), pEvent.Value);
                    } else {
                        javaEvent = new AndroidJavaObject("io.justtrack.PayoutOptionConfirmEvent", pEvent.GetDimension(Dimension.CATEGORY_NAME), pEvent.GetDimension(Dimension.PROVIDER_NAME));
                    }
                }
                break;
            default:
                break;
            }
            if (javaEvent == null) {
                AndroidJavaObject fullName;
                if (pEvent.Name.Category != null && pEvent.Name.Element != null && pEvent.Name.Action != null) {
                    fullName = new AndroidJavaObject("io.justtrack.EventDetails", pEvent.Name.Name, pEvent.Name.Category, pEvent.Name.Element, pEvent.Name.Action);
                } else {
                    fullName = new AndroidJavaObject("io.justtrack.EventDetails", pEvent.Name.Name);
                }
                javaEvent = new AndroidJavaObject("io.justtrack.UserEvent", fullName);
                if (Unit.Count == pEvent.Unit) {
                    javaEvent.Call<AndroidJavaObject>("setCount", pEvent.Value);
                }
                if (Unit.Milliseconds == pEvent.Unit) {
                    javaEvent.Call<AndroidJavaObject>("setMilliseconds", pEvent.Value);
                }
                if (Unit.Seconds == pEvent.Unit) {
                    javaEvent.Call<AndroidJavaObject>("setSeconds", pEvent.Value);
                }
            }
            if (custom1 != null) {
              javaEvent.Call<AndroidJavaObject>("setDimension1", custom1);
            }
            if (custom2 != null) {
              javaEvent.Call<AndroidJavaObject>("setDimension2", custom2);
            }
            if (custom3 != null) {
              javaEvent.Call<AndroidJavaObject>("setDimension3", custom3);
            }
            return javaEvent.Call<AndroidJavaObject>("build");
        }
    }
#endif
}
