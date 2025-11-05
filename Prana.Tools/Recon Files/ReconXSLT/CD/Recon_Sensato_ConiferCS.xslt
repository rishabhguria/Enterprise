<?xml version="1.0" encoding="UTF-8"?>
											<!--
											Description: Sensato Recon
											Date :		 17-02-2012
											-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL2 !='InvestID' and COL2 !='CURRENCY FORWARDS' and COL2 !='Cash and Equivalents' ">
					<PositionMaster>

						
						<!--<xsl:variable name = "PB_FUND_NAME" >						
								<xsl:value-of select="COL47"/>						
						</xsl:variable>
																
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>-->

						<!--<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">-->
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							<!--</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>-->

						<xsl:variable name="PB_COMPANY_NAME" select="COL5"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Description>
							<xsl:value-of select ="$PB_COMPANY_NAME"/>
						</Description>

						<xsl:variable name="varBloomberg">
							<xsl:choose>
								<!-- contains(COL9,'.') !=false-->
								<xsl:when test ="COL2 = 'COMMON STOCK' or COL2 = 'DEPOSITORY RECEIPT'">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:when test ="COL2 = 'COMMON STOCK'">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:when test ="COL2 = 'EQUITY SWAPS' or COL2 = 'CONTRACT FOR DIFFERENCE'">
									<xsl:value-of select="substring-before(COL4,'.')"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<Bloomberg>
									<xsl:value-of select="''"/>
								</Bloomberg>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<Bloomberg>
									<xsl:value-of select="$varBloomberg"/>
								</Bloomberg>
							</xsl:otherwise>
						</xsl:choose >						

						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<PBAssetName>
							<xsl:value-of select="COL2"/>
						</PBAssetName>

	
						<xsl:variable name ="varQuantity">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="number($varQuantity) and $varQuantity &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="number($varQuantity) and $varQuantity &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="number($varQuantity)">
								<Quantity>
									<xsl:value-of select="$varQuantity"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="number(COL9) and  number(COL9) &lt; 0">
								<MarkPrice>
									<xsl:value-of select= "COL9 *(-1)"/>
								</MarkPrice>
							</xsl:when>
							<xsl:when test="number(COL9) and  number(COL9) &gt; 0">
								<MarkPrice>
									<xsl:value-of select= "COL9"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select= "0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>



						<xsl:choose>
							<xsl:when test="number(COL11) and  number(COL11) &lt; 0">
								<MarketValue>
									<xsl:value-of select= "COL11 *(-1)"/>
								</MarketValue>
							</xsl:when>
							<xsl:when test="number(COL11) and  number(COL11) &gt; 0">
								<MarketValue>
									<xsl:value-of select= "COL11"/>
								</MarketValue>
							</xsl:when>
							<xsl:otherwise>
								<MarketValue>
									<xsl:value-of select= "0"/>
								</MarketValue>
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
