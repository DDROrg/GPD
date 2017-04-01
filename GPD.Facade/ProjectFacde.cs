using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;


namespace GPD.Facade
{
    using ServiceEntities;
    using DAL.SqlDB;
    using System.Xml;

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
    }
}
