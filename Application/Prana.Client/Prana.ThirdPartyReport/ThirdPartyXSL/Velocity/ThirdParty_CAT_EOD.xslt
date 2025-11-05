<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>       

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>      

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

			<xsl:variable name="PB_NAME">
              <xsl:value-of select="'Clearing'"/>
            </xsl:variable>
          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <Side>
            <xsl:value-of select="Side"/>
          </Side>

          <TradeDate>
            <xsl:value-of select="'false'"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="''"/>
          </SettlementDate>

          <NewOrderFDID>
            <xsl:value-of select="''"/>
          </NewOrderFDID>

          <EventTimestamp>
            <xsl:value-of select="TradeDateTime"/>
          </EventTimestamp>

          <CancelFlag>
            <xsl:value-of select="''"/>
          </CancelFlag>

          <CancelTimestamp>
            <xsl:value-of select="''"/>
          </CancelTimestamp>

          <quantity>
            <xsl:value-of select="AllocatedQty"/>
          </quantity>

          <price>
            <xsl:value-of select="AveragePrice"/>
          </price>

          <AllocationType>
            <xsl:value-of select="'DVP'"/>
          </AllocationType>

			<xsl:variable name="PB_BROKER_NAME">
				<xsl:value-of select="CounterParty"/>
			</xsl:variable>

			<xsl:variable name="PRANA_BROKER_NAME">
			<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
			</xsl:variable>

			<xsl:variable name="varBrokerName">
			<xsl:choose>
				<xsl:when test="$PRANA_BROKER_NAME!=''">
					<xsl:value-of select="$PRANA_BROKER_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PB_BROKER_NAME"/>
				</xsl:otherwise>
			</xsl:choose>
			</xsl:variable>

          <DVPCustodianID>
            <xsl:value-of select="$varBrokerName"/>
          </DVPCustodianID>

          <CorrespondentCRD>
            <xsl:value-of select="''"/>
          </CorrespondentCRD>

          <AllocationInstructionTime>
            <xsl:value-of select="TradeDateTime"/>
          </AllocationInstructionTime>
		  
			
			 <xsl:variable name="PB_FUND_NAME" select="AccountName"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
              <xsl:variable name="varFundName">
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
			  </xsl:variable>
          <FirmDesignatedID>
            <xsl:value-of select="$varFundName"/>
          </FirmDesignatedID>

          <CustomerType>
            <xsl:value-of select="''"/>
          </CustomerType>

          <InstitutionFlag>
            <xsl:value-of select="''"/>
          </InstitutionFlag>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>


      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>