using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Extension
{
    public static class ResourceExtension
    {
        public static ResourceReference GetResourceReference(this Resource resource, string rootUrl = null)
        {
            var resourceUrl = $"{resource.TypeName}/{resource.Id}";

            if (!string.IsNullOrEmpty(rootUrl))
            {
                if (resourceUrl.EndsWith("/"))
                    resourceUrl = rootUrl + resourceUrl;
                else
                    resourceUrl = rootUrl + "/" + resourceUrl;
            }

            return new ResourceReference(resourceUrl);
        }
    }
}