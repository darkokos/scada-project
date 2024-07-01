using ScadaCore.Drivers;
using ScadaCore.Models;

namespace ScadaCore.Services;

public class TagProcessing(IServiceScopeFactory serviceScopeFactory) : BackgroundService {
    private IAnalogRealTimeDriver? _analogRealTimeDriver;
    private IAnalogSimulationDriver? _analogSimulationDriver;
    private IDigitalRealTimeDriver? _digitalRealTimeDriver;
    private IDigitalSimulationDriver? _digitalSimulationDriver;
    
    private void HandleAnalogTag(AnalogInputTag analogInputTag) {
        if (!analogInputTag.IsScanned)
            return;

        if (analogInputTag.IsSimulated) {
            var value = _analogSimulationDriver?.Read(analogInputTag.InputOutputAddress);
            Console.WriteLine(value);
        }
        else {
            var value = _analogRealTimeDriver?.Read(analogInputTag.InputOutputAddress);
            Console.WriteLine(value);
        }
    }

    private void HandleDigitalTag(DigitalInputTag digitalInputTag) {
        if (!digitalInputTag.IsScanned)
            return;

        if (digitalInputTag.IsSimulated) {
            var value = _digitalSimulationDriver?.Read(digitalInputTag.InputOutputAddress);
            Console.WriteLine(value);
        }
        else {
            var value = _digitalRealTimeDriver?.Read(digitalInputTag.InputOutputAddress);
            Console.WriteLine(value);
        }
    }
        
    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            ITagService tagService;
            using (var scope = serviceScopeFactory.CreateScope()) {
                tagService = scope.ServiceProvider.GetRequiredService<ITagService>();
                _analogRealTimeDriver = scope.ServiceProvider.GetRequiredService<IAnalogRealTimeDriver>();
                _analogSimulationDriver = scope.ServiceProvider.GetRequiredService<IAnalogSimulationDriver>();
                _digitalRealTimeDriver = scope.ServiceProvider.GetRequiredService<IDigitalRealTimeDriver>();
                _digitalSimulationDriver = scope.ServiceProvider.GetRequiredService<IDigitalSimulationDriver>();
            }
            
            /*
             TODO: Read all intermittently emitted values
             TODO: Persist all read values
             TODO: Check if alarms were triggered and if they were, persist them
             TODO: Publish everything that was emitted/triggered
             */

            var tags = await tagService.GetAllInputTags();
            foreach (var tag in tags) {
                switch (tag) {
                    case AnalogInputTag analogInputTag:
                        HandleAnalogTag(analogInputTag);
                        break;
                    case DigitalInputTag digitalInputTag:
                        HandleDigitalTag(digitalInputTag);
                        break;
                }
            }

            _analogRealTimeDriver.ClearValues();
            _digitalRealTimeDriver.ClearValues();
            await Task.Delay(5000, cancellationToken);
        }
    }
}