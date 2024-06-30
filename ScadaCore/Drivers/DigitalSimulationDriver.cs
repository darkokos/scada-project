namespace ScadaCore.Drivers;

public class DigitalSimulationDriver : IDigitalDriver {
    public bool? Read(int inputAddress) {
        return inputAddress % 2 == 0;
    }
}