namespace ScadaCore.Drivers;

public class DigitalSimulationDriver : IDigitalSimulationDriver {
    public bool? Read(int inputAddress) {
        return inputAddress % 2 == 0;
    }
}