namespace Users.Identity;

public class ServiceSettings
{
    public string IdentityUrl { get; set; }
    public string Consul { get; set; }
    public IdentitySettings IdentitySettings { get; set; }

}
