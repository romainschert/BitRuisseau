namespace bitruisseau_RomainSchertenleib
{
    internal static class Program
    {
        public static MqttCommunicator MqttCommunicator { get; private set; }
        public static Protocol Protocol { get; private set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MqttCommunicator mqttCommunicator = new MqttCommunicator("mqtt.blue.section-inf.ch","romain");
            Protocol protocol = new Protocol(mqttCommunicator, "romain");
            mqttCommunicator.SetProtocol(protocol);
            mqttCommunicator.Start();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(mqttCommunicator,protocol));
        }
    }
}