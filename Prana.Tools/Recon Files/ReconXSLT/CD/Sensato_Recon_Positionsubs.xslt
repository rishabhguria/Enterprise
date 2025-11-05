<?xml version="1.0" encoding="UTF-8"?>

								<!--Description- Sansato Position Recon
								Date-07-12-2011
								-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="COL1 != 'Account GLI' and number(COL74)!=0">				
				<PositionMaster>

					<!--fundname section-->
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL1"/>
					</xsl:variable>
					
					<xsl:variable name="PRANA_FUND_NAME">

						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						<!--<xsl:choose>
							<xsl:when test ="COL1='9530857'">
								<xsl:value-of select ="'9530857-Swap'"/>
							</xsl:when>
							<xsl:when test="COL1='9584878'">
								<xsl:value-of select ="'S1_5870004_SWAP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:otherwise>
						</xsl:choose>-->
					</xsl:variable>
					
					
					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
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

					<xsl:choose>
						<xsl:when test="number(COL74) &lt; 0">
							<Side>
								<xsl:value-of select="'Sell'"/>
							</Side>
							<Quantity>
								<xsl:value-of select="COL74"/>
							</Quantity>
						</xsl:when>
						<xsl:when test="number(COL74) &gt; 0">
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
							<Quantity>
								<xsl:value-of select="COL74"/>
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
						<xsl:when test ="number(COL64) and number(COL64) &lt; 0 ">
							<MarkPrice>
								<xsl:value-of select="COL64 * (-1)"/>
							</MarkPrice>
						</xsl:when>
						<xsl:when test ="number(COL64) and number(COL64) &gt; 0 ">
							<MarkPrice>
								<xsl:value-of select="COL64"/>
							</MarkPrice>
						</xsl:when>
						<xsl:otherwise>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
						</xsl:otherwise>
					</xsl:choose>
						<!-- Symbol Section-->
					<xsl:variable name="PB_COMPANY_NAME" select="translate(COL14,'&quot;','')"/>

       					<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SENSATO']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
					
						<!--<CompanyName>
							<xsl:value-of select='COL12'/>
						</CompanyName>-->
					
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>								
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>							
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="COL37"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>

					<!-- [RG : 2012,Oct'22] Added Market value columns-->
					<MarketValue>
						<xsl:choose>
							<xsl:when test="number(COL59)">
								<xsl:value-of select="COL59"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MarketValue>

					<MarketValueBase>
						<xsl:choose>
							<xsl:when test="number(COL58)">
								<xsl:value-of select="COL58"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MarketValueBase>

						

					<SMRequest>
						<xsl:value-of select ="'TRUE'"/>
					</SMRequest>
					
				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
