<?xml version="1.0" encoding="UTF-8"?>
										<!-- Object -Trade Recon for GS, Date -01-11-2012(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL12 = 'OP' and COL1 != 'DATA : Custody Positions with Aggregated CFDs (Exp Price)' and COL1 != 'Advisor:' and COL1 != 'Fund:' and COL1 != 'Business Date:' and COL1 != 'Run Date:' and COL1 != 'Report Code:' and COL1 != 'Custody Group Mnemonic' ">
					<PositionMaster>
						
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>
 
						<!--Need to Add here PB Name From FundMapping.xml-->
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name="PB_COMPANY_NAME" select="COL11"/>


						<xsl:variable name="PRIME_SYMBOL_NAME">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<PBSymbol>
							<xsl:value-of select="COL41"/>
						</PBSymbol>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@CompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
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
									<xsl:value-of select="COL41"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:choose>
							<xsl:when test ="COL15 = 'L' ">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL15 = 'S' ">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="COL15"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<!--BEGIN FOR NET POSITION ie QUANTITY -->

						<xsl:choose>
							<xsl:when test ="number(COL13)">
								<Quantity>
									<xsl:value-of select="COL13"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>
						
						
						<!--<xsl:choose>
							<xsl:when test ="COL15 = 'L' ">
								<Quantity>
									<xsl:value-of select="COL13"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL15 = 'S' ">
								<Quantity>
									<xsl:value-of select="COL13*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>-->

						<xsl:choose>
							<xsl:when test="number(COL23) and  number(COL23) &lt; 0">
								<MarkPrice>
									<xsl:value-of select= "COL23 *(-1)"/>
								</MarkPrice>
							</xsl:when>
							<xsl:when test="number(COL23) and  number(COL23) &gt; 0">
								<MarkPrice>
									<xsl:value-of select= "COL23"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select= "0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<!--For SEDOL Search-->
						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
