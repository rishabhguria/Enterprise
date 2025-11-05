<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <xsl:if test ="COL1 != 'Symbol' ">

          <PositionMaster>

			  <xsl:variable name="PB_FUND_NAME">
				  <xsl:value-of select="COL4"/>
			  </xsl:variable>

			  <xsl:variable name ="PRANA_FUND_NAME">
				  <!--<xsl:value-of select ="document('../ReconMappingXml/FundMapping_AScheme/FundMapping.xml')/FundMapping/PB[@Name='DEMO']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>-->
				  <xsl:value-of select ="document('../ReconMappingXml/FundMapping_AScheme.xml')/FundMapping/PB[@Name='DEMO']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

			  <AccountName>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_FUND_NAME !=''">
						  <xsl:value-of select ="$PRANA_FUND_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_FUND_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </AccountName>

            <!--<LongName>
              <xsl:value-of select ="COL9"/>
            </LongName>-->

            <!--<SEDOL>
              <xsl:value-of select ="COL10"/>
            </SEDOL>

            <Bloomberg>
              <xsl:value-of select ="COL11"/>
            </Bloomberg>-->

            <Symbol>
              <xsl:value-of select ="COL1"/>
            </Symbol>

            <Quantity>
              <xsl:value-of select ="COL3"/>
            </Quantity>

            <!--<RoundLot>
              <xsl:value-of select ="COL14"/>
            </RoundLot>-->
          
             <AllocationBasedOn>
              <xsl:value-of select ="'Symbol'"/>
            </AllocationBasedOn>

            <OrderSideTagValue>
              <xsl:choose>
                <xsl:when test="COL2 = 'Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Sell short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Buy to Open'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Buy to Close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Sell to Open'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Sell to Close'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderSideTagValue>
            
       

            <TradeType>
              <xsl:value-of select ="''"/>
            </TradeType>

            <!--<Currency>
              <xsl:value-of select ="COL7"/>
            </Currency>-->

            <!--<PB>
              <xsl:value-of select ="COL4"/>
            </PB>-->

            <AllocationSchemeKey>
              <xsl:value-of select ="'SymbolSide'"/>
            </AllocationSchemeKey>

            <!--<SMMappingReq>
              <xsl:value-of select="'SecMasterMapping.xml'"/>
            </SMMappingReq>-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>


