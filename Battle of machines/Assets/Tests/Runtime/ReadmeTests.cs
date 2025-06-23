using NUnit.Framework;
using UnityEngine;

public class ReadmeTests
{
    [Test]
    public void Readme_WhenCreated_HasDefaultValues()
    {
        var readme = ScriptableObject.CreateInstance<Readme>();
        
        Assert.IsNotNull(readme);
        Assert.IsNull(readme.icon);
        Assert.IsTrue(string.IsNullOrEmpty(readme.title));
        Assert.IsFalse(readme.loadedLayout);
    }
}