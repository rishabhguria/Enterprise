<?xml version="1.0" encoding="UTF-8"?>
										<!-- Object -Trade Recon for GS, Date -01-11-2012(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL18 = 'EQ' and COL1 != 'DATA: Custody Transaction (with Header)' and COL1 != 'Advisor:' and COL1 != 'Fund:' and COL1 != 'Business Date:' and COL1 != 'Run Date:' and COL1 != 'Report Code:' and COL1 != 'Client ID' ">
					<PositionMaster>
						
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL8"/>
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
						
						<xsl:variable name="PB_COMPANY_NAME" select="COL17"/>


						<xsl:variable name="PRIME_SYMBOL_NAME">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<PBSymbol>
							<xsl:value-of select="COL43"/>
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
									<xsl:value-of select="COL43"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:choose>
							<xsl:when test ="COL5 ='BUY' ">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL5 ='BUY TO OPEN' ">
								<Side>
									<xsl:value-of select="'Buy to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL5 ='BCOV' or COL5 ='BUY TO CLOSE'">
								<Side>
									<xsl:value-of select="'Buy to Close'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL5 ='SSEL' ">
								<Side>
									<xsl:value-of select="'Sell short'"/>
								</Side>
							</xsl:when>							
							<xsl:when test ="COL5 ='SELL' ">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL5 ='SELL TO OPEN' ">
								<Side>
									<xsl:value-of select="'Sell to Open'"/>
								</Side>
							</xsl:when>							
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="COL5"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>


						
						
						<!--BEGIN FOR NET POSITION ie QUANTITY -->
						<xsl:choose>
							<xsl:when  test="number(COL30) and COL30 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL30 * (-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when  test="number(COL30) and COL30 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL30"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="'0'"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Value in Local Currency-->
						<xsl:choose>
							<xsl:when test="number(COL29) and  number(COL29) &lt; 0">
								<AvgPX>
									<xsl:value-of select= "COL29 *(-1)"/>
								</AvgPX>
							</xsl:when>
							<xsl:when test="number(COL29) and  number(COL29) &gt; 0">
								<AvgPX>
									<xsl:value-of select= "COL29"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select= "0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Commission Value in Local Currency-->
						<xsl:choose>
							<xsl:when test="number(COL35) and COL35 &lt; 0 ">
								<Commission>
									<xsl:value-of select="COL35 *(-1)"/>
								</Commission>
							</xsl:when>
							<xsl:when test="number(COL35) and COL35 &gt; 0 ">
								<Commission>
									<xsl:value-of select="COL35"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!-- FEES Value in Local Currency-->
						<xsl:choose>
							<xsl:when test="number(COL36) and COL36 &lt; 0 ">
								<Fees>
									<xsl:value-of select="COL36 *(-1)"/>
								</Fees>
							</xsl:when>
							<xsl:when test="number(COL36) and COL36 &gt; 0 ">
								<Fees>
									<xsl:value-of select="COL36"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--GROSS NOTIONAL in Local Currency-->

						<xsl:choose>
							<xsl:when test="number(COL34) and COL34 &lt; 0 ">
								<GrossNotionalValue>
									<xsl:value-of select="COL34 *(-1)"/>
								</GrossNotionalValue>
							</xsl:when>
							<xsl:when test="number(COL34) and COL34 &gt; 0 ">
								<GrossNotionalValue>
									<xsl:value-of select="COL34"/>
								</GrossNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<GrossNotionalValue>
									<xsl:value-of select="0"/>
								</GrossNotionalValue>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--NET NOTIONAL in Local Currency -->

						<xsl:choose>
							<xsl:when test="number(COL31) and COL31 &lt; 0 ">
								<NetNotionalValueLocal>
									<xsl:value-of select="COL31 *(-1)"/>
								</NetNotionalValueLocal>
							</xsl:when>
							<xsl:when test="number(COL31) and COL31 &gt; 0 ">
								<NetNotionalValueLocal>
									<xsl:value-of select="COL31"/>
								</NetNotionalValueLocal>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValueLocal>
									<xsl:value-of select="0"/>
								</NetNotionalValueLocal>
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
