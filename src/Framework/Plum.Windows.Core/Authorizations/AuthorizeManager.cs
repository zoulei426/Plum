using System;
using System.Collections.Generic;

namespace Plum.Windows.Authorizations
{
    public class AuthorizeManager
    {
        private readonly IPlumApi api;
        private Dictionary<string, bool> permissions;

        public static AuthorizeManager Instance { get; private set; }

        public static void Initialize(IPlumApi api)
        {
            Instance = new AuthorizeManager(api);
        }

        private AuthorizeManager(IPlumApi api)
        {
            this.api = api;
            permissions = new Dictionary<string, bool>();
        }

        public bool IsGranted(string policyName)
        {
            lock (permissions)
            {
                if (permissions.ContainsKey(policyName))
                    return permissions[policyName];

                var result = false;

                try
                {
                    result = api.IsGranted(policyName).Result;
                    permissions.Add(policyName, result);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex.ToDetailString());
                }

                return result;
            }
        }
    }
}