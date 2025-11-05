<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Blue Water Life Science Advisors']">
        <ThirdPartyFlatFileDetail>
          <!-- for system internal use -->
          <RowHeader>
            <xsl:value-of select="'true'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <FundName>
            <xsl:value-of select="AccountName"/>
          </FundName>

          <StartofdayAUM>
            <xsl:value-of select ="format-number(StarOfDayNav,'#.00')"/>          
          </StartofdayAUM>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>