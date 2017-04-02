using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Data;
using System.Xml.Linq;
using System.Xml.XPath;


namespace GPD.Facade
{
    using ServiceEntities;
    using DAL.SqlDB;

    public class ProjectFacde
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectFacde));

        public AddProjectResponseDTO Add(ProjectDTO projectDTO)
        {
            AddProjectResponseDTO retVal;
            try
            {
                XDocument doc = new XDocument();

                using (var writer = doc.CreateWriter())
                {
                    var serializer = new DataContractSerializer(projectDTO.GetType());
                    serializer.WriteObject(writer, projectDTO);
                }

                //XmlNameTable nameTable = doc.CreateReader().NameTable;
                //XmlNamespaceManager names = new XmlNamespaceManager(nameTable);

                XmlNamespaceManager nameSpace = new XmlNamespaceManager(new NameTable());
                XNamespace def = "http://www.gpd.com";
                nameSpace.AddNamespace("def", "http://www.gpd.com");
                nameSpace.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");

                doc.Root.XPathSelectElements("def:items/def:item", nameSpace)
                    .ToList()
                    .ForEach(i =>
                    {
                        i.Add(new XElement(def + "project_item_id", System.Guid.NewGuid().ToString()));
                    });

                //string projectId = doc.Root.XPathSelectElement("def:id", nameSpace).Value;
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddProject(doc);
                retVal = new AddProjectResponseDTO(true, projectDTO.Id);
            }
            catch (Exception ex)
            {
                log.Error("Unable to add project", ex);
                retVal = new AddProjectResponseDTO(false, "");
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ProjectDTO> GetAll(string userId)
        {
            List<ProjectDTO> retVal = new List<ProjectDTO>();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjects(userId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ProjectDTO tempProjectDTO = new ProjectDTO()
                        {
                            Id = dr["PROJECT_ID"].ToString(),
                            Author = dr["AUTHOR"].ToString(),
                            BuildingName = dr["BUILDING_NAME"].ToString(),
                            Client = dr["CLIENT"].ToString(),
                            Filename = dr["FILENAME"].ToString(),
                            Name = dr["NAME"].ToString(),
                            Number = dr["NUMBER"].ToString(),
                            OrganizationDescription = dr["ORGANIZATION_DESCRIPTION"].ToString(),
                            OrganizationName = dr["ORGANIZATION_NAME"].ToString(),
                            Status = dr["STATUS"].ToString()
                        };
                        retVal.Add(tempProjectDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get projects", ex);
            }
            return retVal;
        }
    }
}
