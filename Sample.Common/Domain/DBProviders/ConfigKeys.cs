using System.ComponentModel.DataAnnotations;

namespace Sample.Common.Domain.DBProviders
{
    public enum ConfigKeys
    {
        [Display(Name = "Services.Hosts")]
        ServicesHosts,

        [Display(Name = "Services.ApiFile")]
        ServicesApiFile
    }
}
