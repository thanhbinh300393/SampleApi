namespace Sample.Common
{
    public static class AppConstants
    {
        public const string KEY_CONFIG_HOST = "Srp:Host";
        public const string KEY_CONFIG_MODULE_ID = "Srp:ModuleId";
        public const string KEY_CONFIG_URL_FILE = "Srp:File";
        public const string KEY_CONFIG_XTRAREPORT_CACHE_PATH = "Xtrareport:CachePath";

        public const string URL_REPORT_FILE = "/file/reports";
        public const string API_GET_REPORT_FILE = "/api/reports";

        public const string HOST_FILE_FILE_TYPE_REPORT = "ELibraryReport";
        public const string HOST_FILE_FILE_URL_FILE = "file/ELibraryReport";

        public const string HOST_FILE_EQE_ENCRYPTION_PROOF_FILE = "EncryptionProofFile";
        public const string HOST_FILE_EQE_REPORT = "EqeReport";
        public const string HOST_FILE_EQE_REPORT_CACHE = "GenerateFiles";
        public const string HOST_FILE_EQE_REPORTS_FOLDER = "EqeReports";

        // viettel
        public const string SSO_BASE_URL = "SSO:BaseUrl";
        public const string SSO_SERVER = "SSO:SSOServer";
        public const string SSO_CLIENT_ID = "SSO:SSOClientID";
        public const string SSO_CLIENT_SECRET = "SSO:SSOClientSecret";

        public const string ACCOUNT_CALL_API = "SSO:accountCallApi_v2";
        public const string PASSWORD_CALL_API = "SSO:passwordCallApi_v2";
        public const string SSO_API_CLIENT_ID = "SSO:ssoApiClientId";
        public const string SSO_APIGRANT_TYPE = "SSO:ssoApiGrantType";
        public const string SSO_API_SCOPE = "SSO:ssoApiScope";
        public const string SSO_SERVER_SYNC = "SSO:ssoServerSync";

        public const string SSO_VIETTEL_SERVER = "viettelLinkServer:apiSyncLinkServer";
        public const string SSO_VIETTEL_CLIENTID = "viettelLinkServer:clientId";
        public const string SSO_VIETTEL_CLIENTSECRET = "viettelLinkServer:clientSecret";
        public const int SSO_PULPIL = 101;
        public const int SSO_CADRE = 16;

        public const string PRODUCTIONENV_ENVNAME = "productionEnv:envName";
    }
}