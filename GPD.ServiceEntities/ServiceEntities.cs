using System.Runtime.Serialization;
using System.Collections.Generic;

namespace GPD.ServiceEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "project")]
    public class ProjectDTO
    {
        public ProjectDTO()
        {
            Id = System.Guid.NewGuid().ToString();
        }

        [DataMember(Name = "id", Order = 1)]
        public string Id;

        [DataMember(Name = "author", Order = 1)]
        public string Author;

        [DataMember(Name = "building-name", Order = 2)]
        public string BuildingName;

        [DataMember(Name = "client", Order = 3)]
        public string Client;

        [DataMember(Name = "filename", Order = 3)]
        public string Filename;

        [DataMember(Name = "identifiers", Order = 4)]
        public List<IdentifierDTO> Identifiers { get; set; }

        [DataMember(Name = "items", Order = 5)]
        public List<ItemDTO> Items { get; set; }

        [DataMember(Name = "location", Order = 6)]
        public LocationDTO Location { get; set; }

        [DataMember(Name = "name", Order = 7)]
        public string Name;

        [DataMember(Name = "number", Order = 8)]
        public string Number;

        [DataMember(Name = "organization-description", Order = 9)]
        public string OrganizationDescription;

        [DataMember(Name = "organization-name", Order = 10)]
        public string OrganizationName;

        [DataMember(Name = "session", Order = 11)]
        public SessionDTO Session;

        [DataMember(Name = "status", Order = 12)]
        public string Status;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "add-project-response")]
    public class AddProjectResponseDTO
    {
        public AddProjectResponseDTO(bool status, string projectId)
        {
            this.Status = status;
            this.ProjectId = projectId;
        }

        [DataMember(Name = "status", Order = 1)]
        public bool Status;

        [DataMember(Name = "project-id", Order = 2)]
        public string ProjectId;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "location")]
    public class LocationDTO
    {
        [DataMember(Name = "address1", Order = 1)]
        public string Address1;

        [DataMember(Name = "city", Order = 2)]
        public string City;

        [DataMember(Name = "state", Order = 2)]
        public string State;

        [DataMember(Name = "zip", Order = 2)]
        public string Zip;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "material")]
    public class MaterialDTO
    {
        [DataMember(Name = "id", Order = 1)]
        public string Id;

        [DataMember(Name = "product", Order = 2)]
        public MaterialProductDTO Product { get; set; }

        [DataMember(Name = "type", Order = 3)]
        public MaterialTypeDTO Type { get; set; }
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "material-product")]
    public class MaterialProductDTO
    {
        [DataMember(Name = "manufacturer", Order = 1)]
        public string Manufacturer;

        [DataMember(Name = "model", Order = 2)]
        public string Model;

        [DataMember(Name = "name", Order = 3)]
        public string Name;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "material-type")]
    public class MaterialTypeDTO
    {
        [DataMember(Name = "name", Order = 1)]
        public string Name;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "application")]
    public class ApplicationDTO
    {
        [DataMember(Name = "build", Order = 1)]
        public string Build;

        [DataMember(Name = "name", Order = 2)]
        public string Name;

        [DataMember(Name = "plugin-build", Order = 3)]
        public string PluginBuild;

        [DataMember(Name = "plugin-source", Order = 4)]
        public string PluginSource;

        [DataMember(Name = "type", Order = 5)]
        public string Type;

        [DataMember(Name = "version", Order = 6)]
        public string Version;
    }

   

    [DataContract(Namespace = "http://www.gpd.com", Name = "session")]
    public class SessionDTO
    {
        [DataMember(Name = "type", Order = 1)]
        public string Type;

        [DataMember(Name = "application", Order = 2)]
        public ApplicationDTO Application;

        [DataMember(Name = "platform", Order = 3)]
        public string Platform;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "category")]
    public class CategoryDTO
    {
        [DataMember(Name = "taxonomy", Order = 1)]
        public string Taxonomy;

        [DataMember(Name = "title", Order = 2)]
        public string Title;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "identifier")]
    public class IdentifierDTO
    {
        [DataMember(Name = "identifier", Order = 1)]
        public string Identifier;

        [DataMember(Name = "system", Order = 2)]
        public string SystemName;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "item-product")]
    public class ItemProductDTO
    {
        [DataMember(Name = "id", Order = 1)]
        public string Id;

        [DataMember(Name = "manufacturer", Order = 2)]
        public string Manufacturer;

        [DataMember(Name = "model", Order = 3)]
        public string Model;

        [DataMember(Name = "name", Order = 4)]
        public string Name;

        [DataMember(Name = "url", Order = 5)]
        public string URL;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "item")]
    public class ItemDTO
    {

        [DataMember(Name = "categories", Order = 1)]
        public List<CategoryDTO> Categories { get; set; }

        [DataMember(Name = "currency", Order = 2)]
        public string Currency;

        [DataMember(Name = "family", Order = 3)]
        public string Family;

        [DataMember(Name = "id", Order = 4)]
        public string Id;

        [DataMember(Name = "materials", Order = 5)]
        public List<MaterialDTO> Identifiers { get; set; }

        [DataMember(Name = "product", Order = 6)]
        public ItemProductDTO Product { get; set; }

        [DataMember(Name = "quantity", Order = 7)]
        public string Quantity;

        [DataMember(Name = "quantity-unit", Order = 8)]
        public string QuantityUnit;

        [DataMember(Name = "type", Order = 9)]
        public string Type;
    }
}
