
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Miyamasu;
public class AsyncLayoutTests_Miyamasu {
    [UnityTest] public IEnumerator BasicPatternAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "BasicPatternAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.BasicPatternAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator ComplexPatternAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "ComplexPatternAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexPatternAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator ComplexPattern2Async() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "ComplexPattern2Async");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexPattern2Async();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "WithEmojiAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiComplexAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "WithEmojiComplexAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiComplexAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiComplex2Async() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "WithEmojiComplex2Async");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiComplex2Async();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator DetectMissingEmojiAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "DetectMissingEmojiAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.DetectMissingEmojiAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MarkAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "MarkAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MarkAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
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
    [UnityTest] public IEnumerator DetectMissingEmoji() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "DetectMissingEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.DetectMissingEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator Mark() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "Mark");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.Mark();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class MissingCharTests_Miyamasu {
    [UnityTest] public IEnumerator GetMissingChar() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar();
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