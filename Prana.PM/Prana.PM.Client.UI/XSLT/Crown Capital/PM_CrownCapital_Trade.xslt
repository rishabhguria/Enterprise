<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<PositionMaster>
					<!--<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="translate(COL1,'&quot;','')"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
					</xsl:choose >-->

          <FundName>
            <xsl:value-of select="''"/>
          </FundName>

          <Symbol>
            <xsl:value-of select="COL2"/>
          </Symbol>
          
          <xsl:choose>
            <xsl:when  test="boolean(number(COL5))">
              <CostBasis>
                <xsl:value-of select="COL5"/>
              </CostBasis>
            </xsl:when >
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose >
          <xsl:choose>
                        
            <xsl:when test="COL3 != 'Trade Date'">
              <PositionStartDate>
								<xsl:value-of select="COL3"/>
							</PositionStartDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionStartDate>
								<xsl:value-of select="''"/>
							</PositionStartDate>
							
						</xsl:otherwise>
					</xsl:choose>

          <xsl:choose>
            <xsl:when test="COL6 &lt; 0">
              <NetPosition>
                <xsl:value-of select="COL6*(-1)"/>
              </NetPosition>
            </xsl:when>
            <xsl:when test="COL6 &gt; 0">
              <NetPosition>
                <xsl:value-of select="COL6"/>
              </NetPosition>
            </xsl:when>
            <xsl:otherwise>
              <NetPosition>
                <xsl:value-of select="0"/>
              </NetPosition>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test="COL9 &lt; 0">
              <Commission>
                <xsl:value-of select="COL9*(-1)"/>
              </Commission>
            </xsl:when>
            <xsl:when test="COL9 &gt; 0">
              <Commission>
                <xsl:value-of select="COL9"/>
              </Commission>
            </xsl:when>
            <xsl:otherwise>
              <Commission>
                <xsl:value-of select="0"/>
              </Commission>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test="COL13='EquityOption' and normalize-space(COL4) = 'SL'">
              <SideTagValue>
                <!-- Sell to Open-->
                <xsl:value-of select="'C'"/>               
              </SideTagValue>
            </xsl:when>
            <xsl:when test="COL13='EquityOption' and normalize-space(COL4) = 'CS'">
              <SideTagValue>
                <!-- Sell to Open-->
                <xsl:value-of select="'B'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:when test="COL13='Equity' and normalize-space(COL4) = 'SS'">
              <SideTagValue>
                <!-- Sell to Open-->
                <xsl:value-of select="'5'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:when test="COL13='Equity' and normalize-space(COL4) = 'CS'">
              <SideTagValue>
                <!-- Sell to Open-->
                <xsl:value-of select="'B'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:when test="COL13='Equity' and normalize-space(COL4) = 'BL'">
              <SideTagValue>
                <!-- Sell to Open-->
                <xsl:value-of select="'1'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:when test="COL13='Equity' and normalize-space(COL4) = 'SL'">
              <SideTagValue>
                <!-- Sell to Open-->
                <xsl:value-of select="'2'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:otherwise>
              <SideTagValue>
                <xsl:value-of select="''"/>
              </SideTagValue>
            </xsl:otherwise>
          </xsl:choose>
					
			</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


