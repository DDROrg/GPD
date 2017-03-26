
CREATE PROCEDURE [dbo].[uspAddProject] (@P_XML  XML) 
AS 
  BEGIN 
      --DECLARE @P_XML XML;
	  SET @P_XML  = 
'<project>
	<author>James Jackson</author>
	<building-name></building-name>
	<client>North Development Group</client>
	<filename>Roswell Math and Science - 2016.rvt</filename>
	<identifiers>
		<identifier>7cacd49c-ac17-4591-ad0a-cbc9bb40015a-00012b83</identifier>
		<system>REVIT</system>
	</identifiers>	
	<items>
		<categories>
			<taxonomy>PROJECT_ITEM</taxonomy>
			<title>Specialty Equipment</title>
		</categories>
		<family>M_Cook Top-4 Unit2</family>
		<id>676647</id>
		<materials>
			<area>0.741533355256686</area>
			<id>277285</id>
			<product>
				<manufacturer></manufacturer>
				<model></model>
				<name>Metal - Chrome</name>
			</product>
			<type>
				<name>Material-Unassigned</name>
			</type>
			<volume>0.007271317112180178</volume>
		</materials>
		<materials>
			<area>8.677423060506523</area>
			<id>277291</id>
			<product>
				<manufacturer></manufacturer>
				<model></model>
				<name>Metal - Steel, Polished</name>
			</product>
			<type>
				<name>Material-Unassigned</name>
			</type>
			<volume>0.2237009555219917</volume>
		</materials>
		<product>
			<id>676647</id>
			<name>Specialty Equipment - 0615 x 500mm</name>
		</product>
		<quantity>1</quantity>
		<quantity-unit>EA</quantity-unit>
		<type>0615 x 500mm</type>
	</items>
	<items>
		<categories>
			<taxonomy>UNIFORMAT</taxonomy>
			<title>E2020200</title>
		</categories>
		<categories>
			<taxonomy>PROJECT_ITEM</taxonomy>
			<title>Furniture</title>
		</categories>
		<family>Bar-Curved-8Ft_Reed</family>
		<id>676623</id>
		<materials>
			<area>61.758738413865615</area>
			<id>367904</id>
			<product>
				<manufacturer></manufacturer>
				<model></model>
				<name>granite</name>
			</product>
			<type>
				<name>Material-Unassigned</name>
			</type>
			<volume>7.456345177279178</volume>
		</materials>
		<materials>
			<area>0.8248957086761317</area>
			<id>676618</id>
			<product>
				<manufacturer></manufacturer>
				<model></model>
				<name>Rubber</name>
			</product>
			<type>
				<name>Material-Unassigned</name>
			</type>
			<volume>0.012556359829468594</volume>
		</materials>
		<materials>
			<area>100.87984032522036</area>
			<id>676619</id>
			<product>
				<manufacturer></manufacturer>
				<model></model>
				<name>Wood, Mahogany</name>
			</product>
			<type>
				<name>Material-Wood</name>
			</type>
			<volume>3.985831879324426</volume>
		</materials>
		<product>
			<id>676623</id>
			<manufacturer></manufacturer>
			<name>Furniture - Bar-Curved-8Ft_Reed</name>
		</product>
		<quantity>1</quantity>
		<quantity-unit>EA</quantity-unit>
		<type>Bar-Curved-8Ft_Reed</type>
	</items>	
	<location>
		<address1>820 Ebenezer</address1>
		<city>Rd</city>
		<state></state>
		<zip></zip>
	</location>
	<name>Roswell Math and Science Charter School</name>
	<number>3222121</number>
	<organization-description></organization-description>
	<organization-name>Global Product Data, LLC.</organization-name>
	<session>
		<type>ApplicationSession</type>
		<application>
			<build>20150714_1515(x64)</build>
			<name>Autodesk Revit 2016</name>
			<plugin-build>2.3.0.383</plugin-build>
			<plugin-source>SAVEPOSTDATA</plugin-source>
			<type>REVIT</type>
			<version>2016</version>
		</application>
		<platform>windows</platform>
	</session>
	<status>Schematic Design - Demo Project</status>
</project>';

Select 
M.value('(categories/taxonomy)[1]', 'NVARCHAR(200)')    AS taxonomy,
M.value('(categories/title)[1]', 'NVARCHAR(200)')    AS title,
M.value('(id)[1]', 'INT')    AS id,
M.value('(family)[1]', 'NVARCHAR(200)')    AS title,
M.value('(quantity)[1]', 'INT')    AS quantity
from @P_XML.nodes('/project/items') M(M); 

  END