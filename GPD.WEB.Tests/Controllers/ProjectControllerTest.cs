using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GPD.WEB.Tests.Controllers
{
    using WEB;
    using WEB.Controllers;
    using ServiceEntities;

    [TestClass]
    public class ProjectControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            ProjectController controller = new ProjectController();

            // Act
            IEnumerable<ProjectDTO> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
        }        
    }
}
