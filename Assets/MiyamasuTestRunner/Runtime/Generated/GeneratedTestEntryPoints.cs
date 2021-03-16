
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
    [UnityTest] public IEnumerator SimpleEmojiAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "SimpleEmojiAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SimpleEmojiAsync();
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
    [UnityTest] public IEnumerator ImageAndButtonAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "ImageAndButtonAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ImageAndButtonAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator LayoutOneLineTextDoesNotContainInTheEndOfLineAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "LayoutOneLineTextDoesNotContainInTheEndOfLineAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.LayoutOneLineTextDoesNotContainInTheEndOfLineAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator LayoutHeadOfTextAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "LayoutHeadOfTextAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.LayoutHeadOfTextAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MultilineAfterFilledOneLineAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "MultilineAfterFilledOneLineAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MultilineAfterFilledOneLineAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MultilineAfterFilledOneLineLongAsync() {
        var instance = new AsyncLayoutTests();
        instance.SetInfo("AsyncLayoutTests", "MultilineAfterFilledOneLineLongAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MultilineAfterFilledOneLineLongAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class AsyncMethodTests_Miyamasu {
    [UnityTest] public IEnumerator AsyncMethod() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethod");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethod();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodComplexPattern() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodComplexPattern");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodComplexPattern();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodComplexPattern2() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodComplexPattern2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodComplexPattern2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodSimpleEmoji() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodSimpleEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodSimpleEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmoji() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmojiComplex() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithEmojiComplex");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmojiComplex();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmojiComplex2() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithEmojiComplex2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmojiComplex2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodDetectMissingEmoji() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodDetectMissingEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodDetectMissingEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodMark() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodMark");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodMark();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodImageAndButton() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodImageAndButton");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodImageAndButton();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodComplexPatternWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodComplexPatternWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodComplexPatternWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodComplexPattern2WithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodComplexPattern2WithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodComplexPattern2WithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodSimpleEmojiWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodSimpleEmojiWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodSimpleEmojiWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmojiWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithEmojiWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmojiWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmojiComplexWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithEmojiComplexWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmojiComplexWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmojiComplex2WithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodWithEmojiComplex2WithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmojiComplex2WithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodDetectMissingEmojiWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodDetectMissingEmojiWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodDetectMissingEmojiWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodMarkWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodMarkWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodMarkWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodImageAndButtonWithRelayout() {
        var instance = new AsyncMethodTests();
        instance.SetInfo("AsyncMethodTests", "AsyncMethodImageAndButtonWithRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodImageAndButtonWithRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class AsyncMissingCharTests_Miyamasu {
    [UnityTest] public IEnumerator GetMissingCharAsync() {
        var instance = new AsyncMissingCharTests();
        instance.SetInfo("AsyncMissingCharTests", "GetMissingCharAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingCharAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class AsyncRelayoutTests_Miyamasu {
    [UnityTest] public IEnumerator RelayoutWithEmojiAsync() {
        var instance = new AsyncRelayoutTests();
        instance.SetInfo("AsyncRelayoutTests", "RelayoutWithEmojiAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.RelayoutWithEmojiAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator RelayoutWithEmojiWithDelayAsync() {
        var instance = new AsyncRelayoutTests();
        instance.SetInfo("AsyncRelayoutTests", "RelayoutWithEmojiWithDelayAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.RelayoutWithEmojiWithDelayAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator RelayoutWithLongEmojiAsync() {
        var instance = new AsyncRelayoutTests();
        instance.SetInfo("AsyncRelayoutTests", "RelayoutWithLongEmojiAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.RelayoutWithLongEmojiAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class AsyncSameTimeTests_Miyamasu {
    [UnityTest] public IEnumerator AsyncMethodSameTime() {
        var instance = new AsyncSameTimeTests();
        instance.SetInfo("AsyncSameTimeTests", "AsyncMethodSameTime");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodSameTime();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator AsyncMethodWithEmojiSameTime() {
        var instance = new AsyncSameTimeTests();
        instance.SetInfo("AsyncSameTimeTests", "AsyncMethodWithEmojiSameTime");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.AsyncMethodWithEmojiSameTime();
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
    [UnityTest] public IEnumerator ImageAndButton() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "ImageAndButton");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ImageAndButton();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator LayoutOneLineTextDoesNotContainInTheEndOfLine() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "LayoutOneLineTextDoesNotContainInTheEndOfLine");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.LayoutOneLineTextDoesNotContainInTheEndOfLine();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator LayoutHeadOfText() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "LayoutHeadOfText");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.LayoutHeadOfText();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MultilineAfterFilledOneLine() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "MultilineAfterFilledOneLine");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MultilineAfterFilledOneLine();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MultilineAfterFilledOneLineLong() {
        var instance = new BasicLayoutTests();
        instance.SetInfo("BasicLayoutTests", "MultilineAfterFilledOneLineLong");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MultilineAfterFilledOneLineLong();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class ComplexTests_Miyamasu {
    [UnityTest] public IEnumerator ComplexMissingCharAsync() {
        var instance = new ComplexTests();
        instance.SetInfo("ComplexTests", "ComplexMissingCharAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexMissingCharAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class ErrorTests_Miyamasu {
    [UnityTest] public IEnumerator EmptyStringError() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "EmptyStringError");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.EmptyStringError();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator EmptyStringErrorAsync() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "EmptyStringErrorAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.EmptyStringErrorAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MarkContinues() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "MarkContinues");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MarkContinues();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MarkContinuesAsync() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "MarkContinuesAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MarkContinuesAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayoutAsync() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayoutAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayoutAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayoutRelayout() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayoutRelayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayoutRelayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout2() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout2Relayout() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout2Relayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout2Relayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout2Async() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout2Async");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout2Async();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout2RelayoutAsync() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout2RelayoutAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout2RelayoutAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout3() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout3");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout3();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout3Relayout() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout3Relayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout3Relayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout3Async() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout3Async");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout3Async();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SpacesAndLayout3RelayoutAsync() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "SpacesAndLayout3RelayoutAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SpacesAndLayout3RelayoutAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator IsEmojiOrNotComplexCase() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "IsEmojiOrNotComplexCase");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.IsEmojiOrNotComplexCase();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator IsEmojiOrNotComplexCase2() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "IsEmojiOrNotComplexCase2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.IsEmojiOrNotComplexCase2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator IsEmojiOrNotComplexCase3() {
        var instance = new ErrorTests();
        instance.SetInfo("ErrorTests", "IsEmojiOrNotComplexCase3");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.IsEmojiOrNotComplexCase3();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}
public class FrameTests_Miyamasu {
    [UnityTest] public IEnumerator BasicPatternFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "BasicPatternFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.BasicPatternFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator ComplexPatternFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "ComplexPatternFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexPatternFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator ComplexPattern2Frames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "ComplexPattern2Frames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ComplexPattern2Frames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator SimpleEmojiFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "SimpleEmojiFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.SimpleEmojiFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "WithEmojiFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiComplexFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "WithEmojiComplexFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiComplexFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator WithEmojiComplex2Frames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "WithEmojiComplex2Frames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.WithEmojiComplex2Frames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator DetectMissingEmojiFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "DetectMissingEmojiFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.DetectMissingEmojiFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator MarkFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "MarkFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.MarkFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator ImageAndButtonFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "ImageAndButtonFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.ImageAndButtonFrames();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator DetectMissingEmojiContinuesFrames() {
        var instance = new FrameTests();
        instance.SetInfo("FrameTests", "DetectMissingEmojiContinuesFrames");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.DetectMissingEmojiContinuesFrames();
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
    [UnityTest] public IEnumerator GetMissingChar2() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar2");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar2();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator GetMissingChar3() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar3");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar3();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator GetMissingChar3Relayout() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar3Relayout");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar3Relayout();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator GetMissingCharAsync() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingCharAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingCharAsync();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator GetMissingChar2Async() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar2Async");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar2Async();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator GetMissingChar3Async() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar3Async");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar3Async();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
    [UnityTest] public IEnumerator GetMissingChar3RelayoutAsync() {
        var instance = new MissingCharTests();
        instance.SetInfo("MissingCharTests", "GetMissingChar3RelayoutAsync");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.GetMissingChar3RelayoutAsync();
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
    [UnityTest] public IEnumerator RelayoutWithLongEmoji() {
        var instance = new RelayoutTests();
        instance.SetInfo("RelayoutTests", "RelayoutWithLongEmoji");
        
        try {
            instance.Setup();
        } catch (Exception e) {
            instance.SetupFailed(e);
            throw;
        }
        var startDate = DateTime.Now;
        yield return instance.RelayoutWithLongEmoji();
        instance.MarkAsPassed((DateTime.Now - startDate).ToString());

        
        try {
            instance.Teardown();
        } catch (Exception e) {
            instance.TeardownFailed(e);
            throw;
        }
    }
}