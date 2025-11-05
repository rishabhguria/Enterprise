<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/NewDataSet">
    <DocumentElement>
      <xsl:for-each select="Comparision">
        <xsl:variable name="QUANTITY" select="Column11"/>
        <xsl:variable name="PB_DISTRIBUTION_ID" select="translate(Column2,'&quot;','')"/>
        <xsl:if test="$PB_DISTRIBUTION_ID = 'lucas' and $QUANTITY != 0 ">
          <PositionMaster>
         
            <xsl:variable name="PB_COMPANY_NAME" select="translate(Column8,'&quot;','')"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                <Symbol>
                  <xsl:value-of select='$PRANA_SYMBOL_NAME'/>
                </Symbol>
              </xsl:when>
              <xsl:when test="starts-with(Column7,'$')">
                <xsl:variable name = "varLength" >
                  <xsl:value-of select="string-length(Column7)"/>
                </xsl:variable>
                <Symbol>
                  <xsl:value-of select="concat(substring(Column7,2,($varLength - 3)),' ',substring(Column7,($varLength - 1),$varLength))"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select='Column7'/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose>

			  <xsl:choose>
				  <xsl:when test="Column11 &lt; 0">
					  <Side>
						  <xsl:value-of select="'Sell'"/>
					  </Side>
					  <Quantity>
						  <xsl:value-of select="Column11*(-1)"/>
					  </Quantity>					  
				  </xsl:when>
				  <xsl:when test="Column11 &gt; 0">
					  <Side>
						  <xsl:value-of select="'Buy'"/>
					  </Side>
					  <Quantity>
						  <xsl:value-of select="Column11"/>
					  </Quantity>					  
				  </xsl:when>
				  <xsl:otherwise>
					  <Side>
						  <xsl:value-of select="''"/>
					  </Side>
					  <Quantity>
						  <xsl:value-of select="0"/>
					  </Quantity>
				  </xsl:otherwise>
			  </xsl:choose>
           

            <CompanyName>
              <xsl:value-of select='Column8'/>
            </CompanyName>

            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="translate(Column3,'&quot;','')"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/FundData[@GSFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <FundName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </FundName>

              </xsl:otherwise>
            </xsl:choose>

            <CostBasis>
              <xsl:value-of select="Column10"/>
            </CostBasis>

            <PBSymbol>
              <xsl:value-of select="Column7"/>
            </PBSymbol>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
