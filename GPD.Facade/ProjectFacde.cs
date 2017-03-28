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

                int projectId = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddProject(doc.Root);
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
