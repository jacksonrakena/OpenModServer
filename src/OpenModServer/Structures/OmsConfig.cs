namespace OpenModServer.Structures;

public class OmsConfig
{
    public OmsConfigKeys Keys { get; set; }
    public OmsConfigPaths Paths { get; set; }
    public OmsConfigBranding Branding { get; set; }
    
    public OmsConfigEmailSettings Email { get; set; }
}

public class OmsConfigEmailSettings
{
    public string Provider { get; set; }
    public string FromName { get; set; }
    public string FromAddress { get; set; }
}

public class OmsConfigKeys
{
    public string VirusTotal { get; set; }
    public string? SendGrid { get; set; }
}

public class OmsConfigPaths
{
    public string Uploads { get; set; }
    public string ReleaseScreenshots { get; set; }
    public string UserAccountPictures { get; set; }
}

public class OmsConfigBranding
{
    public string? Name { get; set; }
}

public class OmsConfigExternalAuthentication
{
    public OmsConfigExternalAuthenticationKeyPair Discord { get; set; }
}

public class OmsConfigExternalAuthenticationKeyPair
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}