using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

public class ReadmeEditorTests
{
    [Test]
    public void SelectReadme_WhenNoReadmeExists_ReturnsNull()
    {
        // This would test the SelectReadme method
        // Note: Would need to refactor ReadmeEditor to make methods testable
        var result = ReadmeEditorTestHelper.CallSelectReadme();
        Assert.IsNull(result);
    }

    [Test]
    public void LoadLayout_WhenCalled_DoesNotThrowException()
    {
        // Test that LoadLayout doesn't crash
        Assert.DoesNotThrow(() => {
            ReadmeEditorTestHelper.CallLoadLayout();
        });
    }
}

// Helper class to access private/static methods for testing
public static class ReadmeEditorTestHelper
{
    public static Readme CallSelectReadme()
    {
        var method = typeof(ReadmeEditor).GetMethod("SelectReadme", 
            BindingFlags.NonPublic | BindingFlags.Static);
        return (Readme)method.Invoke(null, null);
    }

    public static void CallLoadLayout()
    {
        var method = typeof(ReadmeEditor).GetMethod("LoadLayout", 
            BindingFlags.NonPublic | BindingFlags.Static);
        method.Invoke(null, null);
    }
}