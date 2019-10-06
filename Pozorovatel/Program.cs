using System;

namespace Pozorovatel
{
    class Program
    {
        //----------------------------------------------------------
        //                   JEDNODUCHÝ EVENT
        //----------------------------------------------------------

        /// <summary>
        /// Třída skenující vstupy na klávesnici (pouze simulátor)
        /// Tato třída používá námi vytvořený jednoduchý event
        /// </summary>
        class KeyboardScanner_SimpleEventClass
        {
            public SimpleEvent OnKeyPress { get; } = new SimpleEvent();

            public void PressKey(string key)
            {
                //--Třída si stisk zpracuje--

                //Notifikace pozorovatelů
                OnKeyPress.Invoke();
            }
        }

        static void Demo_KeyboardScanner_SimpleEventClass()
        {
            //Subjekt
            var keyboardScanner = new KeyboardScanner_SimpleEventClass();

            //Vytvoření pozorovatele (resp. my jsme pozorovatel a přidáme si funkci na zavolání/nofitikaci)
            keyboardScanner.OnKeyPress.Add(() =>
            {
                Console.WriteLine("Subjekt mě notifikoval!");
            });

            //Stiskneme klávesu a subjekt nás automaticky notifikuje
            keyboardScanner.PressKey("space");
        }

        //----------------------------------------------------------
        //                PARAMETRIZOVATELNÝ EVENT
        //----------------------------------------------------------
        /// <summary>
        /// Třída skenující vstupy na klávesnici (pouze simulátor)
        /// Tato třída používá námi vytvořený parametrizovatelný event
        /// </summary>
        class KeyboardScanner_ParameterizableEventClass
        {
            public ParameterizableEvent<KeyboardScanner_ParameterizableEventClass, KeyboardScanner_EventArguments> OnKeyPress { get; } 
                = new ParameterizableEvent<KeyboardScanner_ParameterizableEventClass, KeyboardScanner_EventArguments>();

            public void PressKey(string key)
            {
                //--Třída si stisk zpracuje--

                //Notifikace pozorovatelů
                OnKeyPress.Invoke(this, new KeyboardScanner_EventArguments(key));
            }
        }

        /// <summary>
        /// Třída nesoucí informace o události
        /// </summary>
        class KeyboardScanner_EventArguments
        {
            public string PressedKey { get; set; }

            public KeyboardScanner_EventArguments(string pressedKey)
            {
                PressedKey = pressedKey;
            }
        }

        static void Demo_KeyboardScanner_SimpleEventClassArguments()
        {
            //Subjekt
            var keyboardScanner = new KeyboardScanner_ParameterizableEventClass();

            //Vytvoření pozorovatele (resp. my jsme pozorovatel a přidáme si funkci na zavolání/nofitikaci)
            //Dozvíme se dokonce i nějaké informace
            keyboardScanner.OnKeyPress.Add((caller, args) =>
            {
                Console.WriteLine("Subjekt mě notifikoval!");
                Console.WriteLine("Stiskla se klávesa: " + args.PressedKey);
            });

            //Stiskneme klávesu a subjekt nás automaticky notifikuje
            keyboardScanner.PressKey("space");
        }

        //----------------------------------------------------------
        //                   NATIVNÍ C# EVENT
        //----------------------------------------------------------
        /// <summary>
        /// Třída skenující vstupy na klávesnici (pouze simulátor)
        /// Tato třída používá nativní C# event
        /// </summary>
        class KeyboardScanner_NativeCSharpEvent
        {
            public event KeyboardScannerHandler OnKeyPress;

            public void PressKey(string key)
            {
                //--Třída si stisk zpracuje--

                //Notifikace pozorovatelů
                OnKeyPress?.Invoke(this, new KeyboardScanner_EventArguments(key));
            }

            public delegate void KeyboardScannerHandler(KeyboardScanner_NativeCSharpEvent caller, KeyboardScanner_EventArguments args);
        }

        static void Demo_KeyboardScanner_NativeCSharpEvent()
        {
            //Subjekt
            var keyboardScanner = new KeyboardScanner_NativeCSharpEvent();

            //Vytvoření pozorovatele (resp. my jsme pozorovatel a přidáme si funkci na zavolání/nofitikaci)
            keyboardScanner.OnKeyPress += (caller, args) =>
            {
                Console.WriteLine("Subjekt mě notifikoval!");
                Console.WriteLine("Stiskla se klávesa: " + args.PressedKey);
            };

            //Stiskneme klávesu a subjekt nás automaticky notifikuje
            keyboardScanner.PressKey("space");
        }

        static void Main(string[] args)
        {
            Demo_KeyboardScanner_SimpleEventClass();
            Console.WriteLine();
            Demo_KeyboardScanner_SimpleEventClassArguments();
            Console.WriteLine();
            Demo_KeyboardScanner_NativeCSharpEvent();
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
