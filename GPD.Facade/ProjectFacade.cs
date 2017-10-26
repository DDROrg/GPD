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
    using ServiceEntities.ResponseEntities;
    using ServiceEntities.ResponseEntities.ProjectsList;

    public class ProjectFacade : BaseFacade
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProjectFacade() : base() { }

        public AddProjectResponse AddProject(string partnerName, int userId, ProjectDTO projectDTO)
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
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).AddProject(partnerName, userId, projectId, xDoc);

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
        public ProjectDTO GetProjectById(string projectId)
        {
            ProjectDTO retVal = new ProjectDTO();
            string projectItemId = "";

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

                        retVal.Location = new LocationDTO()
                        {
                            Country = dr["COUNTRY"].ToString(),
                            AddressLine1 = dr["ADDRESS_LINE_1"].ToString(),
                            AddressLine2 = dr["ADDRESS_LINE_2"].ToString(),
                            City = dr["CITY"].ToString(),
                            State = dr["STATE"].ToString(),
                            PostalCode = dr["ZIP"].ToString()
                        };

                        retVal.Session = new SessionDTO()
                        {
                            Type = dr["TYPE"].ToString(),
                            Platform = dr["PLATFORM"].ToString(),
                            Application = new ApplicationDTO()
                            {
                                Build = dr["APPLICATION_BUILD"].ToString(),
                                Name = dr["APPLICATION_NAME"].ToString(),
                                PluginBuild = dr["APPLICATION_PLUGIN_BUILD"].ToString(),
                                PluginSource = dr["APPLICATION_PLUGIN_SOURCE"].ToString(),
                                PluginName = dr["APPLICATION_PLUGIN_NAME"].ToString(),
                                Version = dr["APPLICATION_VERSION"].ToString(),
                                Type = dr["APPLICATION_TYPE"].ToString(),
                                ClientIP = dr["APPLICATION_CLIENT_IP"].ToString(),
                            }
                        };
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
                            // get project-item-id
                            projectItemId = dr["project_item_id"].ToString();

                            #region Item 
                            ItemDTO itemDTO = new ItemDTO()
                            {
                                Id = int.Parse(dr["ITEM_ID"].ToString()),
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

                            #region Item-Materials
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                var drMaterials = ds.Tables[3].AsEnumerable()
                                    .Where(i => i["project_item_id"].ToString().Equals(projectItemId));

                                if (drMaterials.Count() > 0)
                                {
                                    itemDTO.Materials = new List<MaterialDTO>();

                                    foreach (DataRow dataRow in drMaterials)
                                    {
                                        itemDTO.Materials.Add(
                                            new MaterialDTO()
                                            {
                                                Id = dataRow["MATERIAL_ID"].ToString(),
                                                Type = new MaterialTypeDTO()
                                                {
                                                    Name = dataRow["TYPE_NAME"].ToString(),
                                                },
                                                Product = new MaterialProductDTO()
                                                {
                                                    Name = dataRow["PRODUCT_NAME"].ToString(),
                                                    Manufacturer = dataRow["PRODUCT_MANUFACTURER"].ToString(),
                                                    Model = dataRow["PRODUCT_MODEL"].ToString()
                                                }
                                            });
                                    }
                                }
                            }
                            #endregion Item-Materials

                            #region Item-Categories
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                var drCategories = ds.Tables[4].AsEnumerable()
                                   .Where(i => i["PROJECT_ITEM_ID"].ToString().Equals(projectItemId));

                                if (drCategories.Count() > 0)
                                {
                                    itemDTO.Categories = new List<CategoryDTO>();

                                    foreach (DataRow drC in drCategories)
                                    {
                                        itemDTO.Categories.Add(
                                            new CategoryDTO()
                                            {
                                                Taxonomy = drC["TAXONOMY"].ToString(),
                                                Title = drC["TITLE"].ToString()
                                            });
                                    }
                                }
                            }
                            #endregion Item-Categories

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
        /// <param name="partnerName"></param>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchTerm"></param>
        /// <param name="pIdentifier"></param>
        /// <returns></returns>
        public ProjectsListResponse GetProjectsList(string partnerName, int userId, int pageSize, int pageIndex,
            string fromDate, string toDate, string searchTerm = null, string projectIdentifier = null)
        {
            ProjectsListResponse retVal = new ProjectsListResponse()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecordCount = 0
            };

            try
            {
                // get projects dataset from database
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection)
                    .GetProjectsList(partnerName, userId, fromDate, toDate, searchTerm, projectIdentifier, pageSize, pageIndex);

                //DataSet ds = (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(pIdentifier)) ?
                //    new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectsList(partnerName, pageSize, pageIndex, fromDate, toDate)
                //    :
                //    new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectsListWithSearchTerm(partnerName, searchTerm, pIdentifier, fromDate, toDate, pageSize, pageIndex);


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
                            Active = dr["ACTIVE"].ToString(),
                            DeleteStatus = dr["DELETED"].ToString(),
                            CreateTimestamp = ((DateTime)dr["CREATE_DATE"]).ToString("o"),
                            UpdateTimestamp = DBNull.Value.Equals(dr["UPDATE_DATE"]) ? "" : ((DateTime)dr["UPDATE_DATE"]).ToString("o"),
                            PartnerName = dr["PARTNER_NAME"].ToString(),
                            UserEmail = dr["EMAIL"].ToString(),
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ChartDTO GetProjectChartData(string partner, string fromDate, string toDate)
        {
            //dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            ChartDTO retVal = new ChartDTO();

            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectChartData(partner, fromDate, toDate);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    string tempAppType = "";
                    LinesDTO line = new LinesDTO();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string appType = dr["APP_TYPE"].ToString();
                        if (appType.ToUpper() != tempAppType)
                        {
                            tempAppType = appType.ToUpper();
                            line = new LinesDTO();
                            line.Name = appType;
                            retVal.Lines.Add(line);
                        }
                        line.Dates.Add(((DateTime)dr["C_DATE"]).ToString("yyyy-MM-dd"));
                        line.Values.Add(Convert.ToInt32(dr["P_COUNT"].ToString()));
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get data for project-chart. ERROR: " + exc.ToString());
            }
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public ChartDTO GetTopProductChartData(string partner)
        {
            ChartDTO retVal = new ChartDTO();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetTopProductChartData(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        retVal.Lines.Add(new LinesDTO()
                        {
                            Name = dr["P_NAME"].ToString(),
                            Dates = new List<string>(new string[] { "" }),
                            Values = new List<int>(new int[] { Convert.ToInt32(dr["P_COUNT"].ToString()) })
                        });
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get data for top product. ERROR: " + exc.ToString());
            }
            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ChartDTO GetAppChartData(string partner)
        {
            ChartDTO retVal = new ChartDTO();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetAppChartData(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        retVal.Lines.Add(new LinesDTO()
                        {
                            Name = dr["A_NAME"].ToString(),
                            Dates = new List<string>(new string[] { "" }),
                            Values = new List<int>(new int[] { Convert.ToInt32(dr["P_COUNT"].ToString()) })
                        });
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get data for top application. ERROR: " + exc.ToString());
            }
            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public ChartDTO GetTopCustomerChartData(string partner)
        {
            ChartDTO retVal = new ChartDTO();
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetTopCustomerChartData(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        retVal.Lines.Add(new LinesDTO()
                        {
                            Name = dr["M_NAME"].ToString(),
                            Dates = new List<string>(new string[] { "" }),
                            Values = new List<int>(new int[] { Convert.ToInt32(dr["P_COUNT"].ToString()) })
                        });
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get data for top application. ERROR: " + exc.ToString());
            }
            //retVal.Lines.Add(new LinesDTO()
            //{
            //    Name = "Delta",
            //    Values = new List<int>(new int[] { 560 })
            //});           
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public int GetProjectCount(string partner)
        {
            int retVal = 0;
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetProjectCount(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt32(ds.Tables[0].Rows[0]["P_COUNT"].ToString());
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get project count. ERROR: " + exc.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public int GetUniqueUserCount(string partner)
        {
            int retVal = 0;
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetUniqueUserCount(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt32(ds.Tables[0].Rows[0]["U_COUNT"].ToString());
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get user count. ERROR: " + exc.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPartnerCount()
        {
            int retVal = 0;
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetPartnerCount();
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt32(ds.Tables[0].Rows[0]["P_COUNT"].ToString());
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get partner count. ERROR: " + exc.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public int GetBPMCount(string partner)
        {
            int retVal = 0;
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetBPMCount(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt32(ds.Tables[0].Rows[0]["U_COUNT"].ToString());
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get BPM count. ERROR: " + exc.ToString());
            }

            return retVal;
        }

        /// <summary>
        /// Get percentage of project with ProductTAG data
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public double GetPctProjectWithManufacturer(string partner)
        {
            double retVal = 0;
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetPctProjectWithManufacturer(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    int y = Convert.ToInt32(ds.Tables[0].Rows[0]["HAS_MFG"].ToString());
                    int n = Convert.ToInt32(ds.Tables[0].Rows[0]["NO_MFG"].ToString());
                    retVal = (double)(y * 100) / (y + n);
                    retVal = Math.Round(retVal, 2);
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get percentage of project with manufacturer data. ERROR: " + exc.ToString());
            }
            return retVal;
        }

        /// <summary>
        /// Get percentage of project with manufacturer data
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public double GetPctProjectWithProductTAG(string partner)
        {
            double retVal = 0;
            try
            {
                DataSet ds = new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).GetPctProjectWithProductTAG(partner);
                if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    int y = Convert.ToInt32(ds.Tables[0].Rows[0]["HAS_URL"].ToString());
                    int n = Convert.ToInt32(ds.Tables[0].Rows[0]["NO_URL"].ToString());
                    retVal = (double)(y * 100) / (y + n);
                    retVal = Math.Round(retVal, 2);
                }
            }
            catch (Exception exc)
            {
                log.Error("Unable to get percentage of project with manufacturer data. ERROR: " + exc.ToString());
            }
            return retVal;
        }

        public UpdateProjectResponse UpdateProject(string projectId, ProjectDTO projectDTO)
        {
            UpdateProjectResponse projectResponse = new UpdateProjectResponse(false, projectId);
            XDocument xDoc = new XDocument();

            try
            {
                // get XML based on ProjectDTO object
                using (var writer = xDoc.CreateWriter())
                {
                    var serializer = new DataContractSerializer(projectDTO.GetType());
                    serializer.WriteObject(writer, projectDTO);
                }

                // send the project to DB 
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).UpdateProject(projectId, xDoc);

                // project updated successful
                projectResponse.Status = true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to update the project", ex);
                projectResponse.Message = "Unable to update the project";
            }

            return projectResponse;
        }

        public ActivateProjectListResponse ActivateProjectList(List<string> projectList, bool isActive)
        {
            ActivateProjectListResponse responseObj = new ActivateProjectListResponse();

            try
            {
                XDocument xDoc = new XDocument(
                    new XElement("project-list",
                        projectList.Select(T => new XElement("project", T))
                    )
                );

                // call DB function
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).ActivateProjectList(xDoc, isActive);

                // project updated successful
                responseObj.Status = true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to update the project list", ex);
                responseObj.Message = "Unable to update the project list";
            }

            return responseObj;
        }

        public DeleteProjectListResponse DeleteProjectList(List<string> projectList, bool deleteFlag)
        {
            DeleteProjectListResponse responseObj = new DeleteProjectListResponse();

            try
            {
                XDocument xDoc = new XDocument(
                    new XElement("project-list",
                        projectList.Select(T => new XElement("project", T))
                    )
                );

                // call DB function
                new ProjectDB(Utility.ConfigurationHelper.GPD_Connection).DeleteProjectList(xDoc, deleteFlag);

                // project updated successful
                responseObj.Status = true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to delete the project list", ex);
                responseObj.Message = "Unable to update the project list";
            }

            return responseObj;
        }
    }
}