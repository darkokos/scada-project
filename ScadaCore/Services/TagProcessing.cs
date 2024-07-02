using ScadaCore.Drivers;
using ScadaCore.Models;

namespace ScadaCore.Services;

public class TagProcessing(IServiceScopeFactory serviceScopeFactory) : BackgroundService {
    private IAnalogRealTimeDriver? _analogRealTimeDriver;
    private IAnalogSimulationDriver? _analogSimulationDriver;
    private IDigitalRealTimeDriver? _digitalRealTimeDriver;
    private IDigitalSimulationDriver? _digitalSimulationDriver;
    private ITagLogService _tagLogService;
    
    private async Task HandleSimulatedAnalogTag(AnalogInputTag analogInputTag) {
        var value = _analogSimulationDriver?.Read(analogInputTag.InputOutputAddress);
        if (value is null)
            return;

        var tagLog = await _tagLogService.CreateTagLogAsync(new AnalogTagLog(analogInputTag.Name, DateTime.Now, (decimal) value));
    }
    
    private async Task HandleNonSimulatedAnalogTag(AnalogInputTag analogInputTag) {
        var value = _analogRealTimeDriver?.Read(analogInputTag.InputOutputAddress);
        if (value == null)
            return;
        
        await _tagLogService.CreateAnalogTagLogAsync(value);
    }

    private async Task HandleSimulatedDigitalTag(DigitalInputTag digitalInputTag) {
        var value = _digitalSimulationDriver?.Read(digitalInputTag.InputOutputAddress);
        if (value == null)
            return;

        await _tagLogService.CreateTagLogAsync(new DigitalTagLog(digitalInputTag.Name, DateTime.Now, (bool) value));
    }

    private async Task HandleNonSimulatedDigitalTag(DigitalInputTag digitalInputTag) {
        var value = _digitalRealTimeDriver?.Read(digitalInputTag.InputOutputAddress);
        if (value == null)
            return;

        await _tagLogService.CreateDigitalTagLogAsync(value);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            using var scope = serviceScopeFactory.CreateScope();
            var tagService = scope.ServiceProvider.GetRequiredService<ITagService>();
            _analogRealTimeDriver = scope.ServiceProvider.GetRequiredService<IAnalogRealTimeDriver>();
            _analogSimulationDriver = scope.ServiceProvider.GetRequiredService<IAnalogSimulationDriver>();
            _digitalRealTimeDriver = scope.ServiceProvider.GetRequiredService<IDigitalRealTimeDriver>();
            _digitalSimulationDriver = scope.ServiceProvider.GetRequiredService<IDigitalSimulationDriver>();
            _tagLogService = scope.ServiceProvider.GetRequiredService<ITagLogService>();

            /*
             TODO: Check if alarms were triggered and if they were, persist them
             TODO: Publish everything that was emitted/triggered
             */

            var tags = await tagService.GetAllInputTags();
            foreach (var tag in tags) {
                switch (tag) {
                    case AnalogInputTag analogInputTag:
                        if (!analogInputTag.IsScanned)
                            break;

                        if (analogInputTag.IsSimulated)
                            await HandleSimulatedAnalogTag(analogInputTag);
                        else
                            await HandleNonSimulatedAnalogTag(analogInputTag);
                        break;
                    case DigitalInputTag digitalInputTag:
                        if (!digitalInputTag.IsScanned)
                            break;

                        if (digitalInputTag.IsSimulated)
                            await HandleSimulatedDigitalTag(digitalInputTag);
                        else
                            await HandleNonSimulatedDigitalTag(digitalInputTag);
                        break;
                }
            }

            _analogRealTimeDriver.ClearValues();
            _digitalRealTimeDriver.ClearValues();
            await Task.Delay(5000, cancellationToken);
        }
    }
}