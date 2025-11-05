<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="PositionMaster">
					<PositionMaster>
            
            <xsl:choose>
              <xsl:when test ="COL2 !='SECURITY_NUMBER'">
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <CUSIP>
                  <xsl:value-of select="translate(COL2,' ','')"/>
                </CUSIP>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <CUSIP>
                  <xsl:value-of select="''"/>
                </CUSIP>
              </xsl:otherwise>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="translate(COL2,' ','')"/>
            </PBSymbol>

            <xsl:choose>
              <xsl:when test="COL7 &lt; 0">
                <Quantity>
                  <xsl:value-of select="COL7*(-1)"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL7 &gt; 0">
                <Quantity>
                  <xsl:value-of select="COL7"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="''"/>
                </Side>
              </xsl:otherwise>
            </xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL11))">
								<MarkPrice>
									<xsl:value-of select="COL11"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>
            
						<MarketValue>
							<xsl:value-of select="COL13"/>
						</MarketValue>
            
						<CompanyName>
							<xsl:value-of select="translate(COL3,' ','')"/>
						</CompanyName>

            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="translate(COL1,' ','')"/>
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
            </xsl:choose>

				      <SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
