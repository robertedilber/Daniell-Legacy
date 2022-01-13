using Daniell.Runtime.Helpers.Singletons;
using System;

public class FakeSingletonInstance : SingletonMonoBehaviour<FakeSingletonInstance>
{
    public static void Test(Action action)
    {
        DelayedInstanceCall(instance =>
        {
            action?.Invoke();
        });
    }
}