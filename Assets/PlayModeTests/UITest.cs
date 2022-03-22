using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CustomControls;

public class UITest
{
    // A Test behaves as an ordinary method
    [Test]
    public void UITestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UITestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new WaitForSeconds(5);
        GameObject particleToggle = GameObject.Find("CustomToggle");
        ToggleControl toggle = particleToggle.GetComponent<ToggleControl>();
        Assert.IsTrue(toggle.IsChecked, "Particle Toggle was not Checked");
    }
}
