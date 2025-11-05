<?xml version="1.0" encoding="UTF-8"?>
										<!-- Object -Trade Recon for DB SWAP Date -11-01-2012(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL1 != 'AsOfDate'">
					<PositionMaster>
						
						<!--Need To Ask-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="(COL5)"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select="(COL20)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:choose>
								<!--<xsl:when test ="$PB_FUND_NAME='32577'">
									<xsl:value-of select ="'S1_32577_SWAP'"/>
								</xsl:when>-->
								<xsl:when test="$PB_FUND_NAME='32862' or $PB_FUND_NAME='33153'">
									<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME and @Currency = $PB_CURRENCY_NAME]/@PranaFund"/>

								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
								</xsl:otherwise>
							</xsl:choose>
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
						</xsl:choose >
						
						<xsl:variable name="PB_COMPANY_NAME" select="COL14"/>

						<PBSymbol>
							<xsl:value-of select="COL19"/>
						</PBSymbol>
						
						<!--Need To Add PB Name From  SymbolMapping.xml -->
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SENSATO']/SymbolData[@CompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
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
									<xsl:value-of select="COL19"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--Need To Ask-->
						<xsl:choose>
							<xsl:when test="COL41='SS'">
								<Side>
									<xsl:value-of select="'Sell short'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL41='SL'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL41='BC'">
								<Side>
									<xsl:value-of select="'Buy to Close'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL41='BL'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="COL41"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>						
						
						<!--BEGIN FOR NET POSITION ie QUANTITY -->
						<xsl:choose>
							<xsl:when  test="number(COL27) and COL27 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL27 * (-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when  test="number(COL27) and COL27 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL27"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="'0'"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!--Priec in Base-->
						
						<AvgPX>
							<xsl:value-of select= "COL31"/>
						</AvgPX>


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

						<!--Need To Ask.....-->
						<Fees>
							<xsl:value-of select="0"/>
						</Fees>				
						
						<!--GROSS NOTIONAL-->						
						<GrossNotionalValue>
							<xsl:value-of select="0"/>
						</GrossNotionalValue>
							
						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test="number(COL32) and COL32 &lt; 0 ">
								<NetNotionalValue>
									<xsl:value-of select="COL32 *(-1)"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:when test="number(COL32) and COL32 &gt; 0 ">
								<NetNotionalValue>
									<xsl:value-of select="COL32"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
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
