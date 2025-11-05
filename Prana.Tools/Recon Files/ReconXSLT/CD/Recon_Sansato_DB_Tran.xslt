<?xml version="1.0" encoding="UTF-8"?>
										<!-- Object -Trade Recon for DB Date -01-11-2011(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL1 != 'ProcessDate' and COL20 != 'FX Rate'">
					<PositionMaster>
						
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<!--Add PB Name From Xml FundMapping.xml-->
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

						<!-- SYMBOL, PBSYMBOL, COMPANYNAME SECTION -->
						<xsl:variable name="PB_COMPANY_NAME" select="COL23"/>

						<PBSymbol>
							<xsl:value-of select="COL26"/>
						</PBSymbol>

						<!--NEED TO  Add PB Name From SymbolMapping.xml-->
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='======']/SymbolData[@CompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME !=''">
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
									<xsl:value-of select="COL26"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>

						<!--BEGIN FOR BUY AND SELL DESCRIPTION -->
						
						<xsl:choose>
							<xsl:when test ="COL16 ='SL' ">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL16 ='SO' ">
								<Side>
									<xsl:value-of select="'Sell to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL16 ='SC' ">
								<Side>
									<xsl:value-of select="'Sell to Close'"/>
								</Side>
							</xsl:when>							
							<xsl:when test ="COL16 ='SS' ">
								<Side>
									<xsl:value-of select="'Sell short'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL16 ='BY' ">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL16 ='BO' ">
								<Side>
									<xsl:value-of select="'Buy to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL16 ='BC' ">
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
												

						<!--BEGIN FOR NET POSITION ie QUANTITY -->
						<xsl:choose>
							<xsl:when  test="number(COL32) and COL32 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL32 * (-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when  test="number(COL32) and COL32 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL32"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="'0'"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Average Price-->
						<xsl:choose>
							<xsl:when test="number(COL33) and  number(COL33) &lt; 0">
								<AvgPX>
									<xsl:value-of select= "COL33 *(-1)"/>
								</AvgPX>
							</xsl:when>
							<xsl:when test="number(COL33) and  number(COL33) &gt; 0">
								<AvgPX>
									<xsl:value-of select= "COL33"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select= "0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Commission in Local Currency-->

						<xsl:choose>
							<xsl:when test="number(COL40) and  number(COL40) &lt; 0">
								<Commission>
									<xsl:value-of select= "COL40 *(-1)"/>
								</Commission>
							</xsl:when>
							<xsl:when test="number(COL40) and  number(COL40) &gt; 0">
								<Commission>
									<xsl:value-of select= "COL40"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select= "0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!--Fee in Local Currency -->

						<xsl:choose>
							<xsl:when test="number(COL41) and  number(COL41) &lt; 0">
								<Fees>
									<xsl:value-of select= "COL41 *(-1)"/>
								</Fees>
							</xsl:when>
							<xsl:when test="number(COL41) and  number(COL41) &gt; 0">
								<Fees>
									<xsl:value-of select= "COL41"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select= "0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--GROSS NOTIONAL in Local Currency-->

						<xsl:choose>
							<xsl:when test="number(COL37) and  number(COL37) &lt; 0">
								<GrossNotionalValue>
									<xsl:value-of select= "COL37 * (-1)"/>
								</GrossNotionalValue>
							</xsl:when>
							<xsl:when test="number(COL37) and  number(COL37) &gt; 0">
								<GrossNotionalValue>
									<xsl:value-of select= "COL37"/>
								</GrossNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<GrossNotionalValue>
									<xsl:value-of select= "0"/>
								</GrossNotionalValue>
							</xsl:otherwise>
						</xsl:choose>

						<!--<NetNotionalValue>
							<xsl:value-of select= "COL42 * (-1)"/>
						</NetNotionalValue>-->
						
						<!--NET NOTIONAL in Local Currency-->
						<!--<xsl:choose>-->
							<!--<xsl:when test="number(COL42) and  number(COL42) &lt; 0">
								<NetNotionalValue>
									<xsl:value-of select= "COL42 *(-1)"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:when test="number(COL42) and  number(COL42) &gt; 0">
								<NetNotionalValue>
									<xsl:value-of select= "COL42"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select= "0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>-->

						<xsl:choose>
						<xsl:when test="number(COL42) and  number(COL42) &lt; 0">
								<NetNotionalValueLocal>
									<xsl:value-of select= "COL42 *(-1)"/>
								</NetNotionalValueLocal>
							</xsl:when>
							<xsl:when test="number(COL42) and  number(COL42) &gt; 0">
								<NetNotionalValueLocal>
									<xsl:value-of select= "COL42"/>
								</NetNotionalValueLocal>

							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValueLocal>
									<xsl:value-of select= "0"/>
								</NetNotionalValueLocal>
							</xsl:otherwise>
						</xsl:choose>

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
