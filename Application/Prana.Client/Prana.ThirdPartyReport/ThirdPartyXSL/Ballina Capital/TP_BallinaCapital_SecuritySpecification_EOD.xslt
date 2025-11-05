<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
	
    <ThirdPartyFlatFileDetailCollection>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
          <xsl:variable name="varSecurity">
            <xsl:choose>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select ="SEDOL"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select ="CUSIP"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <SecurityID>
            <xsl:value-of select ="$varSecurity"/>
          </SecurityID>
          <xsl:variable name="varSecurityIDType">
            <xsl:choose>
              <xsl:when test="SEDOL!=''">
                <xsl:value-of select="'SEDOL'"/>
              </xsl:when>
              <xsl:when test="CUSIP!=''">
                <xsl:value-of select="'CUSIP'"/>
              </xsl:when>
           
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <SecurityType>
            <xsl:value-of select ="$varSecurityIDType"/>
          </SecurityType>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>