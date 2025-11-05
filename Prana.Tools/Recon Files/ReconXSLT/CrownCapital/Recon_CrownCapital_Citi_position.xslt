<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="PositionMaster">
				<xsl:if test ="COL1 != 'cobDateMM/DD/YYYY'">

					<PositionMaster>
						<xsl:choose>
							<xsl:when test ="COL16 !=''">

								<Symbol>
									<xsl:value-of select="translate(COL16,' ','')"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select="translate(COL16,' ','')"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when test="COL24 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL24*(-1)"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL24 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL24"/>
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
							<xsl:when test ="boolean(number(COL29))">
								<MarkPrice>
									<xsl:value-of select="COL29"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<MarketValue>
							<xsl:value-of select="COL30"/>
						</MarketValue>

						<MarketValueBase>
							<xsl:value-of select="COL33"/>
						</MarketValueBase>

						<CompanyName>
							<xsl:value-of select="translate(COL20,' ','')"/>
						</CompanyName>

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL6,' ','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CITI']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
