﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Extension
{
    public static class BundleExtension
    {
        public static T FirstOrDefault<T>(this Bundle bundle, Func<T, bool> compareFunc) where T: Resource
        {
            foreach (var entry in bundle.Entry)
            {
                var resource = entry.Resource as T;

                if (resource == null) continue;

                if (compareFunc == null)
                    return resource;

                if (compareFunc(resource))
                    return resource;
            }

            return null;
        }

        public static Bundle.EntryComponent AddResourceEntry(this Bundle bundle, Resource resource)
        {
            if (bundle == null || resource == null)
                return null;

            return bundle.AddResourceEntry(resource, resource.GetResourceReference().Reference);
        }

    }
}
