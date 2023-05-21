﻿using Monitoring.Core.Configurations;

namespace Monitoring.Core.Services;

public interface ISynchronizerService
{
    Task NotifySingleProjectAsync( ProjectConfiguration project );
    Task NotifyAllProjectsAsync( IEnumerable<ProjectConfiguration> projects );
}