using ScadaCore.Drivers;
using ScadaCore.Models;

namespace ScadaCore.Services;

public class TagProcessing(IServiceScopeFactory serviceScopeFactory) : BackgroundService {
    private IAnalogRealTimeDriver? _analogRealTimeDriver;
    private IAnalogSimulationDriver? _analogSimulationDriver;
    private IDigitalRealTimeDriver? _digitalRealTimeDriver;
    private IDigitalSimulationDriver? _digitalSimulationDriver;
    private ITagLogService _tagLogService;
    private IAlarmService _alarmService;
    private IAlarmLogService _alarmLogService;
    
    private async Task HandleSimulatedAnalogTag(AnalogInputTag analogInputTag) {
        var value = _analogSimulationDriver?.Read(analogInputTag.InputOutputAddress);
        if (value is null)
            return;

        var tagLog = await _tagLogService.CreateTagLogAsync(
            new AnalogTagLog(analogInputTag.Name, DateTime.Now, (decimal) value)
        );

        foreach (var alarmId in analogInputTag.AlarmIds) {
            var alarm = await _alarmService.GetAlarmAsync(alarmId);
            if (alarm == null)
                continue;
            
            if (alarm.IsTriggered((decimal)value))
                await _alarmLogService.CreateAlarmLogAsync(
                    new AlarmLog(alarm.Id, alarm.Type, alarm.Priority, alarm.Unit, DateTime.Now)
                );
        }
    }
    
    private async Task HandleNonSimulatedAnalogTag(AnalogInputTag analogInputTag) {
        var value = _analogRealTimeDriver?.Read(analogInputTag.InputOutputAddress);
        if (value == null)
            return;
        
        var tagLog = await _tagLogService.CreateAnalogTagLogAsync(value);
        
        foreach (var alarmId in analogInputTag.AlarmIds) {
            var alarm = await _alarmService.GetAlarmAsync(alarmId);
            if (alarm == null)
                continue;
            
            if (alarm.IsTriggered(value.Value))
                await _alarmLogService.CreateAlarmLogAsync(
                    new AlarmLog(alarm.Id, alarm.Type, alarm.Priority, alarm.Unit, DateTime.Now)
                );
        }
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
            _alarmService = scope.ServiceProvider.GetRequiredService<IAlarmService>();
            _alarmLogService = scope.ServiceProvider.GetRequiredService<IAlarmLogService>();

            /*
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