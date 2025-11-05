<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL1 != 'Trade Date'">
					<PositionMaster>
						<!--   Fund -->
						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jasincap']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>-->


						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>
						<xsl:choose>
							<xsl:when test ="COL4='Buy'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL4='Sell'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL4='Sell Short'">
								<Side>
									<xsl:value-of select="'Sell short'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL4='Cover Short'">
								<Side>
									<xsl:value-of select="'Buy to Close'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL14*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL14"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL12))">
								<AvgPX>
									<xsl:value-of select="COL12"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<NetNotionalValue>
									<xsl:value-of select="(COL12 * COL14*(-1)) - (COL15 + COL16 + COL17) "/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:when test ="COL15 &gt; 0">
								<NetNotionalValue>
									<xsl:value-of select="(COL12 * COL14) + (COL15 + COL16 + COL17)"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>



						<!--COMMISSION-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<Commission>
									<xsl:value-of select="COL15"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<Fees>
							<xsl:value-of select="COL16 + COL17"/>
						</Fees>

						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL3)"/>

						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<PBSymbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</PBSymbol>
						<Symbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</Symbol >

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
