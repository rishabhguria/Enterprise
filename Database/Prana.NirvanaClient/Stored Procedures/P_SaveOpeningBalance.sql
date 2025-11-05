/**
Author: Aman Seth
Date: 04/09/2014 11:04:00
**/

/**
EXEC P_SaveOpeningBalance '<DocumentElement>
  <xs:schema id="DocumentElement" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xs:element name="DocumentElement" msdata:IsDataSet="true" msdata:MainDataTable="PositionMaster" msdata:UseCurrentLocale="true">
      <xs:complexType>
        <xs:choice minOccurs="0" maxOccurs="unbounded">
          <xs:element name="PositionMaster">
            <xs:complexType>
              <xs:sequence>
                <xs:element name="SubAccountID" type="xs:string" minOccurs="0" />
                <xs:element name="Amount" type="xs:string" minOccurs="0" />
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xs:choice>
      </xs:complexType>
    </xs:element>
  </xs:schema>
  <PositionMaster>
    <SubAccountID>1</SubAccountID>
    <Amount>1</Amount>
  </PositionMaster>
  <PositionMaster>
    <SubAccountID>2</SubAccountID>
    <Amount>2</Amount>
  </PositionMaster>
  <PositionMaster>
    <SubAccountID>17</SubAccountID>
    <Amount>18</Amount>
  </PositionMaster>
</DocumentElement>'
**/

Create Procedure [dbo].[P_SaveOpeningBalance]                                                                                                                             
(                                                                                                                                                                                                  
@xml  nText                                                                                                                                                                                                  
)         
as
begin   
DECLARE @handle int
DECLARE @PositionMaster table
(
SubAccountID varchar(10) 
,Amount     varchar(10)
)

EXEC sp_xml_preparedocument @handle OUTPUT, @xml;
insert into @PositionMaster
select *  
    FROM OPENXML (@handle, '/DocumentElement/PositionMaster')
    WITH 
	(
	 SubAccountID  varchar(10) 'SubAccountID'
	,Amount     varchar(10)         'Amount'      
    )  
select *FROM @PositionMaster

EXEC sp_xml_removedocument @handle

end
