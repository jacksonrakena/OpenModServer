using System.ComponentModel;

namespace OpenModServer.Data.Releases.Safety;

public enum VirusTotalScanStatus
{
    /// <summary>
    ///     OMS is waiting on VirusTotal to finish the analysis of this release.
    /// </summary>
    [Description("Waiting on VirusTotal scan")]
    WaitingOnVirusTotalToComplete,
    
    /// <summary>
    ///     VirusTotal engines have not reported any SUSPICIOUS or MALICIOUS file classes.
    /// </summary>
    Clean,
    
    /// <summary>
    ///     VirusTotal engines have detected at least one SUSPICIOUS or MALICIOUS file class.
    /// </summary>
    Dirty,
    
}