
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Miyamasu;
public class BasicLayoutTests_Miyamasu {
    [UnityTest] public IEnumerator BasicPattern() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "BasicPattern");
        
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
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "ComplexPattern");
        
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
    [UnityTest] public IEnumerator ComplexPattern2() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "ComplexPattern2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexPattern2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmoji() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "WithEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiComplex() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "WithEmojiComplex");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiComplex();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiComplex2() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "WithEmojiComplex2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiComplex2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class RelayoutTests_Miyamasu {
    [UnityTest] public IEnumerator RelayoutWithEmoji() {
        var instance = new RelayoutTests();
        instance.SetInfo("RelayoutTests", "RelayoutWithEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.RelayoutWithEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator RelayoutWithEmojiWithDelay() {
        var instance = new RelayoutTests();
        instance.SetInfo("RelayoutTests", "RelayoutWithEmojiWithDelay");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.RelayoutWithEmojiWithDelay();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}