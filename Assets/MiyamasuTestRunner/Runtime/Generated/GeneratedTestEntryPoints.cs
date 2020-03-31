
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Miyamasu;
public class BasicLayoutTest_Miyamasu {
    [UnityTest] public IEnumerator BasicPattern() {
        var instance = new BasicLayoutTest();
        instance.SetInfo("BasicLayoutTest", "BasicPattern");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.BasicPattern();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator ComplexPattern() {
        var instance = new BasicLayoutTest();
        instance.SetInfo("BasicLayoutTest", "ComplexPattern");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexPattern();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}