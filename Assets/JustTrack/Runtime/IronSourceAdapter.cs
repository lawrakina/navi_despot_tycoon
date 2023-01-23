using System;

public interface IronSourceAdapter {
    void SetIronSourceOnImpressionHandler(Action<object> proxy);
}
