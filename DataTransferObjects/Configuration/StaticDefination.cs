namespace DataTransferObjets.Configuration
{
    public static class StaticDefination
    {
        public const string NameOrigins = "misOrigenes";
        public const string DatabaseConnection = "DatabaseConnection";
        public const string AzureActiveDirectory = "AzureAdB2C";
        public const string ConfiguracionApp = "ConfiguracionApp";
        public const string NombreOrigenes = "misOrigenes";

        //Generic Message
        public const string Separator = " ";
        public const string NotImplemented = "The service is not yet implemented.";
        public const string NoData = "No records were found.";
        public const string DataNotFound = "No records were found with the provided data.";
        public const string IsNotGreaterThanZero = "The value entered is not greater than zero.";
        public const string DuplicateRegistration = "Duplicate registration. (Property Name is already in system)";

        //Default
        public const string IdDefaultOwner = "fba5e3d5-21cb-4f23-b4cb-e66708f8e89d";
        public const string NameDefaultOwner = "Mr. Afghanistan";
        public const string AddressDefaultOwner = "Mr. Afghanistan";

        // Table Relations
        public const string PropertyRelations = "Owner";
        public const string PropertyImageRelations = "Property";

        //HTTP Message
        public const string NotFoundMessage = "The requested resource is not found";
        public const string NotContentMessage = "There is no content to send for this request";
        public const string ConflictMessage = "A resource already exists that conflicts with the one you are trying to create.";
        public const string WebDAVMessage = "Some images saved successfully, but there were errors.";

    }
}
