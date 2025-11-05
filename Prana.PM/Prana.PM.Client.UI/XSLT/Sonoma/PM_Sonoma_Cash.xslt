<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>	
			
			<xsl:for-each select="//PositionMaster">
				<xsl:if test =" COL1 != 'Portfolio'" >

					<PositionMaster>
            <!--   Fund -->
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
              <xsl:when test ="COL1='SONOMALP' and ($PB_FUND_NAME='SONOMAFF_HSBC_PB_0000' or $PB_FUND_NAME='SONOMALP_HSBC_BK_CHK' or $PB_FUND_NAME = 'SONOMALP_HSBC_BK_MM')">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_H'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD' and ($PB_FUND_NAME='SONOMAFF_HSBC_PB_0000' or $PB_FUND_NAME='SONOMALTD_HSBC_BK_CKOF') ">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_H'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="COL1='SONOMALP' and ($PB_FUND_NAME='SONOMALP_IB_PB_3965' or $PB_FUND_NAME='ZZZ_Interactive Brokers')">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_IB'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD' and $PB_FUND_NAME='ZZZ_Interactive Brokers' ">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_IB'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="COL1='SONOMALP' and ($PB_FUND_NAME='SONOMALP_JFF_PB_0000')">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_J'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD' and $PB_FUND_NAME='SONOMALTD_JFF_PB_0037' ">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_J'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="COL1='SONOMALP' and ($PB_FUND_NAME='SONOMALP_BNY_PB_1797')">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_P'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="COL1='SONOMALTD' and $PB_FUND_NAME='SONOMALTD_BNY_PB_1813' ">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_P'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="COL1='SONOMALP' and $PB_FUND_NAME='ZZZ_Charles Schwab' ">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_C'"/>
                </FundName>
              </xsl:when>

              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

            <xsl:choose>
              <xsl:when test ="COL4 != 'Currency'">
                <LocalCurrency>
                  <xsl:value-of select="COL4"/>
                </LocalCurrency>
              </xsl:when>
              <xsl:otherwise>
                <LocalCurrency>
                  <xsl:value-of select="''"/>
                </LocalCurrency>
              </xsl:otherwise>
            </xsl:choose>

            <CashValueBase>
              <xsl:value-of select="0"/>
            </CashValueBase>
						
						<xsl:choose>
							<xsl:when test ="number(COL7)">
								<CashValueLocal>
									<xsl:value-of select="COL7"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >
					
						<Date>
							<xsl:value-of select="COL10"/>
						</Date>
            
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
            
					</PositionMaster>
        </xsl:if>
      </xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>
