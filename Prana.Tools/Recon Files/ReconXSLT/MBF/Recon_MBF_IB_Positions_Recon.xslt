<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				
				<xsl:if test ="COL1 ='POST' and COL23='SUMMARY'">

					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='IB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL13 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL13*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL13 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL13"/>
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

						<AvgPX>
							<xsl:value-of select="COL18"/>
						</AvgPX>

						<MarkPrice>
							<xsl:value-of select ="COL15"/>
						</MarkPrice>

						<!-- Symbol Section-->

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:variable name="PB_COMPANY_NAME" select="COL7"/>

						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='IB']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME != ''">								
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>							
								<Symbol>
									<xsl:value-of select="COL6"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
