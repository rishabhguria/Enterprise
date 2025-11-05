<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">

        <xsl:variable name ="varAsset">
          <xsl:value-of select ="COL19"/>
        </xsl:variable>
        <xsl:if test ="$varAsset='EQTY' or $varAsset='EQTYOPT' or $varAsset = 'FUND' or $varAsset ='CONV' or $varAsset = 'CORP' or $varAsset = 'WRNT'">

					<PositionMaster>
						<!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Sonoma']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

						<xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME != ''">
                <FundName>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </FundName>
              </xsl:when>
              
              <xsl:when test ="COL1='SONOMALP'and ($PB_FUND_NAME='SONOMAFF_HSBC_PB_0000' or $PB_FUND_NAME ='SONOMALP_HSBC_BK_CHK' or $PB_FUND_NAME ='SONOMALP_HSBC_BK_MM' or $PB_FUND_NAME ='SONOMALTD_HSBC_BK_CKOF')">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_H'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD'and ($PB_FUND_NAME='SONOMAFF_HSBC_PB_0000' or $PB_FUND_NAME ='SONOMALP_HSBC_BK_CHK' or $PB_FUND_NAME ='SONOMALP_HSBC_BK_MM' or $PB_FUND_NAME ='SONOMALTD_HSBC_BK_CKOF')">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_H'"/>
                </FundName>
              </xsl:when>
              
              <xsl:when test ="COL1='SONOMALP'and $PB_FUND_NAME='SONOMALP_IB_PB_3965'">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_IB'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD'and $PB_FUND_NAME='ZZZ_Interactive Brokers'">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_IB'"/>
                </FundName>
              </xsl:when>


              <xsl:when test ="COL1='SONOMALP'and $PB_FUND_NAME='SONOMALP_JFF_PB_0000'">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_J'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD'and $PB_FUND_NAME ='SONOMALTD_JFF_PB_0037'">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_J'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="COL1='SONOMALP'and $PB_FUND_NAME='SONOMALP_BNY_PB_1797'">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_P'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD'and $PB_FUND_NAME='SSONOMALTD_BNY_PB_1813'">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_P'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="COL1='SONOMALP'and $PB_FUND_NAME='ZZZ_Charles Schwab'">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_C'"/>
                </FundName>
              </xsl:when>
              
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="number(COL6) &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL6 *(-1)"/>
								</Quantity>
							</xsl:when>
              <xsl:when test ="number(COL6) &gt; 0">
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
                <Quantity>
                  <xsl:value-of select="COL6"/>
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

            <xsl:choose>
              <xsl:when test ="number(COL7)">
                <AvgPX>
                  <xsl:value-of select="COL7"/>
                </AvgPX>
              </xsl:when>
              <xsl:otherwise>
                <AvgPX>
                  <xsl:value-of select="0"/>
                </AvgPX>
              </xsl:otherwise>
            </xsl:choose>
						

						<!-- Symbol Section-->

            <xsl:variable name="PB_COMPANY_NAME" select="COL4"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Sonoma']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <CompanyName>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </CompanyName>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:when test ="$varAsset='EQTY' or $varAsset='FUND'">
                <Symbol>
                  <xsl:value-of select ="substring-before(COL14,' ')"/>
                </Symbol>
              </xsl:when>
              <xsl:when test ="$varAsset='EQTYOPT'">
                <Symbol>
                  <xsl:value-of select ="translate(COL14,'+',' ')"/>
                </Symbol>
              </xsl:when>
              <xsl:when test ="$varAsset='CONV' or $varAsset='CORP' or $varAsset='WRNT'">
                <Symbol>
                  <xsl:value-of select ="COL4"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="COL14"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >

            <PBSymbol>
              <xsl:value-of select="COL14"/>
            </PBSymbol>
            

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
