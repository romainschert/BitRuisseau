namespace bitruisseau_RomainSchertenleib
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MqttCommunicator mqttCommunicator = new MqttCommunicator("mqtt.blue.section-inf.ch","b");
            Protocol protocol = new Protocol(mqttCommunicator, "romain");
            mqttCommunicator.SetProtocol(protocol);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}