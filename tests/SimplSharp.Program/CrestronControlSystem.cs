using Crestron.SimplSharp;

namespace SimplSharp.Program
{
    public class ControlSystem : Crestron.SimplSharpPro.CrestronControlSystem
    {

        /// <inheritdoc />
        public override void InitializeSystem()
        {
            CrestronConsole.PrintLine("System Started");

            ErrorLog.Warn("++++++++++++++++++++++++++++ Hello World +++++++++++++++++++++");
            ErrorLog.Warn("++++++++++++++++++++++++++++ Hello World +++++++++++++++++++++");
            ErrorLog.Warn("++++++++++++++++++++++++++++ Hello World +++++++++++++++++++++");
            ErrorLog.Warn("++++++++++++++++++++++++++++ Hello World +++++++++++++++++++++");
        }
    }
}