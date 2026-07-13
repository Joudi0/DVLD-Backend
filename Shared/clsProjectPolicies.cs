using System;

namespace Shared
{
    public static class clsProjectPolicies
    {
        // Authorization Policies
        public const string UserOwnerOrAdmin = "UserOwnerOrAdmin";

        // Rate Limiting Policies
        public const string AuthPolicy = "AuthPolicy";
        public const string WritePolicy = "WritePolicy";
        public const string ReadPolicy = "ReadPolicy";
    }
}