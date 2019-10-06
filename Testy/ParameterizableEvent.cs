using NUnit.Framework;
using Pozorovatel;
using System;
using System.Threading;

namespace Tests
{
    public class ParameterizableEventTests
    {
        class ParameterizableEventFooClass
        {
            public ParameterizableEvent<ParameterizableEventFooClass, ParameterizableEventFooArgs>  Event { get; set; } 
                = new ParameterizableEvent<ParameterizableEventFooClass, ParameterizableEventFooArgs>();

            public void StartCountdown()
            {
                var randomValue = new Random().Next(0, 4000);
                Thread.Sleep(randomValue);
                Event.Invoke(this, new ParameterizableEventFooArgs(randomValue));
            }
        }

        class ParameterizableEventFooArgs
        {
            public int SomeValue { get; set; }

            public ParameterizableEventFooArgs(int someValue)
            {
                SomeValue = someValue;
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Tento test nen� mo�n� use-case eventu a slo�� pouze pro test
        /// </summary>
        [Test]
        public void SimpleInvoke()
        {
            var eventClass = new ParameterizableEventFooClass();
            int invokeCount = 0;

            //Jakmile n�kdo spust� event, dej mi v�d�t 
            eventClass.Event.Add((caller, args) =>
            {
                invokeCount += args.SomeValue;
            });

            //Spus� event - norm�ln� by d�lal t��da sama
            eventClass.Event.Invoke(eventClass, new ParameterizableEventFooArgs(1));
            eventClass.Event.Invoke(eventClass, new ParameterizableEventFooArgs(2));


            //Otestuj
            Assert.AreEqual(3, invokeCount);
        }

        /// <summary>
        /// Tento test u��v� usp�v�n� vl�ken pro n�zorn�j�� pou�it� event�
        /// </summary>
        [Test]
        public void TimedInsertAndInvoke()
        {
            var eventClass = new ParameterizableEventFooClass();
            int invokeCount = 0;
            Mutex mutex = new Mutex();

            //Jakmile n�kdo spust� event, dej mi v�d�t 
            eventClass.Event.Add((caller, args) =>
            {
                mutex.WaitOne();
                invokeCount += args.SomeValue;
                mutex.ReleaseMutex();
            });

            //Spus� count down na nov�m vl�kn� (kter� potom zavol� event invoke)
            new Thread(() =>
            {
                eventClass.StartCountdown();
            }).Start();

            //Po�kej na count down
            Thread.Sleep(6000);

            //Otestuj
            mutex.WaitOne();
            Assert.IsTrue(invokeCount >= 0 && invokeCount <= 4000);
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Tento test u��v� usp�v�n� vl�ken pro n�zorn�j�� pou�it� event�
        /// </summary>
        [Test]
        public void MultipleTimedInsertAndInvoke()
        {
            var eventClass = new ParameterizableEventFooClass();
            int invokeCount = 0;
            Mutex mutex = new Mutex();

            //Jakmile n�kdo spust� event, dej mi v�d�t 
            eventClass.Event.Add((caller, args) =>
            {
                mutex.WaitOne();
                invokeCount += args.SomeValue;
                mutex.ReleaseMutex();
            });

            //Spus� count down na nov�m vl�kn� (kter� potom zavol� event invoke)
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    eventClass.StartCountdown();
                }).Start();
            }

            //Po�kej na count down
            Thread.Sleep(6000);

            //Otestuj
            mutex.WaitOne();
            Assert.IsTrue(invokeCount >= 0 && invokeCount <= 40000);
            mutex.ReleaseMutex();
        }
    }
}