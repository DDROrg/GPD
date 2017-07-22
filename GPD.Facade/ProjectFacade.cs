using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GPD.Facade
{
    using DAL.SqlDB;
    using ServiceEntities.BaseEntities;
    using ServiceEntities.ResponseEntities.AddProject;
    using ServiceEntities.ResponseEntities.ProjectsList;

    public class ProjectFacade : BaseFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProjectFacade() : base() { }

        public AddProjectResponse AddProject(string partnerName, ProjectDTO projectDTO)
        {
            AddProjectResponse responseDTO = new AddProjectResponse();
            XDocument xDoc = new XDocument();

            try
            {
                // set project ID
                string projectId = System.Guid.NewGuid().ToString();

                // get XML based on ProjectDTO object
                using (var writer = xDoc.CreateWriter())
                {
                    var serializer = new DataContractSerializer(projectDTO.GetType());
                    serializer.WriteObject(writer, projectDTO);
                }

                xDoc.Root.XPathSelectElements("//*[local-name()='items']/*[local-name()='item']")
                    .ToList()
                    .ForEach(T => T.Add(new XAttribute("guid", System.Guid.NewGuid().ToString())));

                Dictionary<string, string> categoriesList =
                    xDoc.Root.XPathSelectElements("//*[local-name()='items']/*[local-name()='item']/*[local-name()='categories']/*[local-name()='category']")
                    .ToList()
                    .GroupBy(g => g.XPathSelectElement("*[local-name()='taxonomy']").Value + "::" + g.XPathSelectElement("*[local-name()='title']").Value)
                    .ToDictionary(g => g.Key, g => System.Guid.NewGuid().ToString());

                xDoc.Root.XPathSelectElements("//*[local-name()='items']/*[local-name()='item']/*[local-name()='categories']/*[local-name()='category']")
                    .ToList()
                    .ForEach(T => T.Add(new XAttribute("guid", categoriesList[
                        T.XPathSelectElement("*[local-name()='taxonomy']").Value + "::" + T.XPathSelectElement("*[local-name()='title']").Value
                    ])));

                // send the project to DB 
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddProject(partnerName, projectId, xDoc);

                // project content inserted successful
                responseDTO = new AddProjectResponse(true, projectId);
            }
            catch (Exception ex)
            {
                log.Error("Unable to add project", ex);
                responseDTO = new AddProjectResponse();
            }

            return responseDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectDTO GetProjectById(string partnerName, string projectId)
        {
            ProjectDTO retVal = new ProjectDTO();

            try
            {
                // gete project data
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectById(projectId);

                if (ds != null && ds.Tables.Count > 4 && ds.Tables[0].Rows.Count > 0)
                {
                    //=====================================
                    #region Project
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        retVal.Author = dr["AUTHOR"].ToString();
                        retVal.BuildingName = dr["BUILDING_NAME"].ToString();
                        retVal.Client = dr["CLIENT"].ToString();
                        retVal.Filename = dr["FILENAME"].ToString();
                        retVal.Name = dr["NAME"].ToString();
                        retVal.Number = dr["NUMBER"].ToString();
                        retVal.OrganizationDescription = dr["ORGANIZATION_DESCRIPTION"].ToString();
                        retVal.OrganizationName = dr["ORGANIZATION_NAME"].ToString();
                        retVal.Status = dr["STATUS"].ToString();

                        retVal.Location = new LocationDTO();
                        retVal.Location.Address1 = dr["ADDRESS_LINE_1"].ToString();
                        retVal.Location.City = dr["CITY"].ToString();
                        retVal.Location.State = dr["STATE"].ToString();
                        retVal.Location.Zip = dr["ZIP"].ToString();

                        retVal.Session = new SessionDTO();
                        retVal.Session.Type = dr["TYPE"].ToString();
                        retVal.Session.Platform = dr["PLATFORM"].ToString();
                        retVal.Session.Application = new ApplicationDTO();
                        retVal.Session.Application.Build = dr["APPLICATION_BUILD"].ToString();
                        retVal.Session.Application.Name = dr["APPLICATION_NAME"].ToString();
                        retVal.Session.Application.PluginBuild = dr["APPLICATION_PLUGIN_BUILD"].ToString();
                        retVal.Session.Application.PluginSource = dr["APPLICATION_PLUGIN_SOURCE"].ToString();
                        retVal.Session.Application.Version = dr["APPLICATION_VERSION"].ToString();
                        retVal.Session.Application.Type = dr["APPLICATION_TYPE"].ToString();
                    }
                    #endregion

                    //=====================================
                    #region Identifier
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        retVal.Identifiers = new List<IdentifierDTO>();
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            IdentifierDTO tempIdentifierDTO = new IdentifierDTO()
                            {
                                Identifier = dr["IDENTIFIER"].ToString(),
                                SystemName = dr["SYSTEM"].ToString()
                            };
                            retVal.Identifiers.Add(tempIdentifierDTO);
                        }
                    }
                    #endregion

                    //=====================================
                    #region Item
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        retVal.Items = new List<ItemDTO>();
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            #region Item 
                            ItemDTO itemDTO = new ItemDTO()
                            {
                                Id = dr["ITEM_ID"].ToString(),
                                Type = dr["TYPE"].ToString(),
                                Currency = dr["CURRENCY"].ToString(),
                                Family = dr["FAMILY"].ToString(),
                                Quantity = dr["QUANTITY"].ToString(),
                                QuantityUnit = dr["QUANTITY_UNIT"].ToString()
                            };
                            #endregion

                            #region Item-Product
                            itemDTO.Product = new ItemProductDTO()
                            {
                                Id = dr["PRODUCT_ID"].ToString(),
                                URL = dr["PRODUCT_URL"].ToString(),
                                Manufacturer = dr["PRODUCT_MANUFACTURER"].ToString(),
                                Model = dr["PRODUCT_MODEL"].ToString(),
                                Name = dr["PRODUCT_NAME"].ToString(),
                                ImageUrl = dr["product_image_url"].ToString()

                            };
                            #endregion

                            #region Item-Meterials
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                var drMaterials = ds.Tables[3].AsEnumerable()
                                    .Where(i => i["ITEM_ID"].ToString().Equals(itemDTO.Id));

                                if (drMaterials.Count() > 0)
                                {
                                    itemDTO.Materials = new List<MaterialDTO>();
                                    foreach (DataRow drM in drMaterials)
                                    {
                                        MaterialDTO tempMaterialDTO = new MaterialDTO() { Id = drM["MATERIAL_ID"].ToString() };
                                        tempMaterialDTO.Type = new MaterialTypeDTO() { Name = drM["TYPE_NAME"].ToString() };
                                        tempMaterialDTO.Product = new MaterialProductDTO()
                                        {
                                            Name = drM["TYPE_NAME"].ToString(),
                                            Manufacturer = drM["PRODUCT_MANUFACTURER"].ToString(),
                                            Model = drM["PRODUCT_MODEL"].ToString()
                                        };

                                        itemDTO.Materials.Add(tempMaterialDTO);
                                    }
                                }
                            }
                            #endregion

                            #region Item-Category
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                var drCategories = ds.Tables[4].AsEnumerable()
                                   .Where(i => i["ITEM_ID"].ToString().Equals(itemDTO.Id));

                                if (drCategories.Count() > 0)
                                {
                                    itemDTO.Categories = new List<CategoryDTO>();

                                    foreach (DataRow drC in drCategories)
                                    {
                                        CategoryDTO tempCategoryDTO = new CategoryDTO()
                                        {
                                            Taxonomy = drC["TAXONOMY"].ToString(),
                                            Title = drC["TITLE"].ToString()
                                        };

                                        itemDTO.Categories.Add(tempCategoryDTO);
                                    }
                                }
                            }
                            #endregion

                            retVal.Items.Add(itemDTO);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get project by id: " + projectId, ex);
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ProjectsListResponse GetProjectsList(string partnerName, int pageSize, int pageIndex, string searchTerm = null, string projectNumber = null)
        {
            ProjectsListResponse retVal = new ProjectsListResponse() {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecordCount = 0
            };

            try
            {
                // get projects dataset from database
                DataSet ds = (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(projectNumber)) ?
                    new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectsList(partnerName, pageSize, pageIndex)
                    :
                    new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectsListWithSearchTerm(partnerName, searchTerm, projectNumber, pageSize, pageIndex);
                
                if (ds != null && ds.Tables.Count == 2 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ProjectItem projectDTO = new ProjectItem(dr["PROJECT_ID"].ToString())
                        {
                            Author = dr["AUTHOR"].ToString(),
                            BuildingName = dr["BUILDING_NAME"].ToString(),
                            Client = dr["CLIENT"].ToString(),
                            Filename = dr["FILENAME"].ToString(),
                            Name = dr["NAME"].ToString(),
                            Number = dr["NUMBER"].ToString(),
                            OrganizationDescription = dr["ORGANIZATION_DESCRIPTION"].ToString(),
                            OrganizationName = dr["ORGANIZATION_NAME"].ToString(),
                            Status = dr["STATUS"].ToString(),
                            CreateTimestamp = ((DateTime)dr["CREATE_DATE"]).ToString("o"),
                            Location = new LocationItem()
                            {
                                Address1 = dr["ADDRESS_LINE_1"].ToString(),
                                City = dr["CITY"].ToString(),
                                State = dr["STATE"].ToString(),
                                ZipCode = dr["ZIP"].ToString()
                            }
                        };

                        retVal.ProjectList.Add(projectDTO);
                    }

                    {
                        DataRow dr = ds.Tables[1].Rows[0];
                        retVal.TotalRecordCount = Convert.ToInt32(dr["TotalCount"].ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get all projects. ERROR: " + exc.ToString());
            }

            return retVal;
        }
    }
}