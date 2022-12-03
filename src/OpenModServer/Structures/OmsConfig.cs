﻿namespace OpenModServer.Structures;

public class OmsConfig
{
    public OmsConfigKeys Keys { get; set; }
    public OmsConfigPaths Paths { get; set; }
    public OmsConfigBranding Branding { get; set; }
}

public class OmsConfigKeys
{
    public string VirusTotal { get; set; }
}

public class OmsConfigPaths
{
    public string Uploads { get; set; }
    public string ReleaseScreenshots { get; set; }
    public string UserAccountPictures { get; set; }
}

public class OmsConfigBranding
{
    public string Name { get; set; }
}