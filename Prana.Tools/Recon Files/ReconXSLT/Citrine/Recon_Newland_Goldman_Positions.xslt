<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">
	
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL7 != 'Trade Date Quantity' and COL7 != 0">
					<PositionMaster>
						
						<!--FUNDNAME SECTION-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<!-- SYMBOL, PBSYMBOL, COMPANYNAME SECTION -->
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL4,'&quot;','')"/>

						<PBSymbol>
							<xsl:value-of select="COL17"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
								</Symbol>
							</xsl:when>
							<xsl:when test="starts-with(COL5,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL5)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL5,2,($varLength - 3)),' ',substring(COL5,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select='COL5'/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="COL7 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL7*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL7 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL7"/>
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
							<xsl:when test ="boolean(number(COL10))">
								<MarkPrice>
									<xsl:value-of select="COL10"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<MarketValue>
							<xsl:value-of select="COL12"/>
						</MarketValue>
						<MarketValueBase>
							<xsl:value-of select="COL13"/>
						</MarketValueBase>




					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
