namespace Ironhide.Web.Api.Infrastructure.Authentication
{
    public interface IMenuProvider
    {
        string[] getFeatures(string claim);
        string[] getFeatures(string[] claims);
        string[] getAllFeatures();
    }


}