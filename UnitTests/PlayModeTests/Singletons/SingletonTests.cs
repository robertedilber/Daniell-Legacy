using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Daniell.UnitTests
{
    /// <summary>
    /// Tests related to singletons
    /// </summary>
    public class SingletonTests
    {
        /// <summary>
        /// Tests if the DelayedInstanceCall pattern of the singleton works
        /// </summary>
        [UnityTest]
        public IEnumerator DelayedInstanceCallTest()
        {
            bool valueWasSet = false;

            // Call a test using the instance
            FakeSingletonInstance.Test(() => valueWasSet = true);

            // Skip a frame
            yield return null;

            // Create instance after singleton call was made
            FakeSingletonInstance instance = new GameObject("Fake Singleton").AddComponent<FakeSingletonInstance>();

            // Asset test
            Assert.IsTrue(valueWasSet);
        }

        /// <summary>
        /// Remove objects created by the previous test so that other tests are reliable
        /// </summary>
        [TearDown]
        public void RemoveAllAfterEachTest()
        {
            // Find all the created objects
            Transform[] objects = Object.FindObjectsOfType<Transform>();

            // Remove each objects
            foreach (var obj in objects)
            {
                Object.Destroy(obj.gameObject);
            }
        }
    }
}
