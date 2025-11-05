<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[MasterFund != 'Alpha Sehoy LLC' and MasterFund != 'City of Sarasota Firefighters Pension Plan' and MasterFund != 'Police Officers Pension Fund City of St Petersburg']">

        <ThirdPartyFlatFileDetail>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME" select="'EZE'"/>

          <xsl:variable name = "PRANA_MASTERFUND_NAME">
            <xsl:value-of select="MasterFund"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_MASTERFUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/MasterFUndMapping.xml')/MasterFundMapping/PB[@Name=$PB_NAME]/MasterFundData[@PranaMasterFund=$PRANA_MASTERFUND_NAME]/@PBMasterFundCode"/>
          </xsl:variable>

          <MasterFund>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_MASTERFUND_CODE != ''">
                <xsl:value-of select="$THIRDPARTY_MASTERFUND_CODE"/>
              </xsl:when>             
              <xsl:otherwise>
                <xsl:value-of select="MasterFund"/>
              </xsl:otherwise>
            </xsl:choose>
          </MasterFund>

          <NAV>
            <xsl:value-of select="format-number(NAV,'0.##')"/>
          </NAV>
          
          <Date>
            <xsl:value-of select="Date"/>
          </Date>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>