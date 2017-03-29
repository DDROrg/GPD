using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Linq;

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
            projectDTO.ProjectId = Guid.NewGuid().ToString();

            try
            {
                XDocument xmlDoc = new XDocument();

                using (var writer = xmlDoc.CreateWriter())
                {
                    //var serializer = new DataContractSerializer(projectDTO.GetType());
                    //serializer.WriteObject(writer, projectDTO);

                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(projectDTO.GetType());
                    x.Serialize(writer, projectDTO);
                }

                // Remove attribute from root node
                xmlDoc.Root.Attributes().Remove();

                int projectId = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddProject(xmlDoc);
                retVal = new AddProjectResponseDTO(true, projectId);
            }
            catch (Exception ex)
            {
                log.Error("Unable to add project", ex);
                retVal = new AddProjectResponseDTO(false, 0);
            }            
            return retVal;
        }
    }
}
