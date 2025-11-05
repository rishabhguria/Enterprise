<?xml version="1.0" encoding="utf-8"?>
													

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL3 !='L/S'">		
				<PositionMaster>



					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="''"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CustomHouse']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME !=''">
							<AccountName>
								<xsl:value-of select="$PB_FUND_NAME"/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</AccountName>
						</xsl:otherwise>
					</xsl:choose>


					<Symbol>
						<xsl:value-of select="''"/>
					</Symbol>

					<SEDOL>
						<xsl:value-of select="substring-before(COL4,'CFDUSD')"/>
					</SEDOL>

					<PBSymbol>
						<xsl:value-of select ="''"/>
					</PBSymbol>

					<!--Side tag value Means By or Sell-->
					<xsl:choose>
						<xsl:when test="number(COL6) &lt; 0 ">
							<SideTagValue>
								<xsl:value-of select="5"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="number(COL6) &gt; 0">
							<SideTagValue>
								<xsl:value-of select="1"/>
							</SideTagValue>
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="0"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose>


					<!--NetPosition Means Qty-->
					<xsl:choose>
						<xsl:when test="number(COL6) and number(COL6) &lt; 0 ">
							<NetPosition>
								<xsl:value-of select="normalize-space(COL6) *(-1)"/>
							</NetPosition>
						</xsl:when>
						<xsl:when test=" number(COL6) and number(COL6) &gt; 0">
							<NetPosition>
								<xsl:value-of select="normalize-space(COL6)"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="'0'"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>


					<!--Position Start Date-->
					<PositionStartDate>
						<!--<xsl:value-of select="concat(substring(COL12,5,2),'/',substring(COL12,7,2),'/',substring(COL12,1,4))"/>-->
						<xsl:value-of select="''"/>
					</PositionStartDate>

					<!--Prise ie CostBasis-->
					<xsl:choose>
						<xsl:when test="number(COL7) and number(COL7) &lt; 0 ">
							<CostBasis>
								<xsl:value-of select="number(COL7) *(-1)"/>
							</CostBasis>
						</xsl:when>
						<xsl:when test="number(COL7) and number(COL7) &gt; 0 ">
							<CostBasis>
								<xsl:value-of select="number(COL7)"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="'0'"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>
					
					<xsl:choose>
					<xsl:when test="number(COL10)">
						<FXRate>
							<xsl:value-of select="COL10"/>
						</FXRate>					
					  </xsl:when>
						<xsl:otherwise>
						<FXRate>
							<xsl:value-of select="0"/>
						</FXRate>
						</xsl:otherwise>
					</xsl:choose>

					<FXConversionMethodOperator>
						<xsl:value-of select="'D'"/>
					</FXConversionMethodOperator>
												
					<!--For Description-->
					<Description>
						<xsl:value-of select="COL15"/>
					</Description>
				</PositionMaster>
			</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>