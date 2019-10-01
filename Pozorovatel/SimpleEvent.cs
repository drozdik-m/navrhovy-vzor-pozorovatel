using System;
using System.Collections.Generic;
using System.Text;

namespace Pozorovatel
{
    class SimpleEvent
    {
        private List<VoidFunction> observerFunctions = new List<VoidFunction>();

        /// <summary>
        /// Metoda, která přidá nového pozorovatele (resp. jeho funkci)
        /// </summary>
        /// <param name="newObserverFunction"></param>
        public void Add(VoidFunction newObserverFunction)
        {
            observerFunctions.Add(newObserverFunction);
        }

        /// <summary>
        /// Metoda, která je volána subjektem a která notifikuje všechny pozorovatele
        /// </summary>
        public void Invoke()
        {
            foreach (var observerFunction in observerFunctions)
                observerFunction();
        }
    }

    delegate void VoidFunction();
}
