using System;
using System.Collections.Generic;
using System.Text;

namespace Pozorovatel
{
    public class ParameterizableEvent<Subject, Args>
    {
        private List<ParameterizableEventHandler<Subject, Args>> observerFunctions 
            = new List<ParameterizableEventHandler<Subject, Args>>();

        /// <summary>
        /// Metoda, která přidá nového pozorovatele (resp. jeho funkci)
        /// </summary>
        /// <param name="newObserverFunction"></param>
        public void Add(ParameterizableEventHandler<Subject, Args> newObserverFunction)
        {
            observerFunctions.Add(newObserverFunction);
        }

        /// <summary>
        /// Metoda, která je volána subjektem a která notifikuje všechny pozorovatele
        /// </summary>
        /// <param name="caller">Reference na subjekta, který invoke zavolal</param>
        /// <param name="arguments">Předání stavových argumentů, například jaká klávesa byla stisknuta apod.</param>
        public void Invoke(Subject caller, Args arguments)
        {
            foreach (var observerFunction in observerFunctions)
                observerFunction(caller, arguments);
        }

        
    }

    public delegate void ParameterizableEventHandler<Subject, Args>(Subject caller, Args arguments);
}
