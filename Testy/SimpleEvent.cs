using NUnit.Framework;
using Pozorovatel;
using System;
using System.Threading;

namespace Tests
{
    public class SimpleEventTests
    {
        class SimpleEventFooClass
        {
            public SimpleEvent Event { get; set; } = new SimpleEvent();

            public void StartCountdown()
            {
                Thread.Sleep(new Random().Next(0, 4000));
                Event.Invoke();
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Tento test není monı use-case eventu a sloí pouze pro test
        /// </summary>
        [Test]
        public void SimpleInvoke()
        {
            var eventClass = new SimpleEventFooClass();
            int invokeCount = 0;

            //Jakmile nìkdo spustí event, dej mi vìdìt 
            eventClass.Event.Add(() =>
            {
                invokeCount++;
            });

            //Spus event - normálnì by dìlal tøída sama
            eventClass.Event.Invoke();
            eventClass.Event.Invoke();


            //Otestuj
            Assert.AreEqual(2, invokeCount);
        }

        /// <summary>
        /// Tento test uívá uspávání vláken pro názornìjší pouití eventù
        /// </summary>
        [Test]
        public void TimedInsertAndInvoke()
        {
            var eventClass = new SimpleEventFooClass();
            int invokeCount = 0;
            Mutex mutex = new Mutex();

            //Jakmile nìkdo spustí event, dej mi vìdìt 
            eventClass.Event.Add(() =>
            {
                mutex.WaitOne();
                invokeCount++;
                mutex.ReleaseMutex();
            });

            //Spus count down na novém vláknì (kterı potom zavolá event invoke)
            new Thread(() =>
            {
                eventClass.StartCountdown();
            }).Start();

            //Poèkej na count down
            Thread.Sleep(6000);

            //Otestuj
            mutex.WaitOne();
            Assert.AreEqual(1, invokeCount);
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Tento test uívá uspávání vláken pro názornìjší pouití eventù
        /// </summary>
        [Test]
        public void MultipleTimedInsertAndInvoke()
        {
            var eventClass = new SimpleEventFooClass();
            int invokeCount = 0;
            Mutex mutex = new Mutex();

            //Jakmile nìkdo spustí event, dej mi vìdìt 
            eventClass.Event.Add(() =>
            {
                mutex.WaitOne();
                invokeCount++;
                mutex.ReleaseMutex();
            });

            //Spus count down na novém vláknì (kterı potom zavolá event invoke)
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    eventClass.StartCountdown();
                }).Start();
            }

            //Poèkej na count down
            Thread.Sleep(6000);

            //Otestuj
            mutex.WaitOne();
            Assert.AreEqual(10, invokeCount);
            mutex.ReleaseMutex();
        }
    }
}