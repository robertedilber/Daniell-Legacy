using Daniell.Runtime.Systems.Events;
using NUnit.Framework;
using UnityEngine;

namespace Daniell.UnitTests
{
    /// <summary>
    /// Tests related to events
    /// </summary>
    public class EventTests
    {
        /// <summary>
        /// Tests the link between events and subscribers
        /// </summary>
        [Test]
        public void EventToSubscriberLink()
        {
            // Create a new VoidEvent and add a listener
            bool eventWasTriggered = false;
            VoidEvent e = ScriptableObject.CreateInstance<VoidEvent>();
            e.AddListener(() => eventWasTriggered = true);

            // Fire the event
            e.Raise();

            // Assert test
            Assert.IsTrue(eventWasTriggered);
        }
    }
}